using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Misc Class", menuName = "Item/ Misc")]
public class MiscClass : ItemClass
{
    //[Header("Misc")]


    public override void Use(PlayerController caller) { }

    public override MiscClass GetMisc() { return this; }

    //--------------- No longer uses AbrastClass so not need this 

    //public override ItemClass GetItem() { return this; }
    //public override ToolClass GetTool() { return null; }
    //public override ConsumableClass GetConsumable() { return null; }
    //public override ArmorClass GetArmor() { return null; }
}
