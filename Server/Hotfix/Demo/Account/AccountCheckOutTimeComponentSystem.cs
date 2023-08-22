using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [Timer(TimerType.AccountSessionCheckOutTime)]
    public class AccountSessionCheckoutTimer : ATimer<AccountCheckOutTimeComponent>
    {
        public override void Run(AccountCheckOutTimeComponent self)
        {
            try
            {
                self.DeleteSession();
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }
    public class AccountCheckOutTimeComponentAwakeSystem:AwakeSystem<AccountCheckOutTimeComponent, long>
    {
        public override void Awake(AccountCheckOutTimeComponent self, long a)
        {
            self.AccountId = a;
            TimerComponent.Instance.Remove(ref self.Timer);
            self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 600000, TimerType.AccountSessionCheckOutTime, self);
        }
    }
    public class AccountCheckoutTimeComponentDestroySystem : DestroySystem<AccountCheckOutTimeComponent>
    {
        public override void Destroy(AccountCheckOutTimeComponent self)
        {
            self.AccountId = 0;
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }
    [FriendClass(typeof(AccountCheckOutTimeComponent))]
    public static class AccountCheckOutTimeComponentSystem
    {
        public static void DeleteSession(this AccountCheckOutTimeComponent self)
        {
            Session session = self.GetParent<Session>();
            long sessionInstanceId = session.DomainScene().GetComponent<AccountSessionsComponent>().Get(self.AccountId);
            if (session.InstanceId == sessionInstanceId)
            {
                session.DomainScene().GetComponent<AccountSessionsComponent>().Remove(self.AccountId);
            }
            session.Send(new A2C_Disconnect() { Error = 1 });
            session?.Disconnect().Coroutine();
        }
    }
}
