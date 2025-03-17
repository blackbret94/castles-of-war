using UnityEngine;
using Vashta.CastlesOfWar.ScriptableObject;

namespace Vashta.CastlesOfWar.Unit
{
    [CreateAssetMenu(fileName = "Unit Dictionary", menuName = "CoW/Unit Dictionary", order = 1)]
    public class UnitDataDictionary : ScriptableDictionary<UnitData>
    {
        
    }
}