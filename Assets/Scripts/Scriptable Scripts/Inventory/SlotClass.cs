using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotClass 
{
    [field: SerializeField] public ItemClass item { get; private set; } = null;

    [field: SerializeField] public int quantity { get; private set; } = 0;

    public SlotClass()
    {
        item = null;
        quantity= 0;        
    }

    public SlotClass(ItemClass item, int _quantity)
    {
        this.item = item;
        this.quantity = _quantity;
    }

    public SlotClass(SlotClass slot) 
    {
        this.item = slot.item;
        this.quantity = slot.quantity;
    }               

    public void AddQuantity(int _quantity) { quantity += _quantity; }            // stack items 

    public void SubQuantity(int _quantity)
    { 
        quantity -= _quantity;                            // remove stacked item
        if (quantity <= 0)
            Clear();
    }          

    public void AddItem(ItemClass item, int _quantity)
    {
        this.item = item;
        this.quantity = _quantity;
    }

    public void Clear() 
    { 
        this.item = null;
        this.quantity = 0;
    }


    //public ItemClass GetItem() { return item; }    // no longer need it since i made itemclass above a property

    //public int GetQuantity() { return quantity; }  // no longer need it since i made itemclass above a property
}
