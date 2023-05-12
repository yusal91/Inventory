using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [Header("Item Icon and Quantity")]
    [SerializeField] private Image itemImage;
    [SerializeField] private Image borderImage;
    [SerializeField] private TMP_Text quantityTxt;


    public event Action<InventorySlotUI> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag,
                                         OnItemEndDrag, OnRightMouseBtnClick;

    private bool empty = true;


    private void Awake()
    {
        ResetData();
        Deselect();
    }     

    public void SetData(Sprite sprite, int quantity)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityTxt.text = quantity + "";
        empty = false;
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }

    public void Select()
    {
        borderImage.enabled = true;
        Debug.Log("Item Selected");
    }

    public void Deselect()
    {
        borderImage.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClick?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (empty)
        {
            return;
        }
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging an Item");
    }
}
