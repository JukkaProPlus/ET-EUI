using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class L2G_DisconnectGateUnitHandler : AMActorRpcHandler<Scene, L2G_DisconnectGateUnit, G2L_DisconnectGateUnit>
    {
        protected async override ETTask Run(Scene scene, L2G_DisconnectGateUnit request, G2L_DisconnectGateUnit response, Action reply)
        {
            long accountId = request.AccountId;
            using(await CoroutineLockComponent.Instance.Wait(CoroutineLockType.GateLoginLock, accountId.GetHashCode()))
            {
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player player = playerComponent.Get(accountId);
                if (player == null)
                {
                    reply();
                    return;
                } else
                {
                    playerComponent.Remove(accountId);
                    player.Dispose();
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
