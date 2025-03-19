using UnityEngine;
using Vashta.CastlesOfWar.ScriptableObject;

namespace Vashta.CastlesOfWar.MapEntities
{
    [CreateAssetMenu(fileName = "Map Entity Dictionary", menuName = "CoW/Map Entity Dictionary", order = 1)]
    public class MapEntityDictionary : ScriptableDictionary<MapEntityData>
    {
        
    }
}