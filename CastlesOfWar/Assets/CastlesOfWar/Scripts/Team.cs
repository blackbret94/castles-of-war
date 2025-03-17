using System.Collections.Generic;
using UnityEngine;
using Vashta.CastlesOfWar.Unit;

namespace Vashta.CastlesOfWar
{
    public class Team
    {
        public int Currency { get; set; }
        
        public LandmarkBase Spawn { get; set; }
        public LandmarkBase EnemyBase { get; set; }
        public ushort TeamIndex { get; set; }
        public Color TeamColor { get; set; }

        private List<UnitBase> _units;

        public void Init(ushort teamIndex, LandmarkBase spawn, LandmarkBase enemyBase)
        {
            TeamIndex = teamIndex;
            Spawn = spawn;
            EnemyBase = enemyBase;

            _units = new List<UnitBase>();
        }
        
        public void SpawnSpear()
        {
            GameObject newUnit = Object.Instantiate(GameManager.GetInstance().SpearPrefab, Spawn.transform.position, Spawn.transform.rotation);
            UnitBase newUnitBase = newUnit.GetComponent<UnitBase>();
            newUnitBase.SetForTeam(TeamIndex == 0 ? Color.blue : Color.red); // TODO: replace with team definitions

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
            newUnitBase.Team = this;

            if (!newUnitBase)
            {
                Debug.LogError("Error spawning unit!");
            }
            
            newUnitBase.Init(this);
            _units.Add(newUnitBase);
        }

        public void Advance()
        {
            
        }

        public void Retreat()
        {
            
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
    }
}