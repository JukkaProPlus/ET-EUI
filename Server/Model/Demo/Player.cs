namespace ET
{
	public enum PlayerState
	{
		DisConnect = 0,
		Gate = 1,
		Game = 2,
	}
	// [ObjectSystem]
	// public class PlayerSystem: AwakeSystem<Player, long,long>
	// {
	// 	public override void Awake(Player self, long a, long b)
	// 	{
	// 		self.Account = a;
	// 		self.UnitId = b;
	// 	}
	// }
	public sealed class Player : Entity, IAwake<string>, IAwake<long,long>,IDestroy
	{
		public long Account { get; set; }
		// public long SessionInstanceId { get; set; }
		public Session ClientSession { get; set; }
		
		public long UnitId { get; set; }
		public PlayerState playerState { get; set; }
	}
}