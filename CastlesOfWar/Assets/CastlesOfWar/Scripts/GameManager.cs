using System;
using System.Collections.Generic;
using UnityEngine;

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

        private static GameManager _instance;

        public static GameManager GetInstance()
        {
            return _instance;
        }
        
        void Start()
        {
            _instance = this;
            TeamLeft = new Team();
            TeamRight = new Team();

            Teams = new List<Team>{TeamLeft, TeamRight};
            TeamLeft.Init(0, SpawnerLeft, SpawnerRight);
            TeamRight.Init(1, SpawnerRight, SpawnerLeft);
        }

        private void Update()
        {
            TeamLeft.MoveUnits(Time.deltaTime);
            TeamRight.MoveUnits(Time.deltaTime);
        }

        public void SpawnSpear(int teamIndex)
        {
            if (teamIndex >= Teams.Count)
            {
                Debug.LogWarning("Team index " + teamIndex + " is higher than thee number of teams!");
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
            
            Teams[teamIndex].Advance();
        }

        public void Retreat(int teamIndex)
        {
            if (teamIndex >= Teams.Count)
            {
                Debug.LogWarning("Team index " + teamIndex + " is higher than thee number of teams!");
                return;
            }
            
            Teams[teamIndex].Retreat();
        }
    }
}

