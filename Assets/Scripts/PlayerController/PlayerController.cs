using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(Inventory.instance.selecteditem != null)
            {
                Inventory.instance.selecteditem.Use(this);
            }
        }
    }
}
