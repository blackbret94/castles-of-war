using UnityEngine;

namespace Vashta.CastlesOfWar.AI
{
    public class AIControllerBase : MonoBehaviour
    {
        public ushort TeamIndex;

        public virtual void OneStep()
        {
            
        }
    }
}