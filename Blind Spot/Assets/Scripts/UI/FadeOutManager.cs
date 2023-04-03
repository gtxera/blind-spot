using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutManager : MonoBehaviour
{
    [SerializeField] private float _fadeOutDuration;
    

    private Image _fadeOutImage;

    public static event Action FadeOutStarted, FadeOutFinished;

    private void Start()
    {
        _fadeOutImage = GetComponent<Image>();
        
    }

    public void StartFadeOut()
    {
        FadeOutStarted?.Invoke();
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        var currentDuration = 0f;
        var currentAlpha = Time.deltaTime / _fadeOutDuration;

        while (currentDuration < _fadeOutDuration)
        {
            _fadeOutImage.color = new Color(0, 0, 0, currentAlpha);

            currentDuration += Time.deltaTime;
            currentAlpha += Time.deltaTime / _fadeOutDuration;
            
            yield return new WaitForEndOfFrame();
        }
        
        FadeOutFinished?.Invoke();
    }
}
