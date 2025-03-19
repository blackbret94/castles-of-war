using UnityEngine;

namespace Vashta.CastlesOfWar.MapEntities
{
    public class FlagpostCaptureVisual : EntityCaptureVisual
    {
        public Transform RaisedPosition;
        public Transform LoweredPosition;
        public SpriteRenderer Flag;

        public override void UpdateValues(short teamIndex, float capturePerc, float captureValue)
        {
            base.UpdateValues(teamIndex, capturePerc, captureValue);
            
            Vector3 loweredPosition = LoweredPosition.position;
            Vector3 raisedPosition = RaisedPosition.position;

            Flag.transform.position = Vector3.Slerp(loweredPosition, raisedPosition, capturePerc);
        }
    }
}