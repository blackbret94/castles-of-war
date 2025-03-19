using UnityEngine;
using Vashta.CastlesOfWar.ScriptableObject;

namespace Vashta.CastlesOfWar.Unit
{
    [CreateAssetMenu(fileName = "Unit Data", menuName = "CoW/Unit Data", order = 1)]
    public class UnitData : ScriptableObjectWithID
    {
        [Header("BaseStats")] 
        [Tooltip("Prefab to spawn.  MUST contain UnitBase component. Use UnitBase prefab as a model.")]
        public GameObject Prefab;
        public ushort Health;
        public ushort Armor;
        public float Speed;
        public UnitType UnitType;
        [Tooltip("How far from the target should they stand.  This ensures frontline units are ahead of ranged units.  Positive values stand in front of the target, negative values behind the target.")]
        public float DistanceToStandFromTarget;
        
        [Header("Art")]
        public Sprite SpriteIdle;
        
        [Header("Resources")]
        public ushort GoldCost;
        public ushort PowerCost;
        public ushort GoldRewardOnKill;
        public ushort PowerRewardOnKill;
        
        [Header("Melee")]
        public float MeleeRange;
        public float MeleeAttackSpeed;
        public float MeleeAttackCooldown;
        public ushort MeleeNormalDamage;
        public ushort MeleePiercingDamage;
        public ushort MeleeSiegeDamage;
        
        [Header("Range")]
        public float Range; 
        public float RangedAttackSpeed;
        public float RangedAttackCooldown;
        public ushort RangedNormalDamage;
        public ushort RangedPiercingDamage;
        public ushort RangedSiegeDamage;

        [Header("Area of Effect")] 
        [Tooltip("If null, no melee area of effect damage will occur")]
        public GameObject MeleeAreaOfEffectPrefab;
        [Tooltip("If null, no ranged area of effect damage will occur")]
        public GameObject RangedAreaOfEffectPrefab;
        [Tooltip("Percent of damage that is applied to AOE")]
        public float MeleeDamageAreaPercent;
        [Tooltip("AOE Radius")]
        public float MeleeDamageAreaRadius;
        [Tooltip("Percent of damage that is applied to AOE")]
        public float RangedDamageAreaPercent;
        [Tooltip("AOE Radius")]
        public float RangedDamageAreaRadius;
        
        [Header("Projectile")]
        public float ProjectileSpeedX;
        [Tooltip("Vertical speed.  Ignored if ProjectileHasTrajectory.  0 is horizontal.")]
        public float ProjectileAngle;
        [Tooltip("Accelerates in the -y direction.  M/s.  9.8 for realism.")]
        public float ProjectileGravity = 9.8f;
        [Tooltip("Max lifetime of projectile")]
        public float ProjectileLifetime = 100;
        public bool ProjectileDestroyOnHit = true;
        [Tooltip("Enables trajectory calculations, which ensure the projectile lands at the target.  Use for lobbed artillary-style attacks.")]
        public bool ProjectileHasTrajectory;
        public GameObject ProjectilePrefab;
        
        [Header("Vulnerabilities")]
        public float VulnerabilityMelee = 1;
        public float VulnerabilityRange = 1;
        public float VulnerabilityNormal = 1;
        public float VulnerabilityPiercing = 1;
        public float VulnerabilitySiege = 1;
    }
}