using System.Collections.Generic;
using UnityEngine;

public class SpawnRandom
{
    public static T GetChanceItem<T>(List<T> list) where T : ChanceItem
    {
        float _chanceSum = 0;
        for (int i = 0; i < list.Count; i++)
        {
            T current = list[i];
            _chanceSum += current.chance;

            //first item in the list
            if (i == 0)
            {
                current.minChance = 0;
                current.maxChance = current.chance;
            }
            else
            {
                current.minChance = list[i - 1].maxChance;
                current.maxChance = current.minChance + current.chance;
            }
        }

        float randomNum = Random.Range(0, _chanceSum);

        for (int i = 0; i < list.Count; i++)
        {
            T current = list[i];
            if (randomNum >= current.minChance && randomNum < current.maxChance)
            {
                return current;
            }
        }

        return null;
    }
}
