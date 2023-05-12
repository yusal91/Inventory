using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting Recipe", menuName = "Crafting/ Recipe")]
public class CraftingRecipeClass : ScriptableObject
{
    [Header("Crafting Name")]
    public string recipeName;
    
    public int ID => GetInstanceID();


    [Header("Crafting")]
    public SlotClass[] inputItems; 
    public SlotClass outputItems; 

    public bool CanCraft(Inventory inventory)
    {   
        // check if we actually have space in our inventory to craft
        if(inventory.IsFull()) 
            return false;

        for(int i = 0; i < inputItems.Length; i ++)
        {
            Debug.Log("Counting input item :" + inputItems.Length);
            if (!inventory.contains(inputItems[i].item, inputItems[i].quantity))
            {
                Debug.Log("Cant Craft");
                return false;
            }
        }

        Debug.Log("Can Craft");
        // retrun if the inventory has the required items
        return true;
    }

    public void Craft(Inventory inventory)
    {
        // remove the input items from the inventory
        for (int i = 0; i < inputItems.Length; i++)
        {
            inventory.Remove(inputItems[i].item, inputItems[i].quantity);
        }
        // Ad the out item to the inventory
        inventory.Add(outputItems.item, outputItems.quantity);
    }
}
