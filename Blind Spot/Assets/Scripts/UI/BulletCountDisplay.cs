using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletCountDisplay : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        
        PlayerBulletCounter.Instance.BulletCountChangedEvent +=
            () => _text.SetText(PlayerBulletCounter.Instance.BulletCount.ToString());
    }
}
