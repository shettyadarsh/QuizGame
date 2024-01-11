using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    public static void Shuffle<T>(this List<T> listToShuffle, float shuffleIntensity = 1f)
    {
        var listLength = listToShuffle.Count;
        for (int i = 0; i < listLength * shuffleIntensity; i++)
        {
            var index1 = Random.Range(0, listLength);
            var index2 = Random.Range(0, listLength);
            (listToShuffle[index1], listToShuffle[index2]) = (listToShuffle[index2], listToShuffle[index1]);
        }
    }
}
