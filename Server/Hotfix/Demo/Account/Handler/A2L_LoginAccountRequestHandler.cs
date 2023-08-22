using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class A2L_LoginAccountRequestHandler : AMActorRpcHandler<Scene, A2L_LoginAccount, L2A_LoginAccount>
    {
        protected async override ETTask Run(Scene scene, A2L_LoginAccount request, L2A_LoginAccount response, Action reply)
        {
            long AccountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCenterLock, AccountId.GetHashCode()))
            {
                if (!scene.GetComponent<LoginInfoRecordComponent>().IsExist(AccountId))
                {
                    reply();
                    return;
                }
                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(AccountId);
                StartSceneConfig startSceneConfig = RealmGateAddressHelper.GetGate(zone, AccountId);

                G2L_DisconnectGateUnit g2L_DisconnectGateUnit = (G2L_DisconnectGateUnit)await MessageHelper.CallActor(startSceneConfig.InstanceId, new L2G_DisconnectGateUnit()
                {
                    AccountId = AccountId
                });
                response.Error = g2L_DisconnectGateUnit.Error;
                reply();

                
            }
            
        }
    }
}
