using System.Collections.Generic;
using UnityEngine;
using Vashta.CastlesOfWar.AI;
using Vashta.CastlesOfWar.Currency;
using Vashta.CastlesOfWar.MapEntities;
using Vashta.CastlesOfWar.Simulation;
using Vashta.CastlesOfWar.Unit;

namespace Vashta.CastlesOfWar
{
    public class Team : ISimulatedObject
    {
        public int[] GoldStart = new []{20, 10};
        
        public CurrencyController CurrencyController { get; private set; }
        
        public Base Spawn { get; set; }
        public Base EnemyBase { get; set; }
        public ushort TeamIndex { get; set; }
        public Color TeamColor { get; set; }

        public List<UnitBase> Units { get; private set; }
        public TeamCommander Commander { get; private set; }
        public GameManager GameManager { get; private set; }

        public Team(GameManager gameManager, ushort teamIndex, Base spawn, Base enemyBase)
        {
            CurrencyController = new CurrencyController(TeamIndex, gameManager);
            GameManager = gameManager;
            
            TeamIndex = teamIndex;
            Spawn = spawn;
            EnemyBase = enemyBase;

            Units = new List<UnitBase>();
            Commander = new TeamCommander(gameManager, this);

            CurrencyController.ModifyGold(GoldStart[teamIndex]);
        }

        public bool SpawnUnit(UnitData unitData)
        {
            if (unitData == null)
            {
                Debug.LogError("Cannot spawn, UnitData is null!");
                return false;
            }
            
            int goldCost = unitData.GoldCost;
            int teamGold = CurrencyController.Gold;

            if (goldCost > teamGold)
            {
                return false;
            }
            
            CurrencyController.ModifyGold(-goldCost);
            
            GameObject newUnit = Object.Instantiate(unitData.Prefab, Spawn.transform.position, Spawn.transform.rotation);
            UnitBase newUnitBase = newUnit.GetComponent<UnitBase>();
            newUnitBase.SetForTeam(TeamIndex == 0 ? Color.blue : Color.red); // TODO: replace with team definitions
            newUnitBase.Team = this;

            if (!newUnitBase)
            {
                Debug.LogError("Error spawning unit!");
            }
            
            newUnitBase.Init(this);
            Units.Add(newUnitBase);
            return true;
        }

        public void DespawnUnit(UnitBase unit)
        {
            Units.Remove(unit);
            Object.Destroy(unit.gameObject);
        }

        public void MoveUnits(float timestep)
        {
            foreach (UnitBase unitBase in Units)
            {
                unitBase.OneStep(timestep);
            }
        }

        public void OneStep(float timestep)
        {
            MoveUnits(timestep);
            CurrencyController.OneStep(timestep);
        }
        
        // 0 is furthest from base, max is closest
        public List<UnitBase> GetUnitsByDistanceFromBase()
        {
            List<UnitBase> sortedUnits = new List<UnitBase>(Units);

            if (TeamIndex == 0)
            {
                // Left
                sortedUnits.Sort((a, b) => b.transform.position.x.CompareTo(a.transform.position.x));
            } else if (TeamIndex == 1)
            {
                // Right
                sortedUnits.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
            }
            
            return sortedUnits;
        }

        public bool UnitIsPastTarget(UnitBase unit)
        {
            if (TeamIndex == 0)
            {
                return unit.transform.position.x > Commander.TargetEntity.transform.position.x;
            }
            else
            {
                return unit.transform.position.x < Commander.TargetEntity.transform.position.x;
            }
        }
    }
}