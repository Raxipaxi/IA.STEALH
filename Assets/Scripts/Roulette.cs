using System.Collections.Generic;
using UnityEngine;

public class Roulette
{
    public T Run<T>(Dictionary<T, float> items)
    {
        float max = 0;
        foreach (var item in items)
        {
            max += item.Value;
        }
        //float random = Random.value * max;
        float random = Random.Range(0, max);

        foreach (var item in items)
        {
            random -= item.Value;
            if (random <= 0)
            {
                return item.Key;
            }
        }
        return default(T);
    }
}