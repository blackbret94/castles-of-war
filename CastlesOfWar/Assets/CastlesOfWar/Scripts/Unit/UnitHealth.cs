using System;
using UnityEngine;
using Vashta.CastlesOfWar.Combat;
using Vashta.CastlesOfWar.UI;
using Mathf = UnityEngine.Mathf;

namespace Vashta.CastlesOfWar.Unit
{
    public class UnitHealth : MonoBehaviour
    {
        public UnitBase UnitBase { get; set; }
        private UnitData _unitData => UnitBase.UnitData;
        private Healthbar _healthbar => UnitBase.Healthbar;

        public short TakeAttack(ushort normalDamage, ushort piercingDamage, ushort siegeDamage, UnitCombatType combatType)
        {
            float normalDamageTaken = TakeDamage(normalDamage, DamageType.Normal, combatType);
            float piercingDamageTaken = TakeDamage(piercingDamage, DamageType.Pierce, combatType);
            float siegeDamageTaken = TakeDamage(siegeDamage, DamageType.Siege, combatType);
            short totalDamage = (short)Mathf.FloorToInt(normalDamageTaken + piercingDamageTaken + siegeDamageTaken);
            
            ReduceHealth(totalDamage);
            
            return totalDamage;
        }

        public void ReduceHealth(short healthReduction)
        {
            UnitBase.CurrentHealth -= healthReduction;

            if (UnitBase.CurrentHealth <= 0)
            {
                // Handle death
                UnitBase.Team.DespawnUnit(UnitBase);
                Team enemyTeam = GameManager.GetInstance().GetEnemyTeam(UnitBase.TeamIndex);
                enemyTeam.CurrencyController.ModifyGold(_unitData.GoldRewardOnKill);
                enemyTeam.CurrencyController.ModifyPower(_unitData.PowerRewardOnKill);
            }
            else
            {
                _healthbar.SetHealth(UnitBase.CurrentHealth);
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