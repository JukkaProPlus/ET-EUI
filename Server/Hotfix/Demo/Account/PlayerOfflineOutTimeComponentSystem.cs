using System;
using MongoDB.Driver.Core.Events;

namespace ET
{
    [Timer(TimerType.PlayerOfflineOutTime)]
    public class PlayerOfflineTime: ATimer<PlayerOfflineOutTimeComponent>
    {
        public override void Run(PlayerOfflineOutTimeComponent self)
        {
            try
            {
                self.KickPlayer();
                
            }
            catch (Exception e)
            {
                Log.Error($"playeroffline timer error:{self.Id}\n{e}");
            }
        }
    }
    public class PlayerOfflineOutTimeComponentAwakeSystem:AwakeSystem<PlayerOfflineOutTimeComponent>
    {
        public override void Awake(PlayerOfflineOutTimeComponent self)
        {
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }
    public class PlayerOfflineOutTimeComponentDestroySystem:DestroySystem<PlayerOfflineOutTimeComponent>
    {
        public override void Destroy(PlayerOfflineOutTimeComponent self)
        {
            self.Timer = 0;
        }
    }

    public static class PlayerOfflineOutTimeComponentSystem
    {
        public static void KickPlayer(this PlayerOfflineOutTimeComponent self)
        {
            DisconnectHelper.KickPlayer(self.GetParent<Player>()).Coroutine();
        }
    }
}