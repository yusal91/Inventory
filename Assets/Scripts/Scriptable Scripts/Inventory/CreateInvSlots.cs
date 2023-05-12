using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateInvSlots : MonoBehaviour
{
    [SerializeField] private InvSlotInfo slot;
    [SerializeField] private RectTransform contentPanel;

    [SerializeField] List<InvSlotInfo> slotList = new List<InvSlotInfo>();

    public void CreateSlotsforInventory(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InvSlotInfo uiItem = Instantiate(slot, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            slotList.Add(uiItem);
        }
    }

    //public void SetUIData(ItemClass item)
    //{
    //    slot.SetData(item.icon);
    //    Debug.Log("SetUIData");
    //}

    //public void ResetUIData()
    //{
    //    slot.ResetData();
    //    Debug.Log("ResetUIData");
    //}

}
