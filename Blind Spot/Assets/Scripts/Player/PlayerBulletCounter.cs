using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletCounter : SingletonBehaviour<PlayerBulletCounter>
{
    private int _bulletCount = 10;
    public int BulletCount
    {
        get => _bulletCount;
        private set
        {
            _bulletCount = value;
            BulletCountChangedEvent?.Invoke(); 
        }
    }

    public bool CanFire => _bulletCount > 0;

    public Action BulletCountChangedEvent;
    
    public void Fire()
    {
        BulletCount--;
    }

    public void AddBullets(int amount)
    {
        BulletCount += amount;
    }
}
