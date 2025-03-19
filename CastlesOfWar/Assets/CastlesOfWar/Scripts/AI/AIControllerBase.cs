using UnityEngine;
using Vashta.CastlesOfWar.Simulation;

namespace Vashta.CastlesOfWar.AI
{
    public class AIControllerBase : MonoBehaviour, ISimulatedObject
    {
        public short TeamIndex;

        public virtual void OneStep(float timestep)
        {
        }
    }
}