using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [FriendClassAttribute(typeof(ET.RoleInfo))]
    public class C2A_GetRolesHandler : AMRpcHandler<C2A_GetRoles, A2C_GetRoles>
    {
        protected override async ETTask Run(Session session, C2A_GetRoles request, A2C_GetRoles response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误，当前Scene为：{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeated;
                reply();
                return;
            }
            string token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);
            //string token = session.DomainScene().GetComponent<AccountInfosComponent>().Token
            if (token != request.Token)
            {
                response.Error = ErrorCode.ERR_TokenError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreateRoleLock, request.AccountId.GetHashCode()))
                {
                    var roleInfos = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<RoleInfo>((roleInfo) => roleInfo.AccountId == request.AccountId && roleInfo.ServerId == request.ServerId && roleInfo.State == (int)RoleInfoState.Normal);
                    if (roleInfos.Count == 0)
                    {
                        reply();
                        return;
                    }
                    else
                    {
                        foreach (var roleInfo in roleInfos)
                        {
                            response.RoleInfos.Add(roleInfo.ToMessage());
                            roleInfo?.Dispose();
                        }
                        roleInfos.Clear();
                        reply();
             
                    }
                }
            }
        }
    }
}
