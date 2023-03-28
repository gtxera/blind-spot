using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseWindowController : MonoBehaviour
{
    [SerializeField] private GameObject _window;

    public void OpenCloseWindow()
    {
        _window.SetActive(!_window.activeSelf);
    }
}
