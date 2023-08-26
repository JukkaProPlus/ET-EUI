using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [FriendClass(typeof(RoleInfoState))]
    [FriendClass(typeof(RoleInfo))]
    public class C2A_DeleteRoleHandler : AMRpcHandler<C2A_DeleteRole, A2C_DeleteRole>
    {
        protected override async ETTask Run(Session session, C2A_DeleteRole request, A2C_DeleteRole response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误，当前Scene为{session.DomainScene().SceneType}");
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
            if (token == null || token != request.Token ) 
            {
                Log.Error("token错误");
                response.Error = ErrorCode.ERR_TokenError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }
            
            using (session.AddComponent<SessionLockingComponent>())
            {
                using(await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreateRoleLock, request.AccountId.GetHashCode()))
                {
                    //long id = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Remove<RoleInfo>(info => info.ServerId == request.ServerId && info.AccountId == request.AccountId && info.Id == request.RoleInfoId);
                    List<RoleInfo> roleInfos = await DBManagerComponent.Instance.GetZoneDB(request.ServerId).Query<RoleInfo>(d=>d.Id == request.RoleInfoId&&d.ServerId == request.ServerId && d.AccountId == request.AccountId);
                    if (roleInfos == null || roleInfos.Count() == 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNotExist;
                        reply();
                        return;
                    }
                    RoleInfo roleInfo = roleInfos[0];
                    session.AddChild(roleInfo);
                    roleInfo.State = (int)RoleInfoState.Freeze;
                    await DBManagerComponent.Instance.GetZoneDB(request.ServerId).Save<RoleInfo>(roleInfo);
                    response.DeletedRoleInfoId = roleInfo.Id;
                    roleInfo?.Dispose();
                    reply();
                }
            }
                await ETTask.CompletedTask;
        }
    }
}
