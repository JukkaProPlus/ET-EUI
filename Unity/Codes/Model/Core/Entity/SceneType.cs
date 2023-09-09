namespace ET
{
	public enum SceneType
	{
		Process = 0,//这个是 Game.Scene的类型
		Manager = 1,
		Realm = 2,
		Gate = 3,
		Http = 4,
		Location = 5,
		Map = 6,
		Account = 7,
		LoginCenter = 8,

		// 客户端Model层
		Client = 30,
		Zone = 31,		//这个是客户端的ZoneScene的类型
		Login = 32,
		Robot = 33,
		Current = 34,
	}
}