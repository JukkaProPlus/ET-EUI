using System;
using ET;

namespace ET.Handler
{
    public class Other2UnitCache_AddOrUpdateUnitHandler:AMActorRpcHandler<Scene, Other2UnitCache_AddOrUpdateUnit, UnitCache2Other_AddOrUpdateUnit>
    {
        protected override async ETTask Run(Scene unit, Other2UnitCache_AddOrUpdateUnit request, UnitCache2Other_AddOrUpdateUnit response, Action reply)
        {
            UpdateUnitCacheAsync(unit, request, response).Coroutine();
            reply();
            await ETTask.CompletedTask;
        }

        private async ETTask UpdateUnitCacheAsync(Scene scene, Other2UnitCache_AddOrUpdateUnit request, UnitCache2Other_AddOrUpdateUnit response)
        {
            UnitCacheComponent unitCacheComponent = scene.GetComponent<UnitCacheComponent>();
            using (ListComponent<Entity> entityList = ListComponent<Entity>.Create())
            {
                for (int index = 0; index < request.EntityTypes.Count; index++)
                {
                    Type type = Game.EventSystem.GetType(request.EntityTypes[index]);
                    Entity entity = (Entity)MongoHelper.FromBson(type, request.EntityBytes[index]);
                    entityList.Add(entity);
                }

                await unitCacheComponent.AddOrUpdate(request.UnitId, entityList);
            }
        }
    }
}