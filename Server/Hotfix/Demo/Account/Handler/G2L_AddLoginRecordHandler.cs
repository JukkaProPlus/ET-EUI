using System;

namespace ET
{
    public class G2L_AddLoginRecordHandler:AMActorRpcHandler<Scene, G2L_AddLoginRecord, L2G_AddLoginRecord>
    {
        protected async override ETTask Run(Scene unit, G2L_AddLoginRecord request, L2G_AddLoginRecord response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCenterLock, request.AccountId.GetHashCode()))
            {
                unit.GetComponent<LoginInfoRecordComponent>().Remove(request.AccountId);
                unit.GetComponent<LoginInfoRecordComponent>().Add(request.AccountId, (int)request.ServerId);
                
            }

            reply();
            await ETTask.CompletedTask;
        }
    }
}