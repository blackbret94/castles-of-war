using System.Collections.Generic;
using UnityEngine;
using Vashta.CastlesOfWar.Util;

namespace Vashta.CastlesOfWar.Unit
{
    [RequireComponent(typeof(Collider2D))]
    public class UnitCombatCollider : MonoBehaviour
    {
        private BoxCollider2D _collider2D;
        public UnitBase UnitBase { get; set; }
        public List<UnitBase> Units = new List<UnitBase>();

        private Timer _pruneTimer;

        private void Awake()
        {
            _pruneTimer = new Timer(.25f);
            
            _collider2D = GetComponent<BoxCollider2D>();
            _collider2D.isTrigger = true;
        }

        private void Update()
        {
            if (_pruneTimer.Run())
            {
                PruneUnitsInRange();
            }
        }

        private void PruneUnitsInRange()
        {
            if (Units.Count > 0)
            {
                List<UnitBase> nearestUnits = new List<UnitBase>(Units);

                foreach (UnitBase unit in nearestUnits)
                {
                    if (unit == null)
                    {
                        Units.Remove(unit);
                        continue;
                    }

                    if (!unit.IsAlive())
                    {
                        Units.Remove(unit);
                        continue;
                    }
                }
            }
        }

        public void SetColliderWidth(float width)
        {
            _collider2D.size = new Vector2(width*2, _collider2D.size.y);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            UnitBase otherUnit = other.GetComponent<UnitBase>();
            
            if (otherUnit != null && otherUnit.TeamIndex != UnitBase.TeamIndex)
            {
                // add
                Units.Add(otherUnit);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            UnitBase otherUnit = other.GetComponent<UnitBase>();
            
            if (otherUnit != null && otherUnit.TeamIndex != UnitBase.TeamIndex)
            {
                // remove
                Units.Remove(otherUnit);
            }
        }
        
        // Prunes along the way
        public UnitBase GetNearestUnit(out float distanceToTarget)
        {
            distanceToTarget = float.MaxValue;
            UnitBase nearestUnit = null;
            
            if (Units.Count > 0)
            {
                float x = UnitBase.transform.position.x;
                
                // Copy for safety
                List<UnitBase> nearestUnits = new List<UnitBase>(Units);

                foreach (UnitBase unit in nearestUnits)
                {
                    if (unit == null)
                    {
                        Units.Remove(unit);
                        continue;
                    }

                    if (!unit.IsAlive())
                    {
                        Units.Remove(unit);
                        continue;
                    }

                    float distance = Mathf.Abs(x - unit.transform.position.x);

                    if (distance < distanceToTarget)
                    {
                        distanceToTarget = distance;
                        nearestUnit = unit;
                    }
                }
            }

            return nearestUnit;
        }
    }
}