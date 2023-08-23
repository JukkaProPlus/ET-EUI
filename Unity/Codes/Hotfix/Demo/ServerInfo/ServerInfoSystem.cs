using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    //[FriendClass(typeof(ServerInfoProto))]
    [FriendClass(typeof(ServerInfo))]
    public static class ServerInfoSystem
    {
        public static ServerInfo FromMessage(this ServerInfo self, ServerInfoProto serverInfoProto)
        {
            self.Status = serverInfoProto.Status;
            self.ServerName = serverInfoProto.ServerName;
            self.Id = serverInfoProto.Id;
            return self;
        }
        public static ServerInfoProto ToMessage(this ServerInfo self)
        {
            ServerInfoProto serverInfoProto = new ServerInfoProto();
            serverInfoProto.Status = self.Status;
            serverInfoProto.ServerName = self.ServerName;
            serverInfoProto.Id = (int)self.Id;
            return serverInfoProto;
        }
    }
}
