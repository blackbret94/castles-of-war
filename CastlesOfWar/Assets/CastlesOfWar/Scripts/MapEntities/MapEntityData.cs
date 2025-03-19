using UnityEngine;
using UnityEngine.Serialization;
using Vashta.CastlesOfWar.ScriptableObject;

namespace Vashta.CastlesOfWar.MapEntities
{
    [CreateAssetMenu(fileName = "Map Entity Data", menuName = "CoW/Map Entity Data", order = 1)]
    public class MapEntityData : ScriptableObjectWithID
    {
        [Header("BaseStats")]
        public GameObject Prefab;

        [Header("Capture")] 
        [Tooltip("Can this entity be captured?  If not, it belongs to whoever owns both adjacent outposts")]
        public bool CanCapture;
        [Tooltip("Time to capture entity")]
        public float CaptureTime;
        [Tooltip("Can this be constructed?")]
        public bool CanBuild;
        [Tooltip("Determines how long it takes an engineer to construct")]
        public ushort ConstructionCost;

        [Header("Functionality")] 
        [Tooltip("Does this function as a team base?  If this changes teams the game is over!")]
        public bool IsBase;
        [Tooltip("Does this function as an outpost? This ties it into the advance/hold/retreat commands")]
        public bool IsOutpost;
        public float GoldPerSecond;
        public float PowerPerSecond;
        public bool TrainsBasicMelee;
        public bool TrainsRanged;
        public bool TrainsCavalry;
        public bool TrainsLargeUnits;
        public bool TrainsSiege;
        
        [Header("Health")] 
        public bool CanDestroy;
        public ushort Health;

        [Header("UnitEffects")] 
        public float SpeedModifierPerc = 1;

        [FormerlySerializedAs("BuffDefense")] [Header("UnitBuffs")] 
        public short BuffArmor;
        public float BuffMeleeAttackSpeed;
        public short BuffMeleeNormalDamage;
        public short BuffMeleePiercingDamage;
        public short BuffMeleeSiegeDamage;
        public short BuffRangedNormalDamage;
        public short BuffRangedPiercingDamage;
        public short BuffRangedSiegeDamage;
        public float BuffRangedAttackSpeed;
        public float BuffVulnerabilityMelee = 1;
        public float BuffVulnerabilityRanged = 1;

        [Header("UnitDebuffs")]
        public short DebuffDefense;
        
        [Header("Vulnerabilities")]
        public float VulnerabilityMelee = 1;
        public float VulnerabilityRange = 1;
        public float VulnerabilityNormal = 1;
        public float VulnerabilityPiercing = 1;
        public float VulnerabilitySiege = 1;
    }
}