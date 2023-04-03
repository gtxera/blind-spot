using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FMODUnity.StudioEventEmitter))]
public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private GameObject _itemUIPrefab;
    
    private readonly List<ItemUIPair> _itemUIPairs = new();

    private GameObject _placeholder;

    private RectTransform _rectTransform;

    private bool _inventoryOpen;

    private FMODUnity.StudioEventEmitter _emitter;
    

    private void Start()
    {
        PlayerInputs.Instance.InventoryKeyDown += ShowHideInventory;
        
        PlayerInventory.Instance.ItemNewAdded += AddNewItem;
        PlayerInventory.Instance.ItemCountChanged += ItemCountUpdate;
        PlayerInventory.Instance.ItemRemoved += RemoveItem;

        ItemUIDragger.DragStarted += CreatePlaceholder;
        ItemUIDragger.DragEnded += DestroyPlaceholder;

        DialogueManager.Instance.OnDialogueStarted += so =>
        {
            _rectTransform.anchoredPosition = new Vector2(0, _rectTransform.sizeDelta.y / 2);
            _inventoryOpen = false;
        };

        _rectTransform = GetComponent<RectTransform>();

        _emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void OnDisable()
    {
        PlayerInputs.Instance.InventoryKeyDown -= ShowHideInventory;
    }

    private void ShowHideInventory()
    {
        _inventoryOpen = !_inventoryOpen;
        
        _emitter.Play();
        _emitter.SetParameter("InventoryOpened", _inventoryOpen ? 1f : 0f);

        _rectTransform.anchoredPosition = new Vector2(0,
            _inventoryOpen ? - _rectTransform.sizeDelta.y / 2 : _rectTransform.sizeDelta.y / 2);
    }

    private void AddNewItem(ItemSO item, int amount)
    {
        var newItemUI = Instantiate(_itemUIPrefab, transform);

        var itemImg = newItemUI.transform.GetChild(1).GetComponent<Image>();
        print(item.ItemImage);
        itemImg.sprite = item.ItemImage;
        itemImg.color = Color.white;
        newItemUI.GetComponent<ItemUIDragger>().Initialize(transform.parent, item);

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
