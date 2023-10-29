using System;
using System.Collections.Generic;
using ET.UnitCache;

namespace ET.Handler
{
    [FriendClassAttribute(typeof(ET.UnitCache.UnitCacheComponent))]
    public class Other2UnitCache_GetUnitHandler : AMActorRpcHandler<Scene, Other2UnitCache_GetUnit, UnitCache2Other_GetUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_GetUnit request, UnitCache2Other_GetUnit response, Action reply)
        {
            // throw new NotImplementedException();
            UnitCacheComponent unitCacheComponent = scene.GetComponent<UnitCacheComponent>();
            Dictionary<string, Entity> entityDict = MonoPool.Instance.Fetch(typeof(Dictionary<string, Entity>)) as Dictionary<string, Entity>;
            try
            {
                if (request.ComponentNameList.Count == 0)
                {
                    entityDict.Add(nameof (Unit), null);
                    foreach (string s in unitCacheComponent.UnitCacheKeyList)
                    {
                        entityDict.Add(s, null);
                    }
                }
                else
                {
                    foreach (string s in request.ComponentNameList)
                    {
                        entityDict.Add(s, null);
                    }
                }

                foreach (string key in entityDict.Keys)
                {
                    Entity entity = await unitCacheComponent.Get(request.UnitId, key);
                    entityDict[key] = entity;
                }

                response.ComponentNameList.AddRange(entityDict.Keys);
                response.EntityList.AddRange(entityDict.Values);
            }
            finally
            {
                entityDict.Clear();
                MonoPool.Instance.Recycle(entityDict);
            }
            reply();
            await ETTask.CompletedTask;
        }
    }
}