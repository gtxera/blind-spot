using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputBindings
{
    public enum PlayerActions
    {
        Interact,
        Run,
        CarBreaks,
        Radio
    }

    public static Dictionary<PlayerActions, KeyCode> Bindings = new()
    {
        { PlayerActions.Interact, KeyCode.E},
        { PlayerActions.Run, KeyCode.LeftShift},
        { PlayerActions.CarBreaks, KeyCode.Space},
        { PlayerActions.Radio, KeyCode.R}
    };

    public static void ChangeInputBinding(PlayerActions actionToChange, KeyCode newBinding)
    {
        Bindings[actionToChange] = newBinding;
    }
}
