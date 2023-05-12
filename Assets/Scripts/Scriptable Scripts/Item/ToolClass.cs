using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New ToolClass", menuName = "Item/ Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool")]
    public WeaponType weaponType;

    public override void Use(PlayerController caller)
    {
        base.Use(caller);
        Debug.Log("Swing");
    }

    public override ToolClass GetTool() { return this;}

    //--------------- No longer uses AbrastClass so not need this 

    //public override ItemClass GetItem() {return this;}
    //public override MiscClass GetMisc() {return null;}
    //public override ConsumableClass GetConsumable() {return null;}
    //public override ArmorClass GetArmor() { return null; }
}

public enum WeaponType
{
    sword,
    staff,
    hammer,
    axe,
    dagger
}
