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

    private bool _inventoryOpen;
    

    private void Start()
    {
        PlayerInputs.Instance.InventoryKeyDown += ShowHideInventory;
        
        PlayerInventory.Instance.ItemNewAdded += AddNewItem;
        PlayerInventory.Instance.ItemCountChanged += ItemCountUpdate;
        PlayerInventory.Instance.ItemRemoved += RemoveItem;

        ItemUIDragger.DragStarted += CreatePlaceholder;
        ItemUIDragger.DragEnded += DestroyPlaceholder;

        _rectTransform = GetComponent<RectTransform>();
    }

    private void ShowHideInventory()
    {
        _inventoryOpen = !_inventoryOpen;

        _rectTransform.anchoredPosition = new Vector2(0,
            _inventoryOpen ? - _rectTransform.sizeDelta.y / 2 : _rectTransform.sizeDelta.y / 2);
    }

    private void AddNewItem(ItemSO item, int amount)
    {
        var newItemUI = Instantiate(_itemUIPrefab, transform);

        var itemImg = newItemUI.transform.GetChild(1).GetComponent<Image>();
        itemImg.sprite = item.ItemImage;
        itemImg.color = Color.white;
        newItemUI.GetComponent<ItemUIDragger>().Initialize(transform.parent);

        switch (item.Stackable)
        {
            case true:
                newItemUI.GetComponentInChildren<TextMeshProUGUI>().text = amount.ToString();
                break;
            
            case false:
                newItemUI.transform.GetChild(2).gameObject.SetActive(false);
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

    private struct ItemUIPair
    {
        public ItemSO Item;
        
        public GameObject UI;
    }
}
