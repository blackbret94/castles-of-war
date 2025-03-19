namespace Vashta.CastlesOfWar.Util
{
    public class SimulationTimer
    {
        public float LastTime { get; private set; }
        public float TimeGap { get; private set; }
        public bool HasRun { get; private set; }
        public GameManager GameManager;

        public SimulationTimer(GameManager gameManager, float timeGap, bool runImmediately=true)
        {
            GameManager = gameManager;
            TimeGap = timeGap;

            if (!runImmediately)
                LastTime = GameManager.SimulationTime;
        }

        public bool Run()
        {
            if (GameManager.SimulationTime > LastTime + TimeGap)
            {
                HasRun = true;
                LastTime = GameManager.SimulationTime;
                return true;
            }

            return false;
        }

        public void SetNewTimeGap(float newTimeGap)
        {
            TimeGap = newTimeGap;
        }
    }
}