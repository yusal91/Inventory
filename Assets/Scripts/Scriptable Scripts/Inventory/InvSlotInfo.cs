using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvSlotInfo : MonoBehaviour
{
    [SerializeField] private Image imageIcon;
    [SerializeField] private TextMeshProUGUI quantity;


    //private void Awake()
    //{
    //    ResetData();
    //}


    //public void SetData(Sprite icon, int itemQuanitity)
    //{
    //    this.imageIcon.enabled = true;
    //    this.imageIcon.sprite = icon;
    //    this.quantity.text = itemQuanitity.ToString();
    //    Debug.Log("RefreshUI");
    //}

    //public void ResetData()
    //{
    //    this.imageIcon.enabled = false;
    //    this.imageIcon.sprite = null;
    //    this.quantity.text = null;
    //    Debug.Log("ResetUI");
    //}
}
