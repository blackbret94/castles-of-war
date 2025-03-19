using UnityEngine;

namespace Vashta.CastlesOfWar.Util
{
    public class Timer
    {
        public float LastTime { get; private set; }
        public float TimeGap { get; private set; }
        public bool HasRun { get; private set; }

        public Timer(float timeGap)
        {
            TimeGap = timeGap;
        }

        public bool Run()
        {
            if (Time.time > LastTime + TimeGap)
            {
                HasRun = true;
                LastTime = Time.time;
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