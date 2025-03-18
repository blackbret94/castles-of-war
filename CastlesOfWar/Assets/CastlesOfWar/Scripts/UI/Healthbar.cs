using TMPro;
using UnityEngine.UI;

namespace Vashta.CastlesOfWar.UI
{
    public class Healthbar : GamePanel
    {
        public Slider HealthSlider;
        public TextMeshProUGUI Text;
        
        private ushort _maxHealth;
        private short _health;
        
        public void SetMaxHealth(ushort health)
        {
            _maxHealth = health;
            RefreshVisuals();
        }

        public void SetHealth(short health)
        {
            _health = health;
            RefreshVisuals();
        }

        public void SetText(string text)
        {
            Text.text = text;
        }

        public void RefreshVisuals()
        {
            HealthSlider.maxValue = _maxHealth;
            HealthSlider.value = _health;
        }
    }
}