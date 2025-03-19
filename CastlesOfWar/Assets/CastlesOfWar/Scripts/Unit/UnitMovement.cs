using UnityEngine;
using Vashta.CastlesOfWar.MapEntities;
using Vashta.CastlesOfWar.Simulation;

namespace Vashta.CastlesOfWar.Unit
{
    public class UnitMovement : MonoBehaviour, ISimulatedObject
    {
        public UnitBase unitBase { get; set; }
        private float _speed => unitBase.Speed;
        private float _targetX;
        private const float CLAMP_X = .05f;

        private void Awake()
        {
            _targetX = transform.position.x;
        }
        
        public void OneStep(float timestep)
        {
            if(ShouldMove(_targetX))
                Move(timestep);
        }
        
        public void Move(float timestep)
        {
            MapEntityBase target = unitBase.Target;

            if (target == null)
            {
                Debug.LogError("Unit has null target, cannot move!");
                return;
            }
            
            Vector3 pos = transform.position;
            
            int dir = _targetX - pos.x > 0 ? 1 : -1; 
            float speed = dir * _speed * timestep;
            
            // move here
            if (Mathf.Abs(pos.x - _targetX) > speed)
            {
                transform.position += new Vector3(speed, 0, 0);
                
                // clamp
                if (Mathf.Abs(pos.x - _targetX) < CLAMP_X)
                {
                    transform.position = new Vector3(_targetX, pos.y, pos.z); 
                }
            }
            else
            {
                transform.position = new Vector3(_targetX, pos.y, pos.z);
            }
        }

        public void RecalculateTargetX()
        {
            MapEntityBase target = unitBase.Target;

            if (target == null)
            {
                Debug.LogError("Unit has null target, cannot move!");
                return;
            }
            
            int distanceFromTargetDir = unitBase.TeamIndex == 0 ? 1 : -1;
            float distanceFromTarget = unitBase.UnitData.DistanceToStandFromTarget * distanceFromTargetDir;
            _targetX = unitBase.Target.transform.position.x + distanceFromTarget;
        }

        private bool ShouldMove(float targetX)
        {
            return !unitBase.Combat.IsMovementBlocked() && Mathf.Abs(transform.position.x - targetX) > CLAMP_X;
        }
    }
}