using UnityEngine;

// Code got from https://www.youtube.com/watch?v=WjE4G9lbHsM
public class ChanceItem
{
    public GameObject prefab;
    public float chance;

    [HideInInspector] public float minChance;
    [HideInInspector] public float maxChance;
}
