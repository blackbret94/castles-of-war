namespace Vashta.CastlesOfWar.MapEntities
{
    public class OutpostBase : MapEntityBase
    {
        protected override void OnNeutralized()
        {
            base.OnNeutralized();
            
            // Warn player!!
        }
        
        protected override void OnCapture()
        {
            base.OnCapture();
            
            if (TeamIndex != StartingTeamIndex)
            {
                // Game over!
            }
        }
    }
}