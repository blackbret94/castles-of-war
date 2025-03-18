using System;
using System.Collections.Generic;
using UnityEngine;
using Vashta.CastlesOfWar.Projectiles;

namespace Vashta.CastlesOfWar
{
    public class GameManager : MonoBehaviour
    {
        public Team TeamLeft { get; set; }
        public Team TeamRight { get; set; }

        private List<Team> Teams;

        public LandmarkBase SpawnerLeft;
        public LandmarkBase SpawnerRight;

        public GameObject SpearPrefab;
        public GameObject SlingerPrefab;
        
        private List<ProjectileBase> _projectiles;
        
        public float Time { get; private set; }

        private static GameManager _instance;

        public static GameManager GetInstance()
        {
            return _instance;
        }

        private void Awake()
        {
            _instance = this;
            _projectiles = new List<ProjectileBase>();
        }
        
        void Start()
        {
            TeamLeft = new Team();
            TeamRight = new Team();

            Teams = new List<Team>{TeamLeft, TeamRight};
            TeamLeft.Init(0, SpawnerLeft, SpawnerRight);
            TeamRight.Init(1, SpawnerRight, SpawnerLeft);
        }

        private void Update()
        {
            float deltaTime = UnityEngine.Time.deltaTime;
            Time += deltaTime;
            TeamLeft.MoveUnits(deltaTime);
            TeamRight.MoveUnits(deltaTime);

            List<ProjectileBase> projectiles = new List<ProjectileBase>(_projectiles);
            foreach (ProjectileBase projectileBase in projectiles)
            {
                if (projectileBase == null)
                {
                    _projectiles.Remove(projectileBase);
                }
                
                projectileBase.OneStep(deltaTime);
            }
        }

        public void SpawnSpear(int teamIndex)
        {
            if (teamIndex >= Teams.Count)
            {
                Debug.LogWarning("Team index " + teamIndex + " is higher than the number of teams!");
                return;
            }
            
            Teams[teamIndex].SpawnSpear();
        }

        public void SpawnSlinger(int teamIndex)
        {
            if (teamIndex >= Teams.Count)
            {
                Debug.LogWarning("Team index " + teamIndex + " is higher than thee number of teams!");
                return;
            }
            
            Teams[teamIndex].SpawnSlinger();
        }

        public void Advance(int teamIndex)
        {
            if (teamIndex >= Teams.Count)
            {
                Debug.LogWarning("Team index " + teamIndex + " is higher than thee number of teams!");
                return;
            }
            
            Teams[teamIndex].Advance(teamIndex);
        }

        public void Hold(int teamIndex)
        {
            if (teamIndex >= Teams.Count)
            {
                Debug.LogWarning("Team index " + teamIndex + " is higher than thee number of teams!");
                return;
            }

            Teams[teamIndex].Advance(teamIndex);
        }

        public void Retreat(int teamIndex)
        {
            if (teamIndex >= Teams.Count)
            {
                Debug.LogWarning("Team index " + teamIndex + " is higher than thee number of teams!");
                return;
            }
            
            Teams[teamIndex].Retreat(teamIndex);
        }

        public void AddProjectile(ProjectileBase projectile)
        {
            _projectiles.Add(projectile);
        }

        public void RemoveProjectile(ProjectileBase projectile)
        {
            _projectiles.Remove(projectile);
        }
    }
}

