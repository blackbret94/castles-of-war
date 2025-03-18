using TMPro;
using Vashta.CastlesOfWar.Currency;

namespace Vashta.CastlesOfWar.UI
{
    public class PowerPanel : GamePanel
    {
        public TextMeshProUGUI Text;
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameManager.GetInstance();
            CurrencyController currencyController = _gameManager.TeamLeft.CurrencyController;
            currencyController.PowerChanged += OnPowerUpdated;
            Text.text = "Power: " + currencyController.Power.ToString();
        }
        
        private void OnPowerUpdated(object sender, int result)
        {
            Text.text = "Power: " + result.ToString();
        }
    }
}