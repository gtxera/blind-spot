using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private GameEvent _deathEvent;

    [SerializeField] private float _fadeInTimeInSeconds;

    private Image _image;

    private GameEventListener _listener;
    
    void Start()
    {
        _image = GetComponent<Image>();
        
        _listener = new GameEventListener(_deathEvent, () =>
        {
            StartCoroutine(FadeIn());
        });
    }

    private IEnumerator FadeIn()
    {
        var currentTime = 0f;

        while (currentTime < _fadeInTimeInSeconds)
        {
            var increment = 1 / Time.deltaTime;
            currentTime += Time.deltaTime;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a + increment);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
