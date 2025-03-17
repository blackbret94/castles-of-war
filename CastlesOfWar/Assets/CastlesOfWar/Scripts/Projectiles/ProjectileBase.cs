using UnityEngine;
using Vashta.CastlesOfWar.Simulation;
using Vashta.CastlesOfWar.Unit;

namespace Vashta.CastlesOfWar.Projectiles
{
    public class ProjectileBase : MonoBehaviour, ISimulatedObject
    {
        public ushort TeamIndex { get; private set; }
        private float _speedX;
        private float _speedY;
        private float _gravity;
        private float _lifetime;
        private bool _destroyOnHit;
        private UnitBase _spawnedBy;

        public void Init(UnitBase spawnedBy, bool dirMoveRight, Vector2 speedOffset, float speed, float gravity, 
            float lifetime, bool destroyOnHit, ushort teamIndex)
        {
            _spawnedBy = spawnedBy;
            _speedX = (speed + speedOffset.x) * (dirMoveRight ? 1f : -1f);
            _speedY = speedOffset.y;
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
    }
}