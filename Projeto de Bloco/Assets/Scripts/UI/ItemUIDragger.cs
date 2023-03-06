using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUIDragger : MonoBehaviour
{
    private Canvas _canvas;

    private Transform _inventoryUITransform, _canvasTransform;

    private InventoryUIController _inventoryUIController;

    public static event Action<int> DragStarted, DragEnded;

    public void Drag(BaseEventData data)
    {
        var pointerData = (PointerEventData) data;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)_canvas.transform,
            pointerData.position,
            _canvas.worldCamera, 
            out var position);

        transform.position = _canvas.transform.TransformPoint(position);
    }

    public void InitializeDrag(BaseEventData data)
    {
        DragStarted?.Invoke(transform.GetSiblingIndex());
        
        transform.SetParent(_canvasTransform);
    }

    public void Drop(BaseEventData data)
    {
        DragEnded?.Invoke(1);
        
        transform.SetParent(_inventoryUITransform);

        var maxDist = 0f;
        var closestSiblingIndex = 0;
        var toTheLeft = false;

        for (int i = 0; i < _inventoryUITransform.childCount; i++)
        {
            if(i == transform.GetSiblingIndex()) continue;
            
            var sibling = transform.parent.GetChild(i);
            var dist = Vector2.Distance(transform.position, sibling.position);

            if (dist > maxDist)
            {
                maxDist = dist;
                closestSiblingIndex = i;
                toTheLeft = transform.position.x < sibling.position.x;
            }
        }

        if (toTheLeft) transform.SetSiblingIndex(closestSiblingIndex-1);
        else transform.SetSiblingIndex(closestSiblingIndex);
    }

    public void Initialize(Transform canvasTransform)
    {
        var parent = transform.parent;
        
        _inventoryUITransform = parent;

        _canvasTransform = canvasTransform;

        _inventoryUIController = parent.GetComponent<InventoryUIController>();
        
        _canvas = canvasTransform.GetComponent<Canvas>();
    }
}
