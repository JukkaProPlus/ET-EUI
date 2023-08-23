using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [FriendClass(typeof(RoleInfo))]
    public static class RoleInfoSystem
    {
        public static void FromMessage(this RoleInfo self, RoleInfoProto infoProto)
        {
            self.Id = infoProto.Id;
            self.Name = infoProto.Name;
            self.ServerId = infoProto.ServerId;
            self.AccountId = infoProto.AccountId;
            self.State = infoProto.State;
            self.LastLoginTime = infoProto.LastLoginTime;
            self.CreateTime = infoProto.CreateTime;
        }
        public static RoleInfoProto ToMessage(this RoleInfo self)
        {
            RoleInfoProto infoProto = new RoleInfoProto();
            infoProto.Id = self.Id;
            infoProto.Name = self.Name;
            infoProto.ServerId = self.ServerId;
            infoProto.AccountId = self.AccountId;
            infoProto.State = self.State;
            infoProto.LastLoginTime = self.LastLoginTime;
            infoProto.CreateTime = self.CreateTime;

            return infoProto;
        }
    }
}
