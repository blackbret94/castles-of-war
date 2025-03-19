using UnityEngine;
using Vashta.CastlesOfWar.MapEntities;
using Vashta.CastlesOfWar.Simulation;

namespace Vashta.CastlesOfWar.Unit
{
    public class UnitMovement : MonoBehaviour, ISimulatedObject
    {
        public UnitBase unitBase { get; set; }
        private float _speed => unitBase.Speed;
        
        public void OneStep(float timestep)
        {
            if(ShouldMove())
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
            Vector3 targetPos = target.transform.position;
            
            int dir = targetPos.x - pos.x > 0 ? 1 : -1; 
            float speed = dir * _speed * timestep;
            
            // move here
            if (ShouldMove())
            {
                float targetX = unitBase.Target.transform.position.x;
                
                if (Mathf.Abs(pos.x - targetX) > speed)
                {
                    transform.position += new Vector3(speed, 0, 0);
                }
                else
                {
                    transform.position = new Vector3(targetX, pos.y, pos.z);
                }
            }
        }

        private bool ShouldMove()
        {
            if (unitBase.Combat.IsMovementBlocked())
                return false;

            return Mathf.Abs(transform.position.x - unitBase.Target.transform.position.x) > .1f;
        }
    }
}