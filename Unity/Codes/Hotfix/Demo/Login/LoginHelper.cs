using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{
    [FriendClass(typeof(ET.AccountInfoComponent))]
    [FriendClass(typeof(ET.RoleInfosComponent))]
    [FriendClass(typeof(ET.ServerInfosComponent))]
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string account, string password)
        {
            A2C_LoginAccount a2C_LoginAccount = null;
            Session accountSession = null;
            try
            {
                accountSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                password = MD5Helper.StringMD5(password);
                a2C_LoginAccount = (A2C_LoginAccount)await accountSession.Call(new C2A_LoginAccount() { AccountName = account, Password = password });

            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                accountSession?.Dispose();
                return ErrorCode.ERR_NetWorkError;
            }
            if (a2C_LoginAccount.Error != ErrorCode.ERR_Success)
            {
                Log.Error("登录失败");
                accountSession?.Dispose();
                return a2C_LoginAccount.Error;
            }
            else
            {
                zoneScene.AddComponent<SessionComponent>().Session = accountSession;
                zoneScene.GetComponent<SessionComponent>().Session.AddComponent<PingComponent>();
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
        public async static ETTask<int> GetServerInfos(Scene zoneScene)
        {
            A2C_GetServerInfos a2C_GetServerInfos;
            try
            {
                a2C_GetServerInfos = (A2C_GetServerInfos)await zoneScene.GetComponent<SessionComponent>().Session.Call(
                     new C2A_GetServerInfos() { 
                         AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                         Token = zoneScene.GetComponent<AccountInfoComponent>().Token
                     });
                if (a2C_GetServerInfos.Error != ErrorCode.ERR_Success)
                {
                    return a2C_GetServerInfos.Error;
                }
                foreach (ServerInfoProto info in a2C_GetServerInfos.ServerInfosList)
                {
                    ServerInfo serverInfo = zoneScene.GetComponent<ServerInfosComponent>().AddChild<ServerInfo>();
                    serverInfo.FromMessage(info);
                    zoneScene.GetComponent<ServerInfosComponent>().Add(serverInfo);
                }
                return ErrorCode.ERR_Success;
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }
        }
        public async static ETTask<int> CreateRole(Scene zoneScene, string Name)
        {
            A2C_CreateRole a2C_CreateRole = null;
            try
            {
                C2A_CreateRole c2A_CreateRole = new C2A_CreateRole() {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    Name = Name,
                    ServerId = (int)zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId
                };
                a2C_CreateRole = (A2C_CreateRole) await zoneScene.GetComponent<SessionComponent>().Session.Call(c2A_CreateRole);
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }
            if (a2C_CreateRole.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2C_CreateRole.Error.ToString());
                return a2C_CreateRole.Error;
            }
            else
            {
                //RoleInfo roleInfo = new RoleInfo();
                RoleInfo newRoleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChild<RoleInfo>();
                newRoleInfo.FromMessage(a2C_CreateRole.RoleInfo);
                zoneScene.GetComponent<RoleInfosComponent>().RoleInfos.Add(newRoleInfo);
                //zoneScene.GetComponent<RoleInfosComponent>().CurrentRoleId = newRoleInfo.Id;
                return ErrorCode.ERR_Success;
            }
        }
        public async static ETTask<int> GetRoles(Scene zoneScene)
        {
            A2C_GetRoles a2C_GetRoles;
            try
            {
                C2A_GetRoles c2A_GetRoles = new C2A_GetRoles()
                {
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    ServerId = (int)zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId
                };
                a2C_GetRoles = (A2C_GetRoles)await zoneScene.GetComponent<SessionComponent>().Session.Call(c2A_GetRoles);
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }
            if (a2C_GetRoles.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2C_GetRoles.Error.ToString());
                return a2C_GetRoles.Error;
            }
            else
            {
                foreach(RoleInfoProto info in a2C_GetRoles.RoleInfos)
                {
                    RoleInfo roleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChild<RoleInfo>();
                    roleInfo.FromMessage(info);
                    zoneScene.GetComponent<RoleInfosComponent>().RoleInfos.Add(roleInfo);
                }
                return ErrorCode.ERR_Success;
            }
        }
        public async static ETTask<int> DeleteRole(Scene zoneScene, long deleteRoleId)
        {
            A2C_DeleteRole a2C_DeleteRole;
            try
            {
                a2C_DeleteRole = (A2C_DeleteRole)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_DeleteRole()
                {
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId,
                    RoleInfoId = zoneScene.GetComponent<RoleInfosComponent>().CurrentRoleId,
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }
            if (a2C_DeleteRole.Error == ErrorCode.ERR_Success)
            {
                //zoneScene.GetComponent<RoleInfosComponent>().GetChild<RoleInfo>(a2C_DeleteRole.DeletedRoleInfoId).Dispose();
                //zoneScene.GetComponent<RoleInfosComponent>().CurrentRoleId = 0;

                int index = zoneScene.GetComponent<RoleInfosComponent>().RoleInfos.FindIndex((info) => { return info.Id == a2C_DeleteRole.DeletedRoleInfoId; });
                zoneScene.GetComponent<RoleInfosComponent>().RoleInfos.RemoveAt(index);
                return ErrorCode.ERR_Success;
            }
            Log.Error(a2C_DeleteRole.Error.ToString());
            return a2C_DeleteRole.Error;
        }

        public async static ETTask<int> GetRealmKey(Scene zoneScene)
        {
            A2C_GetRealmKey a2CGetRealmKey = null; 
            try
            {
                a2CGetRealmKey = (A2C_GetRealmKey)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetRealmKey()
                {
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2CGetRealmKey.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2CGetRealmKey.Error.ToString());
                return a2CGetRealmKey.Error;
            }
            else
            {
                zoneScene.GetComponent<AccountInfoComponent>().RealmAddress = a2CGetRealmKey.RealmAddress;
                zoneScene.GetComponent<AccountInfoComponent>().RealmKey = a2CGetRealmKey.RealmKey;
                zoneScene.GetComponent<SessionComponent>().Session.Dispose();
                return ErrorCode.ERR_Success;
            }
        }

        public async static ETTask<int> EnterGame(Scene zoneScene)
        {
            string realmAddress = zoneScene.GetComponent<AccountInfoComponent>().RealmAddress;
            R2C_LoginRealm r2C_LoginRealm = null;
            Session session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(realmAddress));
            try
            {
                r2C_LoginRealm = (R2C_LoginRealm)await session.Call(new C2R_LoginRealm()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    RealmTokenKey = zoneScene.GetComponent<AccountInfoComponent>().RealmKey,
                });
                
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                session?.Dispose();
                return ErrorCode.ERR_NetWorkError;
            }
            session?.Dispose();

            if (r2C_LoginRealm.Error != ErrorCode.ERR_Success)
            {
                Log.Error(r2C_LoginRealm.Error.ToString());
                return r2C_LoginRealm.Error;
            }

            Log.Warning($"GateAddress:{r2C_LoginRealm.GateAddress}");
            Session gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(r2C_LoginRealm.GateAddress));
            gateSession.AddComponent<PingComponent>();
            zoneScene.GetComponent<SessionComponent>().Session = gateSession;
            
            //2.开始连接Gate
            long currentRoleId = zoneScene.GetComponent<RoleInfosComponent>().CurrentRoleId;
            G2C_LoginGameGate g2C_LoginGameGate = null;
            try
            {
                
                g2C_LoginGameGate = (G2C_LoginGameGate)await gateSession.Call(new C2G_LoginGameGate()
                {
                    Key = r2C_LoginRealm.GateSessionKey,
                    RoleId = currentRoleId,
                    Account = zoneScene.GetComponent<AccountInfoComponent>().AccountId
                });

                
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                gateSession?.Dispose();
                return ErrorCode.ERR_NetWorkError;
            }
            if (g2C_LoginGameGate.Error != ErrorCode.ERR_Success)
            {
                Log.Error(g2C_LoginGameGate.Error.ToString());
                return g2C_LoginGameGate.Error;
            }
            Log.Debug("登陆Gate成功");
            //3.角色正式请求进入逻辑服
            G2C_EnterGame g2C_EnterGame = null;
            try
            {
                g2C_EnterGame = (G2C_EnterGame)await gateSession.Call(new C2G_EnterGame());
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
                zoneScene.GetComponent<SessionComponent>().Session?.Dispose();
                return ErrorCode.ERR_NetWorkError;
            }
            if (g2C_EnterGame.Error != ErrorCode.ERR_Success)
            {
                Log.Error(g2C_EnterGame.Error.ToString());
                zoneScene.GetComponent<SessionComponent>().Session?.Dispose();
                return g2C_EnterGame.Error;
            }

            Log.Debug("角色进入游戏服成功");
            return ErrorCode.ERR_Success;
        }
    }
}