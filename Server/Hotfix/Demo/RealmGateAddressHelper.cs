using System.Collections.Generic;


namespace ET
{
	public static class RealmGateAddressHelper
	{
		public static StartSceneConfig GetGate(int zone, long AccountId )
		{
			List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Gates[zone];
			
			//int n = RandomHelper.RandomNumber(0, zoneGates.Count);
			int n = AccountId.GetHashCode() % zoneGates.Count;

			return zoneGates[n];
		}
	}
}
