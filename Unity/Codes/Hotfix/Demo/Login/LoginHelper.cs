using System;


namespace ET
{
    [FriendClass(typeof(ET.AccountInfoComponent))]
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string account, string password)
        {
            A2C_LoginAccount a2C_LoginAccount = null;
            Session accountSession = null;
            try
            {
                accountSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                a2C_LoginAccount = (A2C_LoginAccount)await accountSession.Call(new C2A_LoginAccount() { AccountName = account, Password = password });

            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }
            finally
            {
                accountSession?.Dispose();
            }
            if (accountSession.Error != ErrorCode.ERR_Success)
            {
                Log.Error("登录失败");
                accountSession?.Dispose();
                return a2C_LoginAccount.Error;
            }
            else
            {
                zoneScene.AddComponent<SessionComponent>().Session = accountSession;
                Log.Info("登录成功");
                zoneScene.GetComponent<AccountInfoComponent>().Token = a2C_LoginAccount.Token;
                zoneScene.GetComponent<AccountInfoComponent>().AccountId = a2C_LoginAccount.AccountId;
                return ErrorCode.ERR_Success;
            }
            //await ETTask.CompletedTask;
            //try
            //{
            //    // 创建一个ETModel层的Session
            //    R2C_Login r2CLogin;
            //    Session session = null;
            //    try
            //    {
            //        session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
            //        {
            //            r2CLogin = (R2C_Login) await session.Call(new C2R_Login() { Account = account, Password = password });
            //        }
            //    }
            //    finally
            //    {
            //        session?.Dispose();
            //    }

            //    // 创建一个gate Session,并且保存到SessionComponent中
            //    Session gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(r2CLogin.Address));
            //    gateSession.AddComponent<PingComponent>();
            //    zoneScene.AddComponent<SessionComponent>().Session = gateSession;

            //    G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await gateSession.Call(
            //        new C2G_LoginGate() { Key = r2CLogin.Key, GateId = r2CLogin.GateId});

            //    Log.Debug("登陆gate成功!");

            //    Game.EventSystem.PublishAsync(new EventType.LoginFinish() {ZoneScene = zoneScene}).Coroutine();
            //}
            //catch (Exception e)
            //{
            //    Log.Error(e);
            //}
        }
    }
}