using UnityEngine;
using Vashta.CastlesOfWar.ScriptableObject;

namespace Vashta.CastlesOfWar.Unit
{
    [RequireComponent(typeof(UnitCombat))]
    [RequireComponent(typeof(UnitMovement))]
    [RequireComponent(typeof(UnitDamage))]
    public class UnitBase : MonoBehaviour
    {
        public Team Team { get; set; }
        public ushort TeamIndex { get=>Team.TeamIndex; }
        public float Speed { get; set; }
        public ushort MaxHealth { get; set; }
        public ushort CurrentHealth { get; set; }
        public ushort Armor { get; set; }
        public UnitData UnitData;
        public Transform AttackOrigin;
        public SpriteRenderer SpriteRenderer;

        // Dependencies
        public UnitCombat Combat { get; set; }
        public UnitMovement Movement { get; set; }
        public UnitMeleeCollider MeleeCollider { get; set; }
        public UnitDamage Damage { get; set; }
        
        private void Awake()
        {
            // Get references
            Combat = GetComponent<UnitCombat>();
            Movement = GetComponent<UnitMovement>();
            MeleeCollider = GetComponentInChildren<UnitMeleeCollider>();
            Damage = GetComponent<UnitDamage>();
            
            // Init
            Movement.unitBase = this;
            Combat.UnitBase = this;
            Combat.MeleeCollider = MeleeCollider;
            MeleeCollider.UnitBase = this;
            Damage.UnitBase = this;
        }
        
        public void Init(Team team)
        {
            Team = team;
            Speed = UnitData.Speed * Random.Range(.8f, 1.2f);
            MaxHealth = UnitData.Health;
            CurrentHealth = MaxHealth;
            Armor = UnitData.Armor;
            MeleeCollider.SetColliderWidth(UnitData.MeleeRange);
        }

        public void OneTimeStep(float timestep)
        {
            Combat.OneStep(timestep);
            Movement.OneStep(timestep);
        }
        
        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }

        public void SetForTeam(Color color)
        {
            SpriteRenderer.color = color;
        }

    }
}