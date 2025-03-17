using UnityEngine;
using Vashta.CastlesOfWar.Simulation;
using Vashta.CastlesOfWar.UI;

namespace Vashta.CastlesOfWar.Unit
{
    [RequireComponent(typeof(UnitCombat))]
    [RequireComponent(typeof(UnitMovement))]
    [RequireComponent(typeof(UnitHealth))]
    public class UnitBase : MonoBehaviour, ISimulatedObject
    {
        public Team Team { get; set; }
        public ushort TeamIndex { get=>Team.TeamIndex; }
        public float Speed { get; set; }
        public ushort MaxHealth { get; set; }
        public ushort CurrentHealth { get; set; }
        public ushort Armor { get; set; }
        
        [Header("Dependencies")]
        public UnitCombat Combat { get; set; }
        public UnitMovement Movement { get; set; }
        public UnitMeleeCollider MeleeCollider { get; set; }
        public UnitHealth Health { get; set; }
        public UnitData UnitData;
        public Transform AttackOrigin;
        public SpriteRenderer SpriteRenderer;
        public Healthbar Healthbar;
        
        private void Awake()
        {
            // Get references
            Combat = GetComponent<UnitCombat>();
            Movement = GetComponent<UnitMovement>();
            MeleeCollider = GetComponentInChildren<UnitMeleeCollider>();
            Health = GetComponent<UnitHealth>();
            
            // Init
            Movement.unitBase = this;
            Combat.UnitBase = this;
            Combat.MeleeCollider = MeleeCollider;
            MeleeCollider.UnitBase = this;
            Health.UnitBase = this;
        }
        
        public void Init(Team team)
        {
            Team = team;
            Speed = UnitData.Speed * Random.Range(.8f, 1.2f);
            Armor = UnitData.Armor;

            MaxHealth = UnitData.Health;
            CurrentHealth = MaxHealth;
            Healthbar.SetMaxHealth(MaxHealth);
            Healthbar.SetHealth(CurrentHealth);
            
            MeleeCollider.SetColliderWidth(UnitData.MeleeRange);
        }

        public void OneStep(float timestep)
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