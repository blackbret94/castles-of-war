using System.Collections.Generic;
using UnityEngine;
using Vashta.CastlesOfWar.Unit;
using Vashta.CastlesOfWar.Util;

namespace Vashta.CastlesOfWar.Combat
{
    public class AreaOfEffectBase : MonoBehaviour
    {
        [Tooltip("Default radius, used to scale to damage radius")]
        public float BaseRadius;
        [Tooltip("Lifetime of visual.  Calculation only happens once.")]
        public float Lifetime;
        public Collider2D Collider;

        private SimulationTimer _timer;

        public void Attack(GameManager gameManager, short teamIndex, ushort normalDamage, ushort piercingDamage, 
            ushort siegeDamage, float radius, UnitCombatType combatType, List<UnitBase> unitsToIgnore)
        {
            _timer = new SimulationTimer(gameManager, Lifetime, false);
            
            float scaleRadius = radius / BaseRadius;
            transform.localScale = new Vector3(scaleRadius, scaleRadius, scaleRadius);
            
            List<Collider2D> results = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;

            Collider.Overlap(filter, results);

            foreach (Collider2D collider in results)
            {
                UnitBase otherUnit = collider.GetComponent<UnitBase>();

                if (unitsToIgnore.Contains(otherUnit))
                    continue;
                
                if (otherUnit != null && otherUnit.TeamIndex != teamIndex)
                {
                    otherUnit.Health.TakeAttack(normalDamage, piercingDamage, siegeDamage, combatType);
                }
            }
        }

        private void Update()
        {
            if (_timer != null && _timer.Run())
            {
                Destroy(gameObject);
            }
        }
    }
}