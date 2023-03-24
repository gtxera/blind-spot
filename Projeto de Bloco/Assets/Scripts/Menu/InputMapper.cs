using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class InputMapper : MonoBehaviour
{
    [SerializeField] private InputBindings.PlayerActions _action;

    private TextMeshProUGUI _text;

    private void Start()
    {
        foreach (InputBindings.PlayerActions action in Enum.GetValues(typeof(InputBindings.PlayerActions)))
        {
            if(PlayerPrefs.HasKey(action.ToString())) InputBindings.ChangeInputBinding(action, (KeyCode)PlayerPrefs.GetInt(action.ToString()));
        }
        
        _text = GetComponentInChildren<TextMeshProUGUI>();

        InputBindings.BindingsChanged += actions =>
        {
            foreach (var action in actions)
            {
                if (action == _action)
                {
                    _text.text = InputBindings.Bindings[_action].ToString();
                }
            }
        };
    }

    public void Map()
    {
        StopAllCoroutines();
        StartCoroutine(MapRoutine());
    }

    private IEnumerator MapRoutine()
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        var keyCode = GetKeyPressed();
        
        if(InputBindings.ReservedKeys.Contains(keyCode)) yield break;

        InputBindings.ChangeInputBinding(_action, keyCode);
    }

    private KeyCode GetKeyPressed()
    {
        var length = Enum.GetNames(typeof(KeyCode)).Length;

        for (int i = 0; i < length; i++)
        {
            if (Input.GetKey((KeyCode)i)) return (KeyCode)i;
        }

        return KeyCode.None;
    }
}
