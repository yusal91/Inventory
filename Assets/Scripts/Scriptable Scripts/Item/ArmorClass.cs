using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Class", menuName = "Item/ Armor")]
public class ArmorClass : ItemClass
{
    [field: Header("Armor")]
    [field: SerializeField] public int defense { get; set; }

    public ArmorType armorType;
    public override ArmorClass GetArmor() { return this; }

    //--------------- No longer uses AbrastClass so not need this 

    //public override ItemClass GetItem() { return this; }
    //public override ToolClass GetTool() { return null; }
    //public override MiscClass GetMisc() { return null; }
    //public override ConsumableClass GetConsumable() { return null; }
}

public enum ArmorType
{
    platArmor,
    leather,
    cloth
}
