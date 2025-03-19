using System;
using System.Collections.Generic;
using UnityEngine;
using Vashta.CastlesOfWar.Combat;
using Vashta.CastlesOfWar.Simulation;
using Vashta.CastlesOfWar.Unit;
using Vashta.CastlesOfWar.Util;

namespace Vashta.CastlesOfWar.Projectiles
{
    public class ProjectileBase : MonoBehaviour, ISimulatedObject
    {
        public short TeamIndex { get; private set; }
        public GameObject SpawnOnDestruction;

        private ushort _normalDamage;
        private ushort _pierceDamage;
        private ushort _siegeDamage;
        private float _damageAreaPercent;
        private float _speedX;
        private float _speedY;
        private float _gravity;
        private float _lifetime;
        private bool _destroyOnHit;
        private UnitBase _spawnedBy;
        private UnitData _unitData;

        private SimulationTimer _destructionTimer;

        public bool IsAlive { get; private set; } = true; // Can this actively do damage?
        
        public void Init(UnitData unitData, ushort normalDamage, ushort piercingDamage, ushort siegeDamage, float damageAreaPercent,
            UnitBase spawnedBy, float speedX, float speedY, float gravity, float lifetime, bool destroyOnHit, short teamIndex)
        {
            _unitData = unitData;
            _normalDamage = normalDamage;
            _pierceDamage = piercingDamage;
            _siegeDamage = siegeDamage;
            _damageAreaPercent = damageAreaPercent;
            _spawnedBy = spawnedBy;
            _speedX = speedX;
            _speedY = speedY;
            _gravity = gravity;
            _lifetime = lifetime;
            _destroyOnHit = destroyOnHit;
            TeamIndex = teamIndex;
            
            _destructionTimer = new SimulationTimer(GameManager.GetInstance(), _lifetime, false);
        }

        public void OneStep(float deltaTime)
        {
            _speedY -= _gravity * deltaTime;
            
            float x = _speedX * deltaTime;
            float y = _speedY * deltaTime;
            
            transform.position += new Vector3(x, y, 0);

            if (_destructionTimer.Run())
            {
                DestroyProjectile();
            }
        }

        public void OnDestroy()
        {
            GameManager.GetInstance().RemoveProjectile(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            UnitBase otherUnit = other.GetComponent<UnitBase>();
            
            if (otherUnit != null && otherUnit.TeamIndex != TeamIndex)
            {
                // do damage
                short damageDone = otherUnit.Health.TakeAttack(_normalDamage, _pierceDamage, _siegeDamage, UnitCombatType.Ranged);
                
                if (_destroyOnHit)
                {
                    DestroyProjectile(otherUnit);
                }
            }
        }

        private void DestroyProjectile(UnitBase unitHit = null)
        {
            if (_unitData.RangedAreaOfEffectPrefab != null)
            {
                GameObject areaOfEffect = Instantiate(_unitData.RangedAreaOfEffectPrefab, transform.position, Quaternion.identity);
                AreaOfEffectBase aoeBase = areaOfEffect.GetComponent<AreaOfEffectBase>();

                if (!aoeBase)
                {
                    Debug.LogError("AreaOfEffectBase prefab is missing AreaOfEffectBase component!");
                }
                else
                {
                    ushort normalDamageAoe = (ushort)Mathf.FloorToInt(_normalDamage * _damageAreaPercent);
                    ushort piercingDamageAoe = (ushort)Mathf.FloorToInt(_pierceDamage * _damageAreaPercent);
                    ushort siegeDamageAoe = (ushort)Mathf.FloorToInt(_siegeDamage * _damageAreaPercent);
                    List<UnitBase> unitsToIgnore = new List<UnitBase>(){unitHit};
                    
                    aoeBase.Attack(GameManager.GetInstance(), TeamIndex,normalDamageAoe, piercingDamageAoe, siegeDamageAoe, 
                        _unitData.RangedDamageAreaRadius, UnitCombatType.Ranged, unitsToIgnore);
                }
            }
            
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            // If hit ground, destroy
            if (other.gameObject.CompareTag("Ground"))
            {
                if (SpawnOnDestruction != null)
                {
                    Instantiate(SpawnOnDestruction, transform.position, Quaternion.identity);
                }
                
                DestroyProjectile();
            }
        }
    }
}