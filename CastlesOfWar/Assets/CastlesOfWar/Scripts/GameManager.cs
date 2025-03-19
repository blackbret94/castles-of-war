using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Vashta.CastlesOfWar.MapEntities;
using Vashta.CastlesOfWar.Projectiles;
using Vashta.CastlesOfWar.Unit;

namespace Vashta.CastlesOfWar
{
    public class GameManager : MonoBehaviour
    {
        public Team TeamLeft { get; set; }
        public Team TeamRight { get; set; }

        public List<Team> Teams { get; private set; }

        [FormerlySerializedAs("SpawnerLeft")] public Base BaseLeft;
        [FormerlySerializedAs("SpawnerRight")] public Base BaseRight;
        
        private List<ProjectileBase> _projectiles;
        public List<OutpostBase> Outposts { get; private set; }
        public List<MapEntityBase> MapEntities { get; private set; }

        public float Time { get; private set; }
        
        private static GameManager _instance;
        
        public event EventHandler EntityChanged;

        public static GameManager GetInstance()
        {
            return _instance;
        }

        private void Awake()
        {
            // Init this
            _instance = this;
            _projectiles = new List<ProjectileBase>();
            
            // Register map entities
            Outposts = new List<OutpostBase>(FindObjectsByType<OutpostBase>(FindObjectsSortMode.None));
            MapEntities = new List<MapEntityBase>(FindObjectsByType<MapEntityBase>(FindObjectsSortMode.None));
            
            // Configure teams (must come after map is registered)
            TeamLeft = new Team(this, 0, BaseLeft, BaseRight);
            TeamRight = new Team(this, 1, BaseRight, BaseLeft);
            Teams = new List<Team>{TeamLeft, TeamRight};
        }
        
        private void Update()
        {
            float deltaTime = UnityEngine.Time.deltaTime;
            Time += deltaTime;
            
            // Run teams and units
            TeamLeft.OneStep(deltaTime);
            TeamRight.OneStep(deltaTime);

            // Run projectiles
            List<ProjectileBase> projectiles = new List<ProjectileBase>(_projectiles);
            foreach (ProjectileBase projectileBase in projectiles)
            {
                if (projectileBase == null)
                {
                    _projectiles.Remove(projectileBase);
                }
                
                projectileBase.OneStep(deltaTime);
            }
            
            // Run map entities
            foreach (MapEntityBase mapEntity in MapEntities)
            {
                mapEntity.OneStep(deltaTime);
            }
        }

        public Team GetEnemyTeam(int teamIndex)
        {
            return teamIndex == 0 ? TeamRight : TeamLeft;
        }

        public void SpawnUnit(UnitData unitData, int teamIndex)
        {
            if (unitData == null)
            {
                Debug.LogError("Cannot spawn, UnitData is null!");
                return;
            }
            
            if (teamIndex >= Teams.Count)
            {
                Debug.LogWarning("Team index " + teamIndex + " is higher than the number of teams!");
                return;
            }

            if (Teams[teamIndex].SpawnUnit(unitData))
            {
                // Spawn success
            }
            else
            {
                // Spawn failed
            }
        }

        public void AddProjectile(ProjectileBase projectile)
        {
            _projectiles.Add(projectile);
        }

        public void RemoveProjectile(ProjectileBase projectile)
        {
            _projectiles.Remove(projectile);
        }

        public void EventEntityChanged()
        {
            EntityChanged?.Invoke(this, null);
        }
    }
}

