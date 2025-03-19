using System;
using System.Collections.Generic;
using Vashta.CastlesOfWar.MapEntities;
using Vashta.CastlesOfWar.Simulation;
using Vashta.CastlesOfWar.Unit;

namespace Vashta.CastlesOfWar.AI
{
    // High-level AI that controls the default actions of units, unless overridden by the player or AI personality.
    public class TeamCommander : ISimulatedObject
    {
        public MapEntityBase TargetEntity { get; set; } // Current default target for units
        public MapEntityBase NextEntity { get; set; } // Target for when units are told to advance
        private GameManager _gameManager;
        private Team _team;
        
        public TeamCommander(GameManager gameManager, Team team)
        {
            _gameManager = gameManager;
            _team = team;

            _gameManager.EntityChanged += OnEntityChanged;
            CalculateTargetEntity();
        }

        ~TeamCommander()
        {
            _gameManager.EntityChanged -= OnEntityChanged;
        }
        
        public void OneStep(float timestep)
        {
            
        }

        public void CommandAdvance(short numberOfUnits)
        {
            List<UnitBase> units = _team.GetUnitsByDistanceFromBase();
            short unitsCommanded = 0;

            foreach (UnitBase unitBase in units)
            {
                if (!unitBase.TargetIsOverride)
                {
                    unitBase.SetTargetEntity(NextEntity, true);
                }

                // Stop when max units are hit
                if (unitsCommanded > numberOfUnits)
                    break;
            }
        }

        public void CommandHold(short numberOfUnits)
        {
            List<UnitBase> units = _team.GetUnitsByDistanceFromBase();
            short unitsCommanded = 0;

            foreach (UnitBase unitBase in units)
            {
                if (unitBase.TargetIsOverride && !_team.UnitIsPastTarget(unitBase))
                {
                    unitBase.SetTargetEntity(TargetEntity, false);
                }

                // Stop when max units are hit
                if (unitsCommanded > numberOfUnits)
                    break;
            }
        }

        public void CommandRetreat(short numberOfUnits)
        {
            List<UnitBase> units = _team.GetUnitsByDistanceFromBase();
            short unitsCommanded = 0;

            foreach (UnitBase unitBase in units)
            {
                if (unitBase.TargetIsOverride)
                {
                    unitBase.SetTargetEntity(TargetEntity, false);
                }

                // Stop when max units are hit
                if (unitsCommanded > numberOfUnits)
                    break;
            }
        }

        private void OnEntityChanged(object sender, EventArgs e)
        {
            CalculateTargetEntity();
        }

        private void CalculateTargetEntity()
        {
            List<OutpostBase> outposts = new List<OutpostBase>(_gameManager.Outposts);
            MapEntityBase previousEntity = TargetEntity;
            
            if (_team.TeamIndex == 0)
            {
                // Left
                outposts.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
            } else if (_team.TeamIndex == 1)
            {
                // Right
                outposts.Sort((a, b) => b.transform.position.x.CompareTo(a.transform.position.x));
            }

            // In case no outposts are owned, go to the first one
            TargetEntity = outposts[0];

            NextEntity = null;
            foreach (OutpostBase outpostBase in outposts)
            {
                if (outpostBase.TeamIndex == _team.TeamIndex)
                {
                    // A safe outpost for units, owned outpost
                    TargetEntity = outpostBase;
                }
                else
                {
                    // Next outpost to advance to
                    NextEntity = outpostBase;
                    break;
                }
            }

            // If all outposts are conquered, target base
            if (NextEntity == null)
            {
                NextEntity = _team.TeamIndex == 0 ? _gameManager.BaseLeft : _gameManager.BaseRight;
            }
            
            // If target has changed, update for units
            if (TargetEntity != previousEntity)
            {
                // Debug.Log("Setting team " + _team.TeamIndex + " target to: " + TargetEntity.DebugName);
                List<UnitBase> units = _team.Units;

                foreach (UnitBase unitBase in units)
                {
                    if (unitBase != null)
                    {
                        unitBase.SetTargetEntity(TargetEntity, false);
                    }
                }
            }
        }
    }
}