using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireInterfaceAttribute : PropertyAttribute
{
    public Type requiredType { get; private set; }

    public RequireInterfaceAttribute(Type type)
    {
        requiredType = type;
    }
}
