using TMPro;
using UnityEngine;
using Vashta.CastlesOfWar.Unit;

namespace Vashta.CastlesOfWar.UI
{
    public class SpawnButton : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public UnitData UnitData;
        private GameManager _gameManager;

        private void Awake()
        {
            Text.text = UnitData.DisplayName + " (" + UnitData.GoldCost + ")";
        }

        private void Start()
        {
            _gameManager = GameManager.GetInstance();
        }

        public void OnClick()
        {
            if (!UnitData)
            {
                Debug.Log("Spawn Button is missing UnitData!");
                return;
            }
            
            _gameManager.SpawnUnit(UnitData, 0);   
        }
    }
}