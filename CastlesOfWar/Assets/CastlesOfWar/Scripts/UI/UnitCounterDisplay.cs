using TMPro;
using Vashta.CastlesOfWar.Util;

namespace Vashta.CastlesOfWar.UI
{
    public class UnitCounterDisplay : GamePanel
    {
        public ushort TeamIndex;
        public TextMeshProUGUI Text;
        
        private GameManager _gameManager;
        private Timer _refreshTimer;

        private void Start()
        {
            _gameManager = GameManager.GetInstance();
            _refreshTimer = new Timer(.25f);
        }

        private void Update()
        {
            if (_refreshTimer.Run())
            {
                if (TeamIndex == 0)
                {
                    Text.text = "Blue Units: " + _gameManager.TeamLeft.Units.Count;
                }
                else
                {
                    Text.text = "Red Units: " + _gameManager.TeamRight.Units.Count;
                }
            }
        }
    }
}