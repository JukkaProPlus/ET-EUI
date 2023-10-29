

using System.Net;
using System.ServiceModel.Security;
using ET.UnitCache;

namespace ET
{
    public static class SceneFactory
    {
        public static async ETTask<Scene> Create(Entity parent, string name, SceneType sceneType)
        {
            long instanceId = IdGenerater.Instance.GenerateInstanceId();
            return await Create(parent, instanceId, instanceId, parent.DomainZone(), name, sceneType);
        }
        
        public static async ETTask<Scene> Create(Entity parent, long id, long instanceId, int zone, string name, SceneType sceneType, StartSceneConfig startSceneConfig = null)
        {
            await ETTask.CompletedTask;
            Scene scene = EntitySceneFactory.CreateScene(id, instanceId, zone, sceneType, name, parent);

            scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);

            switch (scene.SceneType)
            {
                case SceneType.Realm://负载均衡服务器
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.OuterIPPort, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    scene.AddComponent<TokenComponent>();
                    break;
                case SceneType.Gate://网关服
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.OuterIPPort, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    scene.AddComponent<PlayerComponent>();
                    scene.AddComponent<GateSessionKeyComponent>();
                    break;
                case SceneType.Map://游戏逻辑服
                    scene.AddComponent<UnitComponent>();
                    scene.AddComponent<AOIManagerComponent>();
                    break;
                case SceneType.Location://定位服
                    scene.AddComponent<LocationComponent>();
                    break;
                case SceneType.Account://账号服
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.OuterIPPort, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    Log.Info("创建账号服");
                    //scene.AddComponent<DBManagerComponent>();
                    scene.AddComponent<TokenComponent>();
                    scene.AddComponent<AccountSessionsComponent>();
                    scene.AddComponent<ServerInfoManagerComponent>();
                    break;
                case SceneType.LoginCenter://登录中心服
                    scene.AddComponent<LoginInfoRecordComponent>();
                    break;
                case SceneType.UnitCache://单元缓存服
                    Log.Info("创建单元缓存服20231029");
                    scene.AddComponent<UnitCacheComponent>();
                    break;
            }

            return scene;
        }
    }
}