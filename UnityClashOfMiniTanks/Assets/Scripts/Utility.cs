using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    private static int[] rotationValues = { 0, 90, 180, 360 };

    public static void RandomRotation(Transform transform)
    {
        transform.Rotate(0, rotationValues[Random.Range(0, rotationValues.Length - 1)], 0);
    }

    public static T[] Shuffle<T>(T[] shuffleArray, int shuffleCount)
    {
        if (shuffleArray.Length <= 0)
        {
            return null;
        }

        T[] arrayToShuffle = shuffleArray;

        for (int i = 0; i < shuffleCount; i++)
        {
            int randNumber1 = Random.Range(0, shuffleArray.Length);
            int randNumber2 = Random.Range(0, shuffleArray.Length);

            var temp = arrayToShuffle[randNumber1];
            arrayToShuffle[randNumber1] = arrayToShuffle[randNumber2];
            arrayToShuffle[randNumber2] = temp;
        }

        return arrayToShuffle;
    }

    public static string CreateRandomSuffix()
    {
        string randomSuffix = string.Empty;
        for (int i = 0; i <= 4; i++)
        {
            randomSuffix += Random.Range(0, 9).ToString();
        }

        return randomSuffix;
    }
}
