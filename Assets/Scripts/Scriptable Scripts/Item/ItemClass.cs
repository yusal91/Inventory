using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass : ScriptableObject
{
    [field: Header("Item")]
    [field: SerializeField] public string ToolName { get; private set; }
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public bool isStackable { get; private set; }  // recomanded not to have = true at begining but it causes isuses
    [field: SerializeField] public int stackSize { get; private set; }    // again dont set the size from the script

    //------------ Changing them into virtual in order to use them since this is nolonger Abstract class---------------------

    public virtual ItemClass GetItem() { return this; }
    public virtual ToolClass GetTool() { return null; }
    public virtual MiscClass GetMisc() { return null; }
    public virtual ConsumableClass GetConsumable() { return null; }
    public virtual ArmorClass GetArmor() { return null; }

    // might not need it for Bossbreader

    public virtual void Use(PlayerController caller) 
    {
        Debug.Log("Use the item");
    }

}
