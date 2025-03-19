using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vashta.CastlesOfWar.ScriptableObject;

namespace Vashta.CastlesOfWar.Unit
{
    [CreateAssetMenu(fileName = "Unit Dictionary", menuName = "CoW/Unit Dictionary", order = 1)]
    public class UnitDataDictionary : ScriptableDictionary<UnitData>
    {
        public UnitData GetRandom()
        {
            List<UnitData> list = Dictionary.Values.ToList();
            return list[Random.Range(0, list.Count)];
        }

        public UnitData GetRandom(short maxGoldCost)
        {
            System.Random rng = new System.Random();
            List<UnitData> list = Dictionary.Values.ToList();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]); // Swap elements
            }

            foreach (UnitData unitData in list)
            {
                if(unitData.GoldCost < maxGoldCost)
                    return unitData;
            }

            return list[0];
        }
    }
}