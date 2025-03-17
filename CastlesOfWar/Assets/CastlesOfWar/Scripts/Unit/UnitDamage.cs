using System;
using UnityEngine;
using Vashta.CastlesOfWar.Combat;
using Mathf = UnityEngine.Mathf;

namespace Vashta.CastlesOfWar.Unit
{
    public class UnitDamage : MonoBehaviour
    {
        public UnitBase UnitBase { get; set; }
        private UnitData _unitData => UnitBase.UnitData;

        public ushort TakeAttack(ushort normalDamage, ushort piercingDamage, ushort siegeDamage, UnitCombatType combatType)
        {
            float normalDamageTaken = TakeDamage(normalDamage, DamageType.Normal, combatType);
            float piercingDamageTaken = TakeDamage(piercingDamage, DamageType.Pierce, combatType);
            float siegeDamageTaken = TakeDamage(siegeDamage, DamageType.Siege, combatType);
            ushort totalDamage = (ushort)Mathf.FloorToInt(normalDamageTaken + piercingDamageTaken + siegeDamageTaken);
            
            ReduceHealth(totalDamage);
            
            return totalDamage;
        }

        public void ReduceHealth(ushort healthReduction)
        {
            UnitBase.CurrentHealth -= healthReduction;

            if (UnitBase.CurrentHealth <= 0)
            {
                // Handle death
                Destroy(UnitBase);
            }
        }
        
        private float TakeDamage(ushort baseDamage, DamageType damageType, UnitCombatType combatType)
        {
            if (baseDamage == 0)
                return 0f;
            
            ushort armor = UnitBase.Armor;
            
            // Get melee/ranged modifiers
            float combatModifier = 1f;
            if (combatType == UnitCombatType.Melee)
                combatModifier = _unitData.VulnerabilityMelee;
            else if (combatType == UnitCombatType.Ranged)
                combatModifier = _unitData.VulnerabilityRange;
            
            switch (damageType)
            {
                case DamageType.Normal:
                    return combatModifier * Mathf.Max(0, baseDamage - armor) * _unitData.VulnerabilityNormal;
                case DamageType.Pierce:
                    return combatModifier * baseDamage * _unitData.VulnerabilityPiercing;
                case DamageType.Siege:
                    return combatModifier * baseDamage * _unitData.VulnerabilitySiege;
                default:
                    throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null);
            }
            
            return 0;
        }
    }
}