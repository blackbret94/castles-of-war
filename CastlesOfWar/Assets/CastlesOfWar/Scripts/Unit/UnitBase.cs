using System.Collections.Generic;
using UnityEngine;
using Vashta.CastlesOfWar.AI;
using Vashta.CastlesOfWar.MapEntities;
using Vashta.CastlesOfWar.Simulation;
using Vashta.CastlesOfWar.UI;

namespace Vashta.CastlesOfWar.Unit
{
    [RequireComponent(typeof(UnitCombat))]
    [RequireComponent(typeof(UnitMovement))]
    [RequireComponent(typeof(UnitHealth))]
    public class UnitBase : MonoBehaviour, ISimulatedObject
    {
        [Tooltip("Instance name for debugging purposes only")]
        public string DebugName;
        public Team Team { get; set; }
        public TeamCommander Commander { get; private set; }
        public ushort TeamIndex { get=>Team.TeamIndex; }
        
        // Stats
        public float BaseSpeed { get; set; }
        public float Speed { get; private set; }
        public ushort MaxHealth { get; set; }
        public short CurrentHealth { get; set; }
        public ushort Armor { get; private set; }
        public float MeleeVulnerability { get; private set; }
        public float RangedVulnerability { get; private set; }
        
        // AI
        public MapEntityBase Target { get; private set; }
        public bool TargetIsOverride { get; private set; } // Was this set by the commander or by the Player/AI Personality?
        
        // Modifiers
        public MapEntityBase EntityAura { get; private set; }
        
        [Header("Dependencies")]
        public UnitCombat Combat { get; set; }
        public UnitMovement Movement { get; set; }
        public UnitCombatCollider MeleeCollider;
        public UnitCombatCollider RangedCollider;
        
        public UnitHealth Health { get; set; }
        public UnitData UnitData;
        public Transform AttackOrigin;
        public List<SpriteRenderer> SpritesToColor;
        public SpriteRenderer Aura;
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
            Commander = Team.Commander;
            Target = Commander.TargetEntity;
            BaseSpeed = UnitData.Speed * Random.Range(.8f, 1.2f);
            CalculateStats();

            MaxHealth = UnitData.Health;
            CurrentHealth = (short)MaxHealth;
            Healthbar.SetMaxHealth(MaxHealth);
            Healthbar.SetHealth(CurrentHealth);
            Healthbar.SetText(UnitData.DisplayName);
            
            MeleeCollider.SetColliderWidth(UnitData.MeleeRange);
            RangedCollider.SetColliderWidth(UnitData.Range);
            
            Aura.gameObject.SetActive(false);
        }

        // Use base values and modifiers to update unit stats
        private void CalculateStats()
        {
            Armor = UnitData.Armor;
            Speed = BaseSpeed;
            RangedVulnerability = UnitData.VulnerabilityRange;
            MeleeVulnerability = UnitData.VulnerabilityMelee;

            if (EntityAura != null)
            {
                MapEntityData entityData = EntityAura.MapEntityData;
                Armor += (ushort)entityData.BuffArmor;
                Speed *= entityData.SpeedModifierPerc;
                RangedVulnerability *= entityData.BuffVulnerabilityRanged;
                MeleeVulnerability *= entityData.VulnerabilityMelee;
            }
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

        public void SetForTeam(Color mainColor, Color auraColor)
        {
            foreach (SpriteRenderer spriteRenderer in SpritesToColor)
            {
                spriteRenderer.color = mainColor;
            }

            Aura.color = auraColor;
        }

        public void SetTargetEntity(MapEntityBase entity, bool isOverride)
        {
            Target = entity;
            TargetIsOverride = isOverride;
        }

        public void CommandAdvance()
        {
            Target = Commander.NextEntity;
            TargetIsOverride = true;
        }

        public void SetEntityAura(MapEntityBase entity)
        {
            EntityAura = entity;
            Aura.gameObject.SetActive(true);
            CalculateStats();
        }

        public void RemoveEntityAura(MapEntityBase entity)
        {
            if (EntityAura == entity)
            {
                EntityAura = null;
                Aura.gameObject.SetActive(false);
                CalculateStats();
            }
        }
    }
}