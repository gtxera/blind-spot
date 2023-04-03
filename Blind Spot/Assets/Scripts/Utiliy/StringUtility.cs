using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class StringUtility
{
    public static bool HasAtLeastOneCharacter(this string str)
    {
        return str.Any(char.IsLetterOrDigit);
    }
}
