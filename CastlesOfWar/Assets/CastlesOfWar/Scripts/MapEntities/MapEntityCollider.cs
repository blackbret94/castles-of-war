using System.Collections.Generic;
using UnityEngine;
using Vashta.CastlesOfWar.Unit;
using Vashta.CastlesOfWar.Util;

namespace Vashta.CastlesOfWar.MapEntities
{
    public class MapEntityCollider : MonoBehaviour
    {
        private BoxCollider2D _collider2D;
        public MapEntityBase MapEntity;
        private List<UnitBase> UnitsLeft = new List<UnitBase>();
        private List<UnitBase> UnitsRight = new List<UnitBase>();

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
            PruneList(UnitsLeft);
            PruneList(UnitsRight);
        }

        private void PruneList(List<UnitBase> units)
        {
            if (units.Count > 0)
            {
                List<UnitBase> nearestUnits = new List<UnitBase>(units);

                foreach (UnitBase unit in nearestUnits)
                {
                    if (unit == null)
                    {
                        units.Remove(unit);
                        continue;
                    }

                    if (!unit.IsAlive())
                    {
                        units.Remove(unit);
                        continue;
                    }
                }
            }
        }

        public short GetDominantTeam()
        {
            if (UnitsLeft.Count > 0 && UnitsRight.Count == 0)
                return 0;
            if (UnitsRight.Count > 0 && UnitsLeft.Count == 0)
                return 1;

            return -1;
        }
        
        public void SetColliderWidth(float width)
        {
            _collider2D.size = new Vector2(width*2, _collider2D.size.y);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            UnitBase otherUnit = other.GetComponent<UnitBase>();
            
            if (otherUnit != null)
            {
                // add
                AddUnit(otherUnit);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            UnitBase otherUnit = other.GetComponent<UnitBase>();
            
            if (otherUnit != null)
            {
                // remove
                RemoveUnit(otherUnit);
            }
        }

        private void AddUnit(UnitBase unit)
        {
            if(unit.TeamIndex == 0)
                UnitsLeft.Add(unit);
            else if (unit.TeamIndex == 1)
                UnitsRight.Add(unit);
        }

        private void RemoveUnit(UnitBase unit)
        {
            if(unit.TeamIndex == 0)
                UnitsLeft.Remove(unit);
            else if (unit.TeamIndex == 1)
                UnitsRight.Remove(unit);
        }
    }
}