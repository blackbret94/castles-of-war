using System;
using UnityEngine;
using Vashta.CastlesOfWar.Simulation;
using Vashta.CastlesOfWar.Unit;

namespace Vashta.CastlesOfWar.Projectiles
{
    public class ProjectileBase : MonoBehaviour, ISimulatedObject
    {
        public ushort TeamIndex { get; private set; }
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

        public bool IsAlive { get; private set; } = true; // Can this actively do damage?
        
        public void Init(ushort normalDamage, ushort piercingDamage, ushort siegeDamage, float damageAreaPercent,
            UnitBase spawnedBy, float speedX, float speedY, float gravity, float lifetime, bool destroyOnHit, ushort teamIndex)
        {
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
        }

        public void OneStep(float deltaTime)
        {
            _speedY -= _gravity * deltaTime;
            
            float x = _speedX * deltaTime;
            float y = _speedY * deltaTime;
            
            transform.position += new Vector3(x, y, 0);
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
                    Destroy(gameObject);
                }
            }
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
                
                Destroy(gameObject);
            }
        }
    }
}