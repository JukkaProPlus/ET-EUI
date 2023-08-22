using NLog.LayoutRenderers.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ET
{
    [FriendClassAttribute(typeof(ET.Account))]
    class C2A_LoginAccountHandler : AMRpcHandler<C2A_LoginAccount, A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误，当前Scene为{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeated;
                reply();
                session.Disconnect().Coroutine();
                return;
            }
            if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_AccountOrPasswordError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }
            if (!Regex.IsMatch(request.AccountName.Trim(), @"^[0-9]*$"))
            {
                response.Error = ErrorCode.ERR_AccountNameInvalid;
                reply();
                session.Disconnect().Coroutine();
                return;
            }
            //if (!Regex.IsMatch(request.Password, @"^[0-9]*$"))
            //{
            //    response.Error = ErrorCode.ERR_PasswordInvalid;
            //    reply();
            //    session.Disconnect().Coroutine();
            //    return;
            //}
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountName.Trim().GetHashCode()))
                {
                    var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<Account>(d => d.AccountName.Equals(request.AccountName));
                    Account account = null;
                    if (null != accountInfoList && accountInfoList.Count == 0)
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.AccountName;
                        account.Password = request.Password;
                        account.CreateTime = TimeHelper.ServerNow();
                        account.AccountType = AccountType.General;
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<Account>(account);
                    } else
                    {
                        account = accountInfoList[0];
                        if (account.AccountType == AccountType.BlackList)
                        {
                            response.Error = ErrorCode.ERR_AccountBlocked;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }
                        if (!account.Password.Equals(request.Password))
                        {
                            response.Error = ErrorCode.ERR_PasswordError;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }
                        session.AddChild(account);
                    }

                    StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "LoginCenter");
                    long LoginCenterInstanceId = startSceneConfig.InstanceId;
                    L2A_LoginAccount l2a_LoginAccount = (L2A_LoginAccount)await ActorMessageSenderComponent.Instance.Call(LoginCenterInstanceId, new A2L_LoginAccount() { AccountId = account.Id });
                    if (l2a_LoginAccount.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = l2a_LoginAccount.Error;
                        reply();
                        session?.Disconnect().Coroutine();
                        account.Dispose();
                        return;
                    }
                    long sessionId = session.DomainScene().GetComponent<AccountSessionsComponent>().Get(account.Id);
                    Session otherSession = Game.EventSystem.Get(sessionId) as Session;
                    if (otherSession != null)
                    {
                        otherSession.Send(new A2C_Disconnect() { Error = 0 });
                        otherSession.Disconnect().Coroutine();
                    }

                    session.DomainScene().GetComponent<AccountSessionsComponent>().Add(account.Id, session.InstanceId);
                    session.AddComponent<AccountCheckOutTimeComponent, long>(account.Id);


                    string Token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue).ToString();
                    session.DomainScene().GetComponent<TokenComponent>().Remove(account.Id);
                    session.DomainScene().GetComponent<TokenComponent>().Add(account.Id, Token);
                    response.AccountId = account.Id;
                    response.Token = Token;
                    reply();
                    account?.Dispose();
                }
            }


            
        }
    }
}
