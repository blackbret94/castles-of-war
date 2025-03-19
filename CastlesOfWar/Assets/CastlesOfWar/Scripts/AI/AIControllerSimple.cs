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
        public UnitDataDictionary UnitDataDictionary;
        
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
        
        public override void OneStep(float deltaTime)
        {
            // Attempt spawn
            if (_spawnTimer.Run())
            {
                UnitData unitData = null;
                
                if (Random.Range(0, 5) < 4)
                {
                    // Get a random unit.  May fail
                    unitData = UnitDataDictionary.GetRandom();
                }
                else
                {
                    // Get a unit we can afford
                    unitData = UnitDataDictionary.GetRandom((short)_team.CurrencyController.Gold);
                }
                
                GameManager.GetInstance().SpawnUnit(unitData, TeamIndex);
                
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