using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vashta.CastlesOfWar.AI;
using Vashta.CastlesOfWar.MapEntities;
using Vashta.CastlesOfWar.Projectiles;
using Vashta.CastlesOfWar.Simulation;
using Vashta.CastlesOfWar.UI;
using Vashta.CastlesOfWar.Unit;

namespace Vashta.CastlesOfWar
{
    public class GameManager : MonoBehaviour
    {
        public Team TeamLeft { get; set; }
        public Team TeamRight { get; set; }

        public List<Team> Teams { get; private set; }
        public GameState GameState { get; private set; } = GameState.WAITING;

        public Base BaseLeft;
        public Base BaseRight;

        public GamePanel VictoryPanel;
        public GamePanel DefeatPanel;
        public GamePanel StartGamePanel;

        public AIControllerBase EnemyAIController;
        
        private List<ProjectileBase> _projectiles;
        public List<OutpostBase> Outposts { get; private set; }
        public List<MapEntityBase> MapEntities { get; private set; }

        public float SimulationTime { get; private set; }
        
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
            
            // Init UI
            StartGamePanel.SetActive(true);
            VictoryPanel.SetActive(false);
            DefeatPanel.SetActive(false);
            
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
            if (GameState == GameState.RUNNING)
            {
                OneStep();
            }
        }

        private void OneStep()
        {
            float deltaTime = Time.deltaTime;
            SimulationTime += deltaTime;
            
            // Run enemy AI
            EnemyAIController.OneStep(deltaTime);
            
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

        public UnitBase SpawnUnit(UnitData unitData, int teamIndex)
        {
            if (unitData == null)
            {
                Debug.LogError("Cannot spawn, UnitData is null!");
                return null;
            }
            
            if (teamIndex >= Teams.Count)
            {
                Debug.LogWarning("Team index " + teamIndex + " is higher than the number of teams!");
                return null;
            }

            if (Teams[teamIndex].SpawnUnit(unitData, out UnitBase outUnit))
            {
                // Spawn success
            }
            else
            {
                // Spawn failed
            }

            return outUnit;
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

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void StartGame()
        {
            StartGamePanel.SetActive(false);
            GameState = GameState.RUNNING;
        }

        public void EndGame(short victorTeamIndex)
        {
            GameState = GameState.GAME_OVER;

            if (victorTeamIndex == 0)
            {
                VictoryPanel.gameObject.SetActive(true);
            }
            else
            {
                DefeatPanel.gameObject.SetActive(true);
            }
        }
    }
}

