using UnityEngine;
using Vashta.CastlesOfWar.Projectiles;
using Vashta.CastlesOfWar.Simulation;

namespace Vashta.CastlesOfWar.Unit
{
    public class UnitCombat : MonoBehaviour, ISimulatedObject
    {
        public UnitBase UnitBase { get; set; }
        private UnitData _unitData => UnitBase.UnitData;
        private Transform _attackOrigin => UnitBase.AttackOrigin;
        
        private float _lastAttackTime = 0f;
        private UnitCombatType _combatType;
        private GameManager _gameManager;
        
        public UnitCombatPhase CombatPhase { get; private set; }
        public UnitCombatCollider MeleeCollider { get; set; }
        public UnitCombatCollider RangedCollider { get; set; }

        private void Awake()
        {
            CombatPhase = UnitCombatPhase.Ready;
        }

        private void Start()
        {
            _gameManager = GameManager.GetInstance();
        }

        public bool IsMovementBlocked()
        {
            // Block if attacking or recovering from an attack
            if (CombatPhase != UnitCombatPhase.Ready)
            {
                // Debug.Log("Blocked by combat phase: " + CombatPhase);
                return true;
            }

            // Block if an enemy is in melee range
            if (MeleeCollider.Units.Count > 0)
            {
                // Debug.Log("Blocked by units in range: " + MeleeCollider.Units.Count);
                return true;
            }

            return false;
        }
        
        // Later this can be turned into a state machine or behaviour tree
        public void OneStep(float timestep)
        {
            switch (CombatPhase)
            {
                // Check to start combat
                case UnitCombatPhase.Ready:
                {
                    // if target in range, attack
                    if (!TryAttackMelee())
                    {
                        TryAttackRanged();
                    } 
                
                    // Attack
                    break;
                }
                // Execute melee attack
                case UnitCombatPhase.Attacking when _combatType == UnitCombatType.Melee:
                {
                    if (_gameManager.SimulationTime >= _lastAttackTime + _unitData.MeleeAttackSpeed)
                    {
                        AttackMelee();
                        CombatPhase = UnitCombatPhase.Recovering;

                    }
                    
                    break;
                }
                // Execute ranged attack
                case UnitCombatPhase.Attacking when _combatType == UnitCombatType.Ranged:
                {
                    if (_gameManager.SimulationTime >= _lastAttackTime + _unitData.RangedAttackSpeed)
                    {
                        AttackRanged();
                        
                        CombatPhase = UnitCombatPhase.Recovering;
                    }
                    break;
                }
                // Recover melee
                case UnitCombatPhase.Recovering when _combatType == UnitCombatType.Melee:
                {
                    if (_gameManager.SimulationTime >= _lastAttackTime + _unitData.MeleeAttackSpeed + _unitData.MeleeAttackCooldown)
                        CombatPhase = UnitCombatPhase.Ready;
                    
                    break;
                }
                // Recover ranged
                case UnitCombatPhase.Recovering when _combatType == UnitCombatType.Ranged:
                {
                    if (_gameManager.SimulationTime >= _lastAttackTime + _unitData.RangedAttackSpeed + _unitData.RangedAttackCooldown)
                        CombatPhase = UnitCombatPhase.Ready;

                    break;
                }
            }
        }
        private bool TryAttackMelee()
        {
            UnitBase nearestUnit = MeleeCollider.GetNearestUnit(out float distanceToTarget);

            if (nearestUnit != null)
            {
                // Attack
                CombatPhase = UnitCombatPhase.Attacking;
                _lastAttackTime = _gameManager.SimulationTime;
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

            short damageDone = nearestUnit.Health.TakeAttack(_unitData.MeleeNormalDamage, _unitData.MeleePiercingDamage,
                _unitData.MeleeSiegeDamage, UnitCombatType.Melee);

            return damageDone > 0;
        }

        private bool TryAttackRanged()
        {
            UnitBase nearestUnit = RangedCollider.GetNearestUnit(out float distanceToTarget);

            if (nearestUnit != null)
            {
                // Attack
                CombatPhase = UnitCombatPhase.Attacking;
                _lastAttackTime = _gameManager.SimulationTime;
                _combatType = UnitCombatType.Ranged;

                return true;
            }

            return false;
        }

        private bool AttackRanged()
        {
            // Getting the nearest unit again so the correct unit is hit even if the previous target died during
            // the attack time
            UnitBase nearestUnit = RangedCollider.GetNearestUnit(out float distanceToTarget);

            if (nearestUnit == null)
            {
                return false;
            }

            if (_unitData.ProjectilePrefab == null)
            {
                Debug.LogError("Unit type " + _unitData.Title + " is missing a projectile prefab");
                return false;
            }
            
            // Created projectile
            GameObject newProjectile = Instantiate(_unitData.ProjectilePrefab, _attackOrigin.position, Quaternion.identity);
            ProjectileBase projectileBase = newProjectile.GetComponent<ProjectileBase>();

            if (!projectileBase)
            {
                Debug.LogError("Projectile for " + _unitData.Title + " is missing a ProjectileBase component");
                Destroy(newProjectile);
                return false;
            }
            
            float speedDirectionSign = (nearestUnit.transform.position.x - UnitBase.transform.position.x) > 0 ? 1 : -1;
            
            float angle = _unitData.ProjectileAngle;
            float speedX = _unitData.ProjectileSpeedX;
            float speedY = Mathf.Tan(Mathf.Deg2Rad * angle) * speedX; // Right now this only works for very small angles, not trajectories
            float gravity = _unitData.ProjectileGravity;
            float lifetime = _unitData.ProjectileLifetime;
            bool destroyOnHit = _unitData.ProjectileDestroyOnHit;
            ushort teamIndex = UnitBase.TeamIndex;

            ushort normalDamage = _unitData.RangedNormalDamage;
            ushort piercingDamage = _unitData.RangedPiercingDamage;
            ushort siegeDamage = _unitData.RangedSiegeDamage;
            float damageAreaPerc = _unitData.RangedDamageAreaPercent;
            
            projectileBase.Init(normalDamage, piercingDamage, siegeDamage, damageAreaPerc, UnitBase, 
                speedX*speedDirectionSign, speedY, gravity, lifetime, destroyOnHit, teamIndex);
            
            _gameManager.AddProjectile(projectileBase);

            return true;
        }
    }
}