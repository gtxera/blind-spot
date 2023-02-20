using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatUtility
{
    public static float RoundWithDecimals(this float num, int decimals)
    {
        return MathF.Round(num * MathF.Pow(10, decimals)) / MathF.Pow(10, decimals);
    }
}
