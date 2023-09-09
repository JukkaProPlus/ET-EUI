namespace ET
{
    [FriendClass(typeof(Player))]
    public static class PlayerSystem
    {
        [ObjectSystem]
        public class PlayerAwakeSystem : AwakeSystem<Player, long,long>
        {

            public override void Awake(Player self, long a,long roleId)
            {
                self.Account = a;
                self.UnitId = roleId;
            }
        }
        
        [ObjectSystem]
        public class PlayerDestroySystem : DestroySystem<Player>
        {
            public override void Destroy(Player self)
            {
                self.Account = 0;
                self.UnitId    = 0;
                self.playerState = PlayerState.DisConnect;
                self.ClientSession?.Dispose();
            }
        }
    }
}