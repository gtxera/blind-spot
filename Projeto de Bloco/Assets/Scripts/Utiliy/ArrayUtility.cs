using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayUtility
{
    public static void Move<T>(this IList<T> list, int source, int destination)
    {
        var sourceCopy = list[source];
        
        if (destination < source)
        {
            var lastCopy = list[destination];
            
            for (int i = destination + 1; i <= source; i++)
            {
                var currentCopy = list[i];
                list[i] = lastCopy;
                lastCopy = currentCopy;
            }
        }

        else
        {
            var lastCopy = list[destination];
            
            for (int i = destination - 1; i >= source; i--)
            {
                var currentCopy = list[i];
                list[i] = lastCopy;
                lastCopy = currentCopy;
            }
        }

        list[destination] = sourceCopy;
    }
}
