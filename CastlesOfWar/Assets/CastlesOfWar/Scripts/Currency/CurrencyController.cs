using System;
using System.Collections.Generic;
using UnityEngine;
using Vashta.CastlesOfWar.MapEntities;
using Vashta.CastlesOfWar.Simulation;
using Vashta.CastlesOfWar.Util;

namespace Vashta.CastlesOfWar.Currency
{
    public class CurrencyController : ISimulatedObject
    {
        public int Gold { get; private set; }
        private float _gold;
        public int Power { get; private set; }
        private float _power;
        public ushort TeamIndex { get; private set; }
        
        public event EventHandler<int> GoldChanged;
        public event EventHandler<int> PowerChanged;

        private GameManager _gameManager;
        private SimulationTimer _timer;
        
        public CurrencyController(ushort teamIndex, GameManager gameManager)
        {
            _gameManager = gameManager;
            TeamIndex = teamIndex;
            _timer = new SimulationTimer(gameManager, 1f);
        }
        
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
            if(_timer.Run())
                OneStepCurrencyCollection();
        }

        private void OneStepCurrencyCollection()
        {
            float goldEarned = 0;
            float powerEarned = 0;

            List<MapEntityBase> mapEntities = _gameManager.MapEntities;

            foreach (MapEntityBase mapEntity in mapEntities)
            {
                if (mapEntity.TeamIndex == TeamIndex)
                {
                    MapEntityData data = mapEntity.MapEntityData;
                    goldEarned += data.GoldPerSecond;
                    powerEarned += data.PowerPerSecond;
                }
            }
            
            ModifyGold(goldEarned);
            ModifyPower(powerEarned);
        }
    }
}