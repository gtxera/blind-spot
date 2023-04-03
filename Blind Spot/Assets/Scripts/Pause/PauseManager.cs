using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : SingletonBehaviour<PauseManager>
{
    [SerializeField] private GameObject _inventoryCanvas, _questText;

    public TextMeshProUGUI QuestText => _questText.GetComponent<TextMeshProUGUI>();
    
    [HideInInspector]
    public bool IsPaused;

    private void Start()
    {
        PlayerInputs.Instance.PauseKeyDown += Pause;
        
        DontDestroyOnLoad(_inventoryCanvas);
    }

    private void Pause()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;

        IsPaused = !IsPaused;

        Time.timeScale = IsPaused ? 0 : 1;
        
        FMODUnity.RuntimeManager.PauseAllEvents(IsPaused);
        
        _inventoryCanvas.SetActive(!_inventoryCanvas.activeSelf);
        
    }
}
