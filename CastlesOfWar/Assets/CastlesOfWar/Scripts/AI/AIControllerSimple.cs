using UnityEngine;
using Vashta.CastlesOfWar.Unit;
using Vashta.CastlesOfWar.Util;

namespace Vashta.CastlesOfWar.AI
{
    public class AIControllerSimple : AIControllerBase
    {
        public UnitData SpearUnit;
        public UnitData SlingerUnit;
        
        public float SpawnRate = 1.5f;
        private Timer _spawnTimer;

        private void Awake()
        {
            _spawnTimer = new Timer(SpawnRate);
        }

        private void Update()
        {
            OneStep();
        }
        
        public override void OneStep()
        {
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
            }
        }
    }
}