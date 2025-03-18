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
        public short CurrentHealth { get; set; }
        public ushort Armor { get; set; }
        
        [Header("Dependencies")]
        public UnitCombat Combat { get; set; }
        public UnitMovement Movement { get; set; }
        public UnitCombatCollider MeleeCollider;
        public UnitCombatCollider RangedCollider;
        
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
            Health = GetComponent<UnitHealth>();
            
            // Init
            Movement.unitBase = this;
            Combat.UnitBase = this;
            Combat.MeleeCollider = MeleeCollider;
            Combat.RangedCollider = RangedCollider;
            
            MeleeCollider.UnitBase = this;
            RangedCollider.UnitBase = this;
            
            Health.UnitBase = this;
        }
        
        public void Init(Team team)
        {
            Team = team;
            Speed = UnitData.Speed * Random.Range(.8f, 1.2f);
            Armor = UnitData.Armor;

            MaxHealth = UnitData.Health;
            CurrentHealth = (short)MaxHealth;
            Healthbar.SetMaxHealth(MaxHealth);
            Healthbar.SetHealth(CurrentHealth);
            Healthbar.SetText(UnitData.DisplayName);
            
            MeleeCollider.SetColliderWidth(UnitData.MeleeRange);
            RangedCollider.SetColliderWidth(UnitData.Range);
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