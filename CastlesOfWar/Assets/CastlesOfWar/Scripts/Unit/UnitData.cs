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
        [Tooltip("Percent of damage that is applied to AOE")]
        public float MeleeDamageAreaPercent;
        
        [Header("Range")]
        public float Range; 
        public float RangedAttackSpeed;
        public float RangedAttackCooldown;
        public ushort RangedNormalDamage;
        public ushort RangedPiercingDamage;
        public ushort RangedSiegeDamage;
        [Tooltip("Percent of damage that is applied to AOE")]
        public float RangedDamageAreaPercent;
        
        [Header("Projectile")]
        public float ProjectileSpeedX;
        [Tooltip("Fake a trajectory using distance as a reference. 0 is horizontal.")]
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