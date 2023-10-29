using System.Collections.Generic;

namespace ET.UnitCache
{
    public interface IUnitCache
    {
        
    }
    
    public class UnitCache:Entity, IAwake, IDestroy
    {
        public string key;
        public Dictionary<long, Entity> CacheComponentDictionary = new Dictionary<long, Entity>();
    }
}