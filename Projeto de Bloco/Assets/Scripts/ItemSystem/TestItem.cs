using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    private ItemEventValidator _validator;
    
    void Start()
    {
        _validator = GetComponent<ItemEventValidator>();
    }

    public void DestroyCube()
    {
        if (_validator.PlayerInRange && PlayerBulletCounter.Instance.CanFire)
        {
            PlayerBulletCounter.Instance.Fire();
            Destroy(gameObject);
        }
    }
}
