using System;
using UnityEngine;
using Vashta.CastlesOfWar.Simulation;

namespace Vashta.CastlesOfWar.Currency
{
    public class CurrencyController : ISimulatedObject
    {
        public int Gold { get; private set; }
        private float _gold;
        public int Power { get; private set; }
        private float _power;

        public float GoldPerSecond { get; private set; } = 5;
        public float PowerPerSecond { get; private set; }
        
        public event EventHandler<int> GoldChanged;
        public event EventHandler<int> PowerChanged;

        public void ModifyGold(float amount)
        {
            if (amount == 0)
                return;
            
            int oldGold = Gold;
            _gold += amount;
            Gold = Mathf.FloorToInt(_gold);

            if (oldGold != Gold)
            {
                GoldChanged?.Invoke(this, Gold);
            }
        }

        public void ModifyPower(float amount)
        {
            if (amount == 0)
                return;
            
            int oldPower = Power;
            _power += amount;
            Power = Mathf.FloorToInt(_power);

            if (oldPower != Power)
            {
                PowerChanged?.Invoke(this, Power);
            }
        }

        public void OneStep(float timestep)
        {
            ModifyGold(timestep * GoldPerSecond);
            ModifyPower(timestep * PowerPerSecond);
        }
    }
}