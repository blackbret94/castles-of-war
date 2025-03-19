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

        public void Advance(int teamIndex)
        {
            Debug.Log("Advancing");
        }

        public void Hold(int teamIndex)
        {
            Debug.Log("Holding");
        }

        public void Retreat(int teamIndex)
        {
            Debug.Log("Retreating");
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
    }
}