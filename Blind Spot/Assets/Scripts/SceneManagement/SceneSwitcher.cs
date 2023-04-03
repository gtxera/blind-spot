using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private int _sceneId;

    private void Start()
    {
        FadeOutManager.FadeOutFinished += LoadNextScene;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(_sceneId, LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        FadeOutManager.FadeOutFinished -= LoadNextScene;
    }
}
