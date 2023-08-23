using Microsoft.CodeAnalysis.FlowAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class ServerInfoManagerComponentAwakeSystem : AwakeSystem<ServerInfoManagerComponent>
    {
        public override void Awake(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }
    public class ServerInfoManagerComponentDestroySystem : DestroySystem<ServerInfoManagerComponent>
    {
        public override void Destroy(ServerInfoManagerComponent self)
        {
            foreach(var info in self.serverList)
            {
                info?.Dispose();
            }
            self.serverList.Clear();
        }
    }
    public class ServerInfoManagerComponentLoadSystem : LoadSystem<ServerInfoManagerComponent>
    {
        public override void Load(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }
    [FriendClassAttribute(typeof(ET.ServerInfoManagerComponent))]
    [FriendClassAttribute(typeof(ET.ServerInfo))]
    public static class ServerInfoManagerComponentSystem
    {
        public async static ETTask Awake(this ServerInfoManagerComponent self)
        {
            var serverList = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<ServerInfo>(d => true);
            if (serverList == null || serverList.Count <= 0)
            {
                self.serverList.Clear();

                var serverInfoConfigDict = ServerInfoConfigCategory.Instance.GetAll();
                foreach (var cfg in serverInfoConfigDict.Values)
                {
                    ServerInfo serverInfo = self.AddChildWithId<ServerInfo>(cfg.Id);
                    serverInfo.ServerName = cfg.ServerName;
                    serverInfo.Status = (int)ServerStatus.Normal;
                    self.serverList.Add(serverInfo);
                    await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save<ServerInfo>(serverInfo);
                }

                return;
            }
            self.serverList.Clear();
            foreach (ServerInfo info in serverList)
            {
                self.AddChild(info);
                self.serverList.Add(info);
            }
            await ETTask.CompletedTask;
            //self.serverList = new List<ServerInfo>();
        }
    }
}
