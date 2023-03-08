using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUIDragger : MonoBehaviour
{
    private Canvas _canvas;

    private Transform _inventoryUITransform, _canvasTransform;

    private Image _borderImage, _backgroundImage;

    private int _initialSiblingIndex;

    private bool _dragInitiated;

    public static event Action<int> DragStarted, DragEnded;

    private void Start()
    {
        _borderImage = GetComponent<Image>();
        _backgroundImage = transform.GetChild(0).GetComponent<Image>();
    }

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

        _borderImage.enabled = false;
        _backgroundImage.enabled = false;

        _initialSiblingIndex = transform.GetSiblingIndex();
        
        transform.SetParent(_canvasTransform);

        _dragInitiated = true;
    }

    public void Drop(BaseEventData data)
    {
        DragEnded?.Invoke(1);
        
        transform.SetParent(_inventoryUITransform);
        
        var pointerData = (PointerEventData) data;

        transform.SetSiblingIndex(GetSiblingPositionOnDrop(pointerData.position));

        _borderImage.enabled = RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, pointerData.position);

        _backgroundImage.enabled = true;

        _dragInitiated = false;
    }

    private int GetSiblingPositionOnDrop(Vector2 mousePos)
    {
        var parentRect = (RectTransform)transform.parent;
        var parentGrid = transform.parent.GetComponent<GridLayoutGroup>();

        var xPos = Mathf.FloorToInt(mousePos.x / (parentRect.rect.width / parentGrid.constraintCount));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            mousePos,
            _canvas.worldCamera,
            out var rectPos);

        var yPos = rectPos.y < 0 && parentRect.childCount > parentGrid.constraintCount ? 1 : 0;
        
        return RectTransformUtility.RectangleContainsScreenPoint(parentRect, mousePos) ? xPos + parentGrid.constraintCount * yPos : _initialSiblingIndex;
    }

    public void OnPointerEnter(BaseEventData _)
    {
        if(!_dragInitiated) _borderImage.enabled = true;
    }

    public void OnPointerExit(BaseEventData _)
    {
        if(!_dragInitiated) _borderImage.enabled = false;
    }

    public void Initialize(Transform canvasTransform)
    {
        var parent = transform.parent;
        
        _inventoryUITransform = parent;

        _canvasTransform = canvasTransform;

        _canvas = canvasTransform.GetComponent<Canvas>();
    }
}
