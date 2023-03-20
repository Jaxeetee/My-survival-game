using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Reference(YT) https://www.youtube.com/watch?v=84rs2Q0z9ak&t=179s
// reference(Github) https://gist.github.com/FadrikAlexander/ab4ef778051ad0b1e634db407c1eb345
namespace Probabilty{
    public class Chance
    {
        #region Percentage Probabilities
        private static List<float>_accumulatedChancesByPercentage;

        public static bool DidGetItemByChance(float chanceValue)
        {
            float randomValue = Random.Range(1,101);
            return randomValue < chanceValue;
        }


        private static bool AddAccumulatedChances(List<float> chances, float maxValue = 100f)
        {
            float SumOfAllChances = 0;

            _accumulatedChancesByPercentage = new List<float>();

            foreach (float chance in chances)
            {
                SumOfAllChances += chance;
                _accumulatedChancesByPercentage.Add(SumOfAllChances);

                if (SumOfAllChances > maxValue)
                {
                    Debug.LogError($"Probabilities Exceeded by max value {maxValue}");
                    return false;
                }

            }

            //returns true if SumOfAllChances did not exceed the maxvalue;
            return true;
        }

        public static int GetIndexItemByChance(List<float> listOfChances, float maxValue = 100f)
        {
            //check first if sum of all chances in this list exceeds max value
            if (!AddAccumulatedChances(listOfChances, maxValue)) return -1;

            float randomValue = Random.Range(1, maxValue + 1);
            for (int i = 0; i < listOfChances.Count; i++)
            {
                if (randomValue <= _accumulatedChancesByPercentage[i])
                {
                    return i;
                }
            }

            return -1;
        }
        
        #endregion

        #region Non-Percentage Probabilities
        private static List<float> _accumulatedChancesByRarity;

        private static float GetProbabilityRarityModifier(List<float> rarities)
        {
            float sumOfItemRarities = 0;

            foreach(float rarity in rarities)
            {
                sumOfItemRarities += rarity;
            }

            return 100/sumOfItemRarities;
        }

        private static void AddAccumulatedRarities(List<float> rarities)
        {
            float sumOfAllRarities = 0;

            _accumulatedChancesByRarity = new List<float>(); 

            foreach(float rarity in rarities)
            {
                sumOfAllRarities += rarity;
                _accumulatedChancesByRarity.Add(sumOfAllRarities);
            }
        }

        /// <summary>
        /// use this for non-percentage list of probabilities. Cannot change max value as it is maxed to 100
        /// </summary>
        /// <returns></returns>
        public static int GetIndexItemByProbabilityRarity(List<float> rarities)
        {
            AddAccumulatedRarities(rarities);

            float randomValue = Random.Range(1,101);

            for (int i = 0; i < rarities.Count; i++)
            {
                if (randomValue <= _accumulatedChancesByRarity[i])
                {
                    return i;
                }
            }

            return -1;
        }
        
        #endregion

        #region Getting Item Based in Rarity in a Sample Size

        public static bool OneInProbability(int inSampleSize)
        {
            float randomValue = Random.Range(1, inSampleSize + 1);
            return randomValue <= 1;
        }

        public static bool ChanceInSampleSize(int inSampleSize, int chance = 1)
        {
            float randomValue = Random.Range(1, inSampleSize + 1);
            return randomValue <= chance;
        }
        #endregion


    }
}

