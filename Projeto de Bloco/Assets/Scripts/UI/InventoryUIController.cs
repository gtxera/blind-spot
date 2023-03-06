using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private GameObject _itemUIPrefab;
    
    private readonly List<ItemUIPair> _itemUIPairs = new();

    private GameObject _placeholder;

    private RectTransform _rectTransform;

    private bool _mouseInInventoryPosition;

    private CancellationTokenSource _dialogueSlideInTokenSource, _dialogueSlideOutTokenSource;
    private CancellationToken _dialogueSlideInToken, _dialogueSlideOutToken;

    private const float SLIDE_SPEED = 0.004f;

    private void Start()
    {
        PlayerInventory.Instance.ItemNewAdded += AddNewItem;
        PlayerInventory.Instance.ItemCountChanged += ItemCountUpdate;
        PlayerInventory.Instance.ItemRemoved += RemoveItem;

        ItemUIDragger.DragStarted += CreatePlaceholder;
        ItemUIDragger.DragEnded += DestroyPlaceholder;

        _rectTransform = GetComponent<RectTransform>();
    }

    private void AddNewItem(ItemSO item, int amount)
    {
        var newItemUI = Instantiate(_itemUIPrefab, transform);

        newItemUI.GetComponent<Image>().sprite = item.ItemImage;
        newItemUI.GetComponent<Image>().color = Color.white;
        newItemUI.GetComponent<ItemUIDragger>().Initialize(transform.parent);

        switch (item.Stackable)
        {
            case true:
                newItemUI.GetComponentInChildren<TextMeshProUGUI>().text = amount.ToString();
                break;
            
            case false:
                newItemUI.transform.GetChild(0).gameObject.SetActive(false);
                break;
        }

        _itemUIPairs.Add(new ItemUIPair
        {
            Item = item,
            UI = newItemUI
        });
    }

    private void ItemCountUpdate(ItemSO item, int amount)
    {
        _itemUIPairs.First(pair => pair.Item == item).UI.GetComponentInChildren<TextMeshProUGUI>().text =
            amount.ToString();
    }

    private void RemoveItem(ItemSO item)
    {
        var pair = _itemUIPairs.First(pair => pair.Item == item);
        
        Destroy(pair.UI);

        _itemUIPairs.Remove(pair);
    }

    private void CreatePlaceholder(int siblingIndex)
    {
        _placeholder = Instantiate(_itemUIPrefab, transform);
        _placeholder.transform.SetSiblingIndex(siblingIndex);
    }

    private void DestroyPlaceholder(int _)
    {
        Destroy(_placeholder);
    }

    private void Update()
    {
        var mouseNormalizedPos = Input.mousePosition / new Vector2(Screen.width, Screen.height);

        var itemUIScreenProportion = (Screen.height - _rectTransform.sizeDelta.y) / Screen.height;

        if (mouseNormalizedPos.y >= itemUIScreenProportion && !_mouseInInventoryPosition)
        {
            _dialogueSlideInTokenSource = new CancellationTokenSource();
            _dialogueSlideInToken = _dialogueSlideInTokenSource.Token;

            _dialogueSlideOutTokenSource?.Cancel();
            
#pragma warning disable CS4014
            DialogueUISlideIn(_dialogueSlideInToken);
#pragma warning restore CS4014
            
            _mouseInInventoryPosition = true;
        }
        else if(mouseNormalizedPos.y < itemUIScreenProportion && _mouseInInventoryPosition)
        {
            _dialogueSlideOutTokenSource = new CancellationTokenSource();
            _dialogueSlideOutToken = _dialogueSlideOutTokenSource.Token;
            
            _dialogueSlideInTokenSource?.Cancel();

#pragma warning disable CS4014
            DialogueUISlideOut(_dialogueSlideOutToken);
#pragma warning restore CS4014
            
            _mouseInInventoryPosition = false;
        }
    }

    private async Task DialogueUISlideIn(CancellationToken token)
    {
        while (_rectTransform.anchoredPosition.y > -_rectTransform.sizeDelta.y / 2)
        {
            _rectTransform.anchoredPosition -= new Vector2(0, _rectTransform.sizeDelta.y / 2 * SLIDE_SPEED);
            await Task.Delay(Convert.ToInt32(Time.deltaTime * 1000), token);
        }

        _rectTransform.anchoredPosition = new Vector2(0, -_rectTransform.sizeDelta.y / 2);
    }

    private async Task DialogueUISlideOut(CancellationToken token)
    {
        while (_rectTransform.anchoredPosition.y < _rectTransform.sizeDelta.y / 2)
        {
            _rectTransform.anchoredPosition += new Vector2(0, _rectTransform.sizeDelta.y / 2 * SLIDE_SPEED);
            await Task.Delay(Convert.ToInt32(Time.deltaTime * 1000), token);
        }

        _rectTransform.anchoredPosition = new Vector2(0, _rectTransform.sizeDelta.y / 2);
    }

    private struct ItemUIPair
    {
        public ItemSO Item;
        
        public GameObject UI;
    }

    private void OnDisable()
    {
        _dialogueSlideInTokenSource?.Cancel();
        _dialogueSlideOutTokenSource?.Cancel();
    }
}
