using UnityEngine;
using Vashta.CastlesOfWar.Unit;

namespace Vashta.CastlesOfWar.MapEntities
{
    public class MapEntityAuraCollider : MonoBehaviour
    {
        private BoxCollider2D _collider2D;
        public MapEntityBase MapEntity;


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
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            UnitBase otherUnit = other.GetComponent<UnitBase>();
            
            if (otherUnit != null && otherUnit.TeamIndex == MapEntity.TeamIndex)
            {
                otherUnit.RemoveEntityAura(MapEntity);
            }
        }
    }
}