using UnityEngine;

namespace Vashta.CastlesOfWar.MapEntities
{
    public class BoxCaptureVisual : EntityCaptureVisual
    {
        public override void UpdateValues(short teamIndex, float capturePerc, float captureValue)
        {
            Color color = Color.gray;

            if (captureValue < 0)
            {
                color = Color.Lerp(Color.gray, Color.blue, capturePerc);                
            }
            else
            {
                color = Color.Lerp(Color.gray, Color.red, capturePerc);      
            }

            foreach (SpriteRenderer sprite in teamColoredSprites)
            {
                sprite.color = color;
            }
        }
    }
}