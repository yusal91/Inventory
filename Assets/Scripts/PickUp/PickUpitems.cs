using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpitems : MonoBehaviour
{
    public ItemClass item;
    public int amountToGet;
    

    private void OnMouseDown()
    {
        Debug.Log("Picking up " + item.name);

        if(item.isStackable)
        {
            bool havePickup = Inventory.instance.Add(item, amountToGet);
            if (havePickup)
                Destroy(gameObject);
        }
        else
        {
            bool havePickup = Inventory.instance.Add(item, amountToGet);
            if (havePickup)
                Destroy(gameObject);
        }
    }
}
