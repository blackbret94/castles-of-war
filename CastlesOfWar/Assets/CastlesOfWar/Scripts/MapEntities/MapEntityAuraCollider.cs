using System.Collections.Generic;
using UnityEngine;
using Vashta.CastlesOfWar.Unit;

namespace Vashta.CastlesOfWar.MapEntities
{
    public class MapEntityAuraCollider : MonoBehaviour
    {
        private BoxCollider2D _collider2D;
        public MapEntityBase MapEntity;
        private List<UnitBase> _units = new List<UnitBase>();
        
        private void Awake()
        {
            _collider2D = GetComponent<BoxCollider2D>();
            _collider2D.isTrigger = true;
        }
        
        public void SetColliderWidth(float width)
        {
            _collider2D.size = new Vector2(width*2, _collider2D.size.y);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            UnitBase otherUnit = other.GetComponent<UnitBase>();
            
            if (otherUnit != null && otherUnit.TeamIndex == MapEntity.TeamIndex)
            {
                otherUnit.SetEntityAura(MapEntity);
                _units.Add(otherUnit);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            UnitBase otherUnit = other.GetComponent<UnitBase>();
            
            if (otherUnit != null && otherUnit.TeamIndex == MapEntity.TeamIndex)
            {
                otherUnit.RemoveEntityAura(MapEntity);
                _units.Remove(otherUnit);
            }
        }

        public void ClearOverlaps()
        {
            foreach (UnitBase unitBase in _units)
            {
                if (unitBase != null)
                {
                    unitBase.RemoveEntityAura(MapEntity);
                }
            }
            
            _units.Clear();
        }

        public void ReCheckForOverlaps()
        {
            foreach (UnitBase unitBase in _units)
            {
                if (unitBase != null)
                {
                    unitBase.RemoveEntityAura(MapEntity);
                }
            }
            
            _units.Clear();
            
            List<Collider2D> results = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;

            _collider2D.Overlap(filter, results);

            foreach (Collider2D collider in results)
            {
                OnTriggerEnter2D(collider);
            }
        }
    }
}