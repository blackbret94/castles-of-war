using System.Collections.Generic;
using UnityEngine;

namespace Vashta.CastlesOfWar.MapEntities
{
    public class EntityCaptureVisual : MonoBehaviour
    {
        public List<SpriteRenderer> teamColoredSprites;

        public virtual void UpdateValues(short teamIndex, float capturePerc, float captureValue)
        {
            Color color = Color.gray;
            if(teamIndex == 0)
                color = Color.blue;
            else if (teamIndex == 1)
                color = Color.red;
            else if (teamIndex == -1)
                color = Color.gray;
            
            foreach (SpriteRenderer sprite in teamColoredSprites)
            {
                sprite.color = color;
            }
        }
    }
}