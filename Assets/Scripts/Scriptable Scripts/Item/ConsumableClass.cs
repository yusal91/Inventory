using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Consumable Class", menuName = "Item/ Consumable")]
public class ConsumableClass : ItemClass
{
    [field: Header("Consumable")]
    [field: SerializeField] public int healthRestored { get; private set; }


    public override void Use(PlayerController caller)
    {
        base.Use(caller);
        Debug.Log("Eat Consumable");
        Inventory.instance.UseSelected();
        //caller.inventory.Remove(this);        // this method is great for use, rather call the one below
    }

    public override ConsumableClass GetConsumable() { return this; }

    // this only if you are using itemClass as Abstract Class

    //public override ItemClass GetItem() { return this; }
    //public override ToolClass GetTool() { return null; }
    //public override MiscClass GetMisc() { return null; }
    //public override ArmorClass GetArmor() { return null; }
}
