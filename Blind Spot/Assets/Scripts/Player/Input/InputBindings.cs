using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InputBindings
{
    public static event Action<PlayerActions[]> BindingsChanged;

    public static readonly KeyCode[] ReservedKeys = new[]
    {
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.Mouse2,
        KeyCode.Mouse3,
        KeyCode.Mouse4,
        KeyCode.Mouse5,
        KeyCode.Mouse6,
        KeyCode.W,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.UpArrow,
        KeyCode.LeftArrow,
        KeyCode.DownArrow,
        KeyCode.RightArrow
    };

    public enum PlayerActions
    {
        None,
        Interact,
        Run,
        CarBreaks,
        Radio,
        OpenInventory,
        Pause
    }

    public static readonly Dictionary<PlayerActions, KeyCode> Bindings = new()
    {
        { PlayerActions.Interact, KeyCode.E},
        { PlayerActions.Run, KeyCode.LeftShift},
        { PlayerActions.CarBreaks, KeyCode.Space},
        { PlayerActions.Radio, KeyCode.R},
        { PlayerActions.OpenInventory, KeyCode.I},
        { PlayerActions.Pause, KeyCode.Escape }
    };
    
    private static readonly Dictionary<PlayerActions, KeyCode> Default = new()
    {
        { PlayerActions.Interact, KeyCode.E},
        { PlayerActions.Run, KeyCode.LeftShift},
        { PlayerActions.CarBreaks, KeyCode.Space},
        { PlayerActions.Radio, KeyCode.R},
        { PlayerActions.OpenInventory, KeyCode.I},
        { PlayerActions.Pause, KeyCode.Escape }
    };

    public static void ChangeInputBinding(PlayerActions actionToChange, KeyCode newBinding)
    {
        var changedBindings = new List<PlayerActions>();
        
        if (Bindings.ContainsValue(newBinding))
        {
            var currentKey = Bindings.FirstOrDefault(pair => pair.Value == newBinding).Key;
            Bindings[currentKey] = Bindings[actionToChange];
            changedBindings.Add(currentKey);
        }
        
        Bindings[actionToChange] = newBinding;
        changedBindings.Add(actionToChange);
        
        PlayerPrefs.SetInt(actionToChange.ToString(), (int)newBinding);
        
        BindingsChanged?.Invoke(changedBindings.ToArray());
    }

    public static void ResetToDefault()
    {
        foreach (var key in Default.Keys)
        {
            Bindings[key] = Default[key];
            PlayerPrefs.SetInt(key.ToString(), (int)Default[key]);
        }
        
        BindingsChanged?.Invoke(Bindings.Keys.ToArray());
    }
}
