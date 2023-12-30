namespace ET
{
    public class UnitCacheDestroySystem: DestroySystem<UnitCache>
    {
        public override void Destroy(UnitCache self)
        {
            foreach (Entity entity in self.CacheComponentDictionary.Values)
            {
                entity.Dispose();
            }
            self.CacheComponentDictionary.Clear();
            self.key = null;
        }
    }
    
    [FriendClassAttribute(typeof(UnitCache))]
    public static class UnitCacheSystem
    {
        public static void AddOrUpdate(this UnitCache self, Entity entity)
        {
            if (entity == null)
            {
                return;
            }

            if (self.CacheComponentDictionary.TryGetValue(entity.Id, out Entity oldEntity))
            {
                if (entity != oldEntity)
                {
                    oldEntity.Dispose();
                }

                self.CacheComponentDictionary.Remove(entity.Id);
            }
            self.CacheComponentDictionary.Add(entity.Id, entity);
        }

        public static async ETTask<Entity> Get(this UnitCache self, long unitId)
        {
            if (!self.CacheComponentDictionary.TryGetValue(unitId, out Entity entity))
            {
                entity = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<Entity>(unitId, self.key);
                if (entity != null)
                {
                    self.AddOrUpdate(entity);
                }
            }

            return entity;
        }

        public static void Delete(this UnitCache self, long id)
        {
            if (self.CacheComponentDictionary.TryGetValue(id, out Entity entity))
            {
                entity.Dispose();
                self.CacheComponentDictionary.Remove(id);
            }
        }
    }
}