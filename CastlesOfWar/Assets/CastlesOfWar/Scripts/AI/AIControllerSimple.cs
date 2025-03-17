using Vashta.CastlesOfWar.Util;

namespace Vashta.CastlesOfWar.AI
{
    public class AIControllerSimple : AIControllerBase
    {
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
                GameManager.GetInstance().SpawnSpear(TeamIndex);
            }
        }
    }
}