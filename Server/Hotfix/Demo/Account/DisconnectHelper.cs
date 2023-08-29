using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public static class DisconnectHelper
    {
        public static async ETTask Disconnect(this Session self)
        {
            if (self == null || self.IsDisposed)
            {
                return;
            }
            long instanceId = self.InstanceId;
            await TimerComponent.Instance.WaitAsync(1000);
            if (self.InstanceId != instanceId)
            {
                return;
            }
            self.Dispose();
        }

        public static async ETTask KickPlayer(Player player)
        {
            if (player == null || player.IsDisposed)
            {
                return;
            }
            long instanceId = player.InstanceId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, player.Account.GetHashCode()))
            {
                if(player.IsDisposed || player.InstanceId != instanceId)
                {
                    return;
                }

                switch (player.playerState)
                {
                    case PlayerState.Game:
                        //todo 通知游戏逻辑服下线Unit角色逻辑，并将数据存入数据库
                        break;
                    case PlayerState.Gate:
                        break;
                    case PlayerState.DisConnect:
                        break;
                }

                player.playerState = PlayerState.DisConnect;
                player.DomainScene().GetComponent<PlayerComponent>().Remove(player.Account);
                player?.Dispose();
                await TimerComponent.Instance.WaitAsync(3000);
            }
            await ETTask.CompletedTask;
        }
    }
}
