using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [FriendClass(typeof(TokenComponent))]
    public static class TokenComponentSystem
    {
        public static void Add(this TokenComponent self, long key, string token)
        {
            self.TokenDictionary.Add(key, token);
            self.TimeOutRemoveKey(key, token).Coroutine();
        }
        public static string Get(this TokenComponent self, long key)
        {
            string token = null;
            if (self.TokenDictionary.ContainsKey(key))
            {
                token = self.TokenDictionary[key];
            }
            return token;
        }
        public static void Remove(this TokenComponent self, long key)
        {
            if(self.TokenDictionary.ContainsKey(key))
            {
                self.TokenDictionary.Remove(key);
            }
        }
        private static async ETTask TimeOutRemoveKey(this TokenComponent self, long key, string token)
        {
            await TimerComponent.Instance.WaitAsync(3600);
            string onlineToken = self.Get(key);
            if (!string.IsNullOrEmpty(onlineToken) && onlineToken == token)
            {
                self.Remove(key);
            }
            
        }
    }
}
