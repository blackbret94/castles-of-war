using UnityEngine;

namespace Vashta.CastlesOfWar.Unit
{
    public class UnitCombat : MonoBehaviour
    {
        public UnitBase UnitBase { get; set; }
        private UnitData _unitData => UnitBase.UnitData;
        private Transform _attackOrigin => UnitBase.AttackOrigin;

        // This could eventually be moved into an AttackData object
        private float _lastMeleeAttackTime;
        private float _lastRangedAttackTime;
        private UnitCombatType _combatType;
        
        public UnitCombatPhase CombatPhase { get; private set; }
        public UnitMeleeCollider MeleeCollider { get; set; }

        private void Awake()
        {
            CombatPhase = UnitCombatPhase.Ready;
        }

        public bool IsMovementBlocked()
        {
            // Block if attacking or recovering from an attack
            if (CombatPhase != UnitCombatPhase.Ready)
            {
                Debug.Log("Blocked by combat phase: " + CombatPhase);
                return true;
            }

            // Block if an enemy is in melee range
            if (MeleeCollider.Units.Count > 0)
            {
                Debug.Log("Blocked by units in range: " + MeleeCollider.Units.Count);
                return true;
            }

            return false;
        }
        
        // Later this can be turned into a state machine or behaviour tree
        public void OneStep(float timestep)
        {
            if (CombatPhase == UnitCombatPhase.Ready)
            {
                // if target in range, attack
                if (!TryAttackMelee())
                {
                    TryAttackRanged();
                }
            } else if (CombatPhase == UnitCombatPhase.Attacking)
            {
                if (Time.time >= _lastMeleeAttackTime + _unitData.MeleeAttackSpeed)
                {
                    if (_combatType == UnitCombatType.Melee)
                    {
                        AttackMelee();
                    } else if (_combatType == UnitCombatType.Ranged)
                    {
                        AttackRanged();
                    }
                    
                    CombatPhase = UnitCombatPhase.Recovering;
                }
            } else if (CombatPhase == UnitCombatPhase.Recovering)
            {
                if (Time.time >= _lastMeleeAttackTime + _unitData.MeleeAttackSpeed + _unitData.MeleeAttackCooldown)
                    CombatPhase = UnitCombatPhase.Ready;
            }
        }
        private bool TryAttackMelee()
        {
            UnitBase nearestUnit = MeleeCollider.GetNearestUnit(out float distanceToTarget);

            if (nearestUnit != null)
            {
                // Attack
                CombatPhase = UnitCombatPhase.Attacking;
                _lastMeleeAttackTime = Time.time;
                _combatType = UnitCombatType.Melee;

                return true;
            }

            return false;
        }

        private bool AttackMelee()
        {
            // Getting the nearest unit again so the correct unit is hit even if the previous target died during
            // the attack time
            UnitBase nearestUnit = MeleeCollider.GetNearestUnit(out float distanceToTarget);

            if (nearestUnit == null)
                return false;

            ushort damageDone = nearestUnit.Damage.TakeAttack(_unitData.MeleeNormalDamage, _unitData.MeleePiercingDamage,
                _unitData.MeleeSiegeDamage, UnitCombatType.Melee);

            return damageDone > 0;
        }

        private bool TryAttackRanged()
        {
            return false;
        }

        private bool AttackRanged()
        {
            return false;
        }
    }
}