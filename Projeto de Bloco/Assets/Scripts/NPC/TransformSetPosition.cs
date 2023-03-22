using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSetPosition : MonoBehaviour
{
    [SerializeField] private Transform _positionTransform;
    
    public void SetPosition()
    {
        transform.position = _positionTransform.position;
        transform.rotation = _positionTransform.rotation;
    }
}
