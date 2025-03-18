using UnityEngine;
using UnityEngine.Serialization;
using Vashta.CastlesOfWar.ScriptableObject;

namespace Vashta.CastlesOfWar.Unit
{
    [CreateAssetMenu(fileName = "Unit Data", menuName = "CoW/Unit Data", order = 1)]
    public class UnitData : ScriptableObjectWithID
    {
        [Header("BaseStats")] 
        public Sprite Sprite;
        public ushort Cost;
        public ushort Health;
        public ushort Armor;
        public float Speed;
        public UnitType UnitType;
        
        [Header("Melee")]
        public float MeleeRange;
        public float MeleeAttackSpeed;
        public float MeleeAttackCooldown;
        public ushort MeleeNormalDamage;
        public ushort MeleePiercingDamage;
        public ushort MeleeSiegeDamage;
        public float MeleeDamageAreaPercent; // Percent of damage that is AOE
        
        [Header("Range")]
        public float Range; 
        public float RangedAttackSpeed;
        public float RangedAttackCooldown;
        public ushort RangedNormalDamage;
        public ushort RangedPiercingDamage;
        public ushort RangedSiegeDamage;
        public float RangedDamageAreaPercent;
        
        [Header("Projectile")]
        public float ProjectileSpeedX;
        public float ProjectileAngle; // Fake a trajectory using distance as a reference. 0 is horizontal.
        public float ProjectileGravity = -9.8f;
        public float ProjectileLifetime = 100; // Max lifetime of projectile
        public bool ProjectileDestroyOnHit = true;
        public GameObject ProjectilePrefab;
        
        [Header("Vulnerabilities")]
        public float VulnerabilityMelee = 1;
        public float VulnerabilityRange = 1;
        public float VulnerabilityNormal = 1;
        public float VulnerabilityPiercing = 1;
        public float VulnerabilitySiege = 1;
    }
}