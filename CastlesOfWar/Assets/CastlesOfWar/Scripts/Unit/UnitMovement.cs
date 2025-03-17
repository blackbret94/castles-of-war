using UnityEngine;

namespace Vashta.CastlesOfWar.Unit
{
    public class UnitMovement : MonoBehaviour
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
            int dir = unitBase.TeamIndex == 0 ? 1 : -1;
            float speed = dir * _speed * timestep;
            
            // move here
            if (ShouldMove())
            {
                transform.position += new Vector3(speed, 0, 0);
            }
            
            // clamp
        }

        private bool ShouldMove()
        {
            if (unitBase.Combat.IsMovementBlocked())
                return false;
            
            if(unitBase.TeamIndex == 0)
            {
                return transform.position.x < unitBase.Team.EnemyBase.transform.position.x;
            }
            else
            {
                return transform.position.x > unitBase.Team.EnemyBase.transform.position.x;
            }
        }
    }
}