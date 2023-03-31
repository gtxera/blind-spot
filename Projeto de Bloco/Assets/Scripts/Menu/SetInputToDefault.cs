using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInputToDefault : MonoBehaviour
{
    public void SetDefault()
    {
        InputBindings.ResetToDefault();
    }
}
