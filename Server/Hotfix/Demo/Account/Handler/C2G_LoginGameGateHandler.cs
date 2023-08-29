using System;

namespace ET
{
    [FriendClass(typeof(SessionPlayerComponent))]
    public class C2G_LoginGameGateHandler:AMRpcHandler<C2G_LoginGameGate, G2C_LoginGameGate>
    {
        protected async override ETTask Run(Session session, C2G_LoginGameGate request, G2C_LoginGameGate response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Gate)
            {
                Log.Error($"当前SceneType为{session.DomainScene().SceneType}，不是Gate");
                session.Dispose();
                return;
            }
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeated;
                reply();
                return;
            }
            Scene domainScene = session.DomainScene();
            string token = domainScene.GetComponent<GateSessionKeyComponent>().Get(request.Account);
            if (token == null || token != request.Key)
            {
                response.Error = ErrorCode.ERR_ConnectGateKeyError;
                response.Message = "Gate Key验证失败";
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            domainScene.GetComponent<GateSessionKeyComponent>().Remove(request.Account);
            long instanceId = domainScene.InstanceId;
            

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, request.Account.GetHashCode()))
                {
                    if (instanceId != session.InstanceId)
                    {
                        return;
                    }
                    //通知登陆中心服，记录本次登陆的服务器zone
                    StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.LoginCenterConfig;
                    L2G_AddLoginRecord l2G_AddLoginRecord = (L2G_AddLoginRecord)await MessageHelper.CallActor(loginCenterConfig.InstanceId, new G2L_AddLoginRecord() { AccountId = request.Account, ServerId = domainScene.Zone, });
                    if (l2G_AddLoginRecord.Error != ErrorCode.ERR_Success)
                    {
                        Log.Error(l2G_AddLoginRecord.Error.ToString());
                        response.Error = l2G_AddLoginRecord.Error;
                        reply();
                        session?.Disconnect().Coroutine();
                        return;
                    }
                    Player player = domainScene.GetComponent<PlayerComponent>().Get(request.Account);
                    if (player == null)
                    {
                        //添加一个新的GateUnit
                        player = domainScene.GetComponent<PlayerComponent>()
                                .AddChildWithId<Player, long, long>(request.RoleId, request.Account, request.RoleId);
                        player.playerState = PlayerState.Gate;
                        domainScene.GetComponent<PlayerComponent>().Add(player);
                        session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
                    }
                    else
                    {
                        // player.RemoveComponent<PlayerOfflineOutTimeComponent>();
                        
                    }
                    session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;
                    session.GetComponent<SessionPlayerComponent>().PlayerInstanceId = player.InstanceId;
                    session.GetComponent<SessionPlayerComponent>().AccountId = player.Account;
                    player.SessionInstanceId = session.InstanceId;
                }
            }
            await ETTask.CompletedTask;
        }
    }
}