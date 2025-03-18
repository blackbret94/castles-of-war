using System.Collections.Generic;
using UnityEngine;
using Vashta.CastlesOfWar.Currency;
using Vashta.CastlesOfWar.Simulation;
using Vashta.CastlesOfWar.Unit;

namespace Vashta.CastlesOfWar
{
    public class Team : ISimulatedObject
    {
        public int[] GoldStart = new []{20, 10};
        
        public CurrencyController CurrencyController { get; private set; }
        
        public LandmarkBase Spawn { get; set; }
        public LandmarkBase EnemyBase { get; set; }
        public ushort TeamIndex { get; set; }
        public Color TeamColor { get; set; }

        private List<UnitBase> _units;

        public Team()
        {
            CurrencyController = new CurrencyController();
        }

        public void Init(ushort teamIndex, LandmarkBase spawn, LandmarkBase enemyBase)
        {
            TeamIndex = teamIndex;
            Spawn = spawn;
            EnemyBase = enemyBase;

            _units = new List<UnitBase>();

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
            _units.Add(newUnitBase);
            return true;
        }
        
        public void SpawnSpear()
        {
            GameObject newUnit = Object.Instantiate(GameManager.GetInstance().SpearPrefab, Spawn.transform.position, Spawn.transform.rotation);
            UnitBase newUnitBase = newUnit.GetComponent<UnitBase>();
            newUnitBase.SetForTeam(TeamIndex == 0 ? Color.blue : Color.red); // TODO: replace with team definitions
            newUnitBase.Team = this;

            if (!newUnitBase)
            {
                Debug.LogError("Error spawning unit!");
            }
            
            newUnitBase.Init(this);
            _units.Add(newUnitBase);
        }

        public void SpawnSlinger()
        {
            GameObject newUnit = Object.Instantiate(GameManager.GetInstance().SlingerPrefab, Spawn.transform.position, Spawn.transform.rotation);
            UnitBase newUnitBase = newUnit.GetComponent<UnitBase>();
            newUnitBase.SetForTeam(TeamIndex == 0 ? Color.blue : Color.red); // TODO: replace with team definitions
            newUnitBase.Team = this;

            if (!newUnitBase)
            {
                Debug.LogError("Error spawning unit!");
            }
            
            newUnitBase.Init(this);
            _units.Add(newUnitBase);
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
            _units.Remove(unit);
            Object.Destroy(unit.gameObject);
        }

        public void MoveUnits(float timestep)
        {
            foreach (UnitBase unitBase in _units)
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