using System;
using UnityEngine;
using Vashta.CastlesOfWar.AI;
using Vashta.CastlesOfWar.Unit;

namespace Vashta.CastlesOfWar.Input
{
    public class PlayerInputActions : MonoBehaviour
    {
        public GameManager GameManager { get; private set; }
        public Team PlayerTeam { get; private set; }
        public TeamCommander Commander { get; private set; }

        public short NumberOfUnitsToCommand = 3;
        
        private void Start()
        {
            GameManager = GameManager.GetInstance();
            PlayerTeam = GameManager.TeamLeft;
            Commander = PlayerTeam.Commander;
        }

        private void Update()
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                Quit();
        }

        public void CommandAdvance()
        {
            Commander.CommandAdvance(NumberOfUnitsToCommand);
        }

        public void CommandHold()
        {
            Commander.CommandHold(NumberOfUnitsToCommand);
        }

        public void CommandRetreat()
        {
            Commander.CommandRetreat(NumberOfUnitsToCommand);
        }
        
        public void SpawnUnit(UnitData unitData)
        {
            GameManager.SpawnUnit(unitData, 0);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}