using System;
using System.Collections.Generic;
using UnityEngine;
using Vashta.CastlesOfWar.MapEntities;
using Vashta.CastlesOfWar.Unit;
using Vashta.CastlesOfWar.Util;
using Random = UnityEngine.Random;

namespace Vashta.CastlesOfWar.AI
{
    public class AIControllerSimple : AIControllerBase
    {
        public UnitData SpearUnit;
        public UnitData SlingerUnit;
        
        [Tooltip("How often does it attempt to spawn a unit?")]
        public float SpawnRate = 1.5f;
        [Tooltip("Minimum time between advancing to the next trench")]
        public float AdvanceRateBaseMin = 10f;
        [Tooltip("Maximum time between advancing to the next trench")]
        public float AdvanceRateBaseMax = 30f;
        private SimulationTimer _spawnTimer;
        private SimulationTimer _advanceTimer;
        private GameManager _gameManager;
        private Team _team;
        private bool _neutralOutpostExists;

        private void Start()
        {
            _gameManager = GameManager.GetInstance();
            _team = _gameManager.TeamRight;
            _spawnTimer = new SimulationTimer(_gameManager, SpawnRate);
            _advanceTimer = new SimulationTimer(_gameManager, AdvanceRateBaseMax);
            
            _gameManager.EntityChanged += OnEntityChanged;
        }

        private void Update()
        {
            OneStep();
        }
        
        public override void OneStep()
        {
            // Attempt spawn
            if (_spawnTimer.Run())
            {
                if (Random.Range(0, 5) < 4)
                {
                    GameManager.GetInstance().SpawnUnit(SpearUnit, TeamIndex);
                }
                else
                {
                    GameManager.GetInstance().SpawnUnit(SlingerUnit, TeamIndex);
                }
                
                // If a neutral outpost exists, go for it
                if(_neutralOutpostExists)
                    _team.Commander.CommandAdvance(short.MaxValue);
            }
            
            // Advance
            if (_advanceTimer.Run())
            {
                _advanceTimer.SetNewTimeGap(Random.Range(AdvanceRateBaseMin, AdvanceRateBaseMax));
                _team.Commander.CommandAdvance(short.MaxValue);
            }
        }

        private void OnEntityChanged(object sender, EventArgs e)
        {
            // Check if any outposts are neutral
            if (CheckForNeutralOutpost())
            {
                // Set all troops to advance if true
                _team.Commander.CommandAdvance(short.MaxValue);
            }
        }

        private bool CheckForNeutralOutpost()
        {
            List<OutpostBase> outposts = _gameManager.Outposts;
            _neutralOutpostExists = false;
            
            foreach (OutpostBase outpost in outposts)
            {
                if (outpost.TeamIndex == -1)
                {
                    _neutralOutpostExists = true;
                    break;
                }
            }
            
            return _neutralOutpostExists;
        }
    }
}