using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutTransition : MonoBehaviour
{
    [SerializeField] private float _fadeOutDuration;

    [SerializeField] private Image _fadeOutImage;

    private IEnumerator Start()
    {
        while (_fadeOutImage.color.a < 1)
        {
            var normalizedAlphaStep = 1 / _fadeOutDuration * Time.deltaTime;
            
            _fadeOutImage.color += new Color(0,0,0,normalizedAlphaStep);

            yield return null;
        }
    }
}
