using TMPro;
using Vashta.CastlesOfWar.Currency;

namespace Vashta.CastlesOfWar.UI
{
    public class GoldPanel : GamePanel
    {
        public TextMeshProUGUI Text;
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameManager.GetInstance();
            CurrencyController currencyController = _gameManager.TeamLeft.CurrencyController;
            currencyController.GoldChanged += OnGoldUpdated;
            Text.text = "Gold: " + currencyController.Gold.ToString();
        }
        
        private void OnGoldUpdated(object sender, int result)
        {
            Text.text = "Gold: " + result.ToString();
        }
    }
}