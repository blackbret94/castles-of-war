using UnityEngine.UI;

namespace Vashta.CastlesOfWar.UI
{
    public class Healthbar : GamePanel
    {
        public Slider HealthSlider;

        private ushort _maxHealth;
        private ushort _health;
        
        public void SetMaxHealth(ushort health)
        {
            _maxHealth = health;
            RefreshVisuals();
        }

        public void SetHealth(ushort health)
        {
            _health = health;
            RefreshVisuals();
        }

        public void RefreshVisuals()
        {
            HealthSlider.maxValue = _maxHealth;
            HealthSlider.value = _health;
        }
    }
}