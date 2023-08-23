using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class ServerInfoComponentDestroySystem : DestroySystem<ServerInfosComponent>
    {
        public override void Destroy(ServerInfosComponent self)
        {
            foreach(var serverinfo in self.serverInfoList)
            {
                serverinfo?.Dispose();
            }
            self.serverInfoList?.Clear();
            self.serverInfoList = null;
        }
    }
    //public class ServerInfoComponentAwakeSystem : AwakeSystem<ServerInfosComponent>
    //{
    //    public override void Awake(ServerInfosComponent self)
    //    {

    //    }
    //}
    [FriendClass(typeof(ServerInfosComponent))]
    public static class ServerInfoComponentSystem
    {
        public static void SaveServerInfoList(this ServerInfosComponent self, List<ServerInfo> serverInfos)
        {
            
        }
        public static void Add(this ServerInfosComponent self, ServerInfo info)
        {
            self.serverInfoList.Add(info);
        }
    }
}
