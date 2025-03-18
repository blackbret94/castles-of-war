namespace Vashta.CastlesOfWar.Util
{
    public class SimulationTimer
    {
        public float LastTime { get; private set; }
        public float TimeGap { get;}
        public bool HasRun { get; private set; }
        public GameManager GameManager;

        public SimulationTimer(GameManager gameManager, float timeGap)
        {
            GameManager = gameManager;
            TimeGap = timeGap;
        }

        public bool Run()
        {
            if (GameManager.Time > LastTime + TimeGap)
            {
                HasRun = true;
                LastTime = GameManager.Time;
                return true;
            }

            return false;
        }
    }
}