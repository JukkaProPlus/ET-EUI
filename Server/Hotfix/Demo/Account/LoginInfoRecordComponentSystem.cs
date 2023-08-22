using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class LoginInfoRecordComponentAwakeSystem : AwakeSystem<LoginInfoRecordComponent>
    {
        public override void Awake(LoginInfoRecordComponent self)
        {
            self.AccountLoginInfoDict = new Dictionary<long, int>();
        }
    }
    public class LoginInfoRecordComponentDestroySystem : DestroySystem<LoginInfoRecordComponent>
    {
        public override void Destroy(LoginInfoRecordComponent self)
        {
            self.AccountLoginInfoDict?.Clear();
            self.AccountLoginInfoDict = null;
        }
    }
    [FriendClass(typeof(LoginInfoRecordComponent))]
    public static class LoginInfoRecordComponentSystem
    {
        public static void Add(this LoginInfoRecordComponent self, long key, int value)
        {
            if (self.AccountLoginInfoDict.ContainsKey(key))
            {
                self.AccountLoginInfoDict[key] = value;
            }
            else
            {
                self.AccountLoginInfoDict.Add(key, value);
            }
        }
        public static void Remove(this LoginInfoRecordComponent self, long key)
        {
            if (self.AccountLoginInfoDict.ContainsKey(key))
            {
                self.AccountLoginInfoDict.Remove(key);
            }
        }
        public static int Get(this LoginInfoRecordComponent self, long key)
        {
            if (self.AccountLoginInfoDict.ContainsKey(key))
            {
                return self.AccountLoginInfoDict[key];
            }
            else
            {
                return -1;
            }
        }
        public static bool IsExist(this LoginInfoRecordComponent self, long key)
        {
            return self.AccountLoginInfoDict.ContainsKey(key);
        }
    }
    
}
