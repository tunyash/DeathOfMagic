using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DomMath
{
    public static bool IsChance(float chance)
    {
        return Random.Range(0, 1f) <= chance;
    }
}
