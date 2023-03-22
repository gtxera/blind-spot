using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartNewGame : MonoBehaviour
{
    [SerializeField] private TMP_InputField _input;
    
    public void StartGame()
    {
        if (_input.text == string.Empty) return;

        GameSaver.Instance.SetSaveId(_input.text);
        
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
