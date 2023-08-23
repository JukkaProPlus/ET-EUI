using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [FriendClass(typeof(RoleInfo))]
    public class C2A_CreateRoleHandler : AMRpcHandler<C2A_CreateRole, A2C_CreateRole>
    {
        protected async override ETTask Run(Session session, C2A_CreateRole request, A2C_CreateRole response, Action reply)
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
                //session?.Disconnect().Coroutine();
                return;
            }
            string token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);
            if (token == null || request.Token != token)
            {
                response.Error = ErrorCode.ERR_TokenError;
                reply();
                session?.Disconnect().Coroutine();
                return;
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                response.Error = ErrorCode.ERR_RoleNameError;
                reply();
                return;
            }
            //可能还需要对角色名进行长度，格式的校验，
            //以及敏感词的过滤，这里就先不做了

            using (session.AddComponent<SessionLockingComponent>())
            {
                //判断角色名是否已经存在
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreateRoleLock, request.AccountId.GetHashCode()))
                {
                    List<RoleInfo> roleList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<RoleInfo>(d => d.Name.Equals(request.Name) && d.ServerId == request.ServerId);
                    if (roleList != null && roleList.Count > 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNameExist;
                        reply();
                        return;
                    }
                    RoleInfo newRoleInfo = session.AddChildWithId<RoleInfo>(IdGenerater.Instance.GenerateUnitId(request.ServerId));
                    newRoleInfo.Name = request.Name;
                    newRoleInfo.State = (int)RoleInfoState.Normal;
                    newRoleInfo.ServerId = request.ServerId;
                    newRoleInfo.AccountId = request.AccountId;
                    newRoleInfo.CreateTime = TimeHelper.ServerNow();
                    newRoleInfo.LastLoginTime = 0;
                    //newRoleInfo.

                    await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<RoleInfo>(newRoleInfo);
                    response.RoleInfo = newRoleInfo.ToMessage();
                    reply();
                    newRoleInfo.Dispose();
                    return;
                }
            }
        }
    }
}
