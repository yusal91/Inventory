using System;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More then one instance of Inventory found!");
            return;
        }

        instance = this;
    }
    #endregion

  
    [Header("List Crafting Recipes")]
    [SerializeField] private CraftingRecipeClass [] craftingRecipes;    
    [SerializeField] private GameObject craftingRecipeHolder;
    [SerializeField] private GameObject[] recipeSlot;

    [Header("List Input Item")]
    [SerializeField] private GameObject inputItemHolder;
    [SerializeField] private GameObject[] inputItemSlot; 

    [Header("List Output Item")]
    [SerializeField] private GameObject outputItemHolder;
    [SerializeField] private GameObject[] outputItemSlot;        

    [Header("Crafting Recipes Object")]
    [SerializeField] private GameObject craftingMenu;     

    [Header("inventory Drag and Drop object")]
    [SerializeField] private GameObject itemCursor;

    [Header("inventory Menu")]
    [SerializeField] private GameObject inventoryMenu; 
    [SerializeField] private GameObject hotbarSlotObj;

    [Header("inventory Slots")]
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private GameObject[] slots;
    

    [Header("HotBar Slots")]
    [SerializeField] private GameObject hotBarHolder; 
    [SerializeField] private GameObject[] hotbarSlots;

    [Header("Select Item")]
    [SerializeField] private GameObject hotbarSelector;
    [SerializeField] private int selectedSlotIndex = 0;
    public ItemClass selecteditem;

    [Header("Item List")]
    [SerializeField] private SlotClass [] items;
    [SerializeField] private SlotClass [] startingItems;

    [Header("Move Item")]
    [SerializeField] private SlotClass movingSlot;
    [SerializeField] private SlotClass tempSlot;
    [SerializeField] private SlotClass orignalSlot;
    bool isMovingItem;    

    [Header("Recipe Selection Index")]
    [SerializeField] private int currentSelectedRecipeIndex;
    [SerializeField] private CraftingRecipeClass selectedRecipe;

    public event Action<int> onRecipeSelected;    


    private void Start()
    {
        SlotsInfoOnStart();

        HotBarSetUp();

        RefreshUI();

        SettingUpRecipeSlot();
        LoadCraftingRecipesFromResources();
    }

    private void Update()
    {
        InventoryMenuKey();  // button to Set Active the Inventory
        DetectMouseButton(); // for Drag and Drop
        PressKeyToCraft();   // crafting button
        HotbarSelector();

        ActivateCraftingMenu();
    }

    #region HotBar Stuff
    void HotBarSetUp()
    {
        hotbarSlots = new GameObject[hotBarHolder.transform.childCount];

        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i] = hotBarHolder.transform.GetChild(i).gameObject;
        }
    }

    void HotbarSelector()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex + 1, 0, hotbarSlots.Length - 1); 
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex - 1, 0, hotbarSlots.Length - 1);
        }

        hotbarSelector.transform.position = hotbarSlots[selectedSlotIndex].transform.position;
        selecteditem = items[selectedSlotIndex + (hotbarSlots.Length * 1)].item;
    }

    void RefreshHotBar()
    {     

        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            try
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = 
                    items[i + (hotbarSlots.Length * 1)].item.icon;

                if (items[i + (hotbarSlots.Length * 1)].item.isStackable)         // if newItem no stackable then dont dispaly the nummber
                {
                    hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                        items[i + (hotbarSlots.Length * 1)].quantity.ToString();
                }
                else
                {
                    hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
            }
            catch
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    #endregion HotBar Stuff

    #region Moving Stuff    

    void DetectMouseButton()
    {
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if(isMovingItem)
            itemCursor.GetComponent<Image>().sprite = movingSlot.item.icon;

        if (Input.GetMouseButtonDown(0))
        {
            // Find the Closest Slot  that we Click on
            
            if(isMovingItem)
            {
                // end newItem move
                EndItemMove();
            }
            else
            {               
                BeginItemMove();
            }
        }
        else if(Input.GetMouseButtonDown(1))   //right click to split the stack
        {
            if (isMovingItem)
            {
                // end newItem move
                EndItemMove_Single();
            }
            else
            {
                BeginItemMove_Half();
            }
        }
    }

    private bool BeginItemMove_Half()   /// function to split the stack in half
    {
        orignalSlot = GetClosestSlot();
        if (orignalSlot == null || orignalSlot.item == null)
        {
            return false;    // there is no newItem to move!
        }
        movingSlot = new SlotClass(orignalSlot.item, Mathf.CeilToInt(orignalSlot.quantity / 2f));                   // this is the problem
        orignalSlot.SubQuantity(Mathf.CeilToInt(orignalSlot.quantity / 2f));

        if (orignalSlot.quantity == 0)
        {
            orignalSlot.Clear();
        }
        isMovingItem = true;
        RefreshUI();
        return true;
    }
    private bool EndItemMove_Single()
    {
        orignalSlot = GetClosestSlot();
        if (orignalSlot is null)
        {
            return false;             // there is no newItem to move!
        }

        if(orignalSlot.item is not null && (orignalSlot.item != movingSlot.item || 
           orignalSlot.quantity >= orignalSlot.item.stackSize))
        {
            return false;
        }

        movingSlot.SubQuantity(1);    
        
        if(orignalSlot.item != null && orignalSlot.item == movingSlot.item)
        {
            orignalSlot.AddQuantity(1);
        }
        else
        {
            orignalSlot.AddItem(movingSlot.item, 1);
        }        

        if(movingSlot.quantity < 1)
        {
            isMovingItem = false;
            movingSlot.Clear();
        }
        else
        {
            isMovingItem = true;
        }

        RefreshUI();
        return true;

    }

    private bool BeginItemMove()
    {
        orignalSlot = GetClosestSlot();
        if(orignalSlot == null || orignalSlot.item == null)
        {
            return false;    // there is no newItem to move!
        }
        movingSlot = new SlotClass(orignalSlot);                   // this is the problem
        orignalSlot.Clear();
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool EndItemMove()
    {
        orignalSlot = GetClosestSlot();
        if (orignalSlot == null)
        {
            Add(movingSlot.item, movingSlot.quantity);
            movingSlot.Clear();
        }
        else        
        {    
            if(orignalSlot.item != null)
            {
                if(orignalSlot.item == movingSlot.item && orignalSlot.item.isStackable 
                                       && orignalSlot.quantity < orignalSlot.item.stackSize)  // the are the same newItem  and should stack
                {
                   
                    var quantityCanAdd = orignalSlot.item.stackSize - orignalSlot.quantity;
                    var quantityToAdd = Mathf.Clamp(movingSlot.quantity, 0, quantityCanAdd);
                    var remainder = movingSlot.quantity - quantityToAdd;
                    Debug.Log(remainder);

                    orignalSlot.AddQuantity(quantityToAdd);

                    if (remainder == 0)
                    {
                        movingSlot.Clear();
                    }
                    else
                    {
                        movingSlot.SubQuantity(quantityCanAdd);
                        RefreshUI();
                        return false;
                    }
                                   
                }
                else
                {
                    tempSlot = new SlotClass(orignalSlot);             // this is the problem
                    orignalSlot.AddItem(movingSlot.item, movingSlot.quantity);    // swaps newItem with other slot
                    movingSlot.AddItem(tempSlot.item, tempSlot.quantity);        //  adds newItem to the moved slot 

                    RefreshUI();
                    return true;
                }
            }
            else
            {
                orignalSlot.AddItem(movingSlot.item, movingSlot.quantity);
                movingSlot.Clear();
            }
        }
        isMovingItem = false;
        RefreshUI();
        return true;
    }

    private SlotClass GetClosestSlot()
    {     
        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
            {
                return items[i];
            }
        }

        return null;
    }

    #endregion Moving Stuff

    #region Inventory utlis
    // this needs to run on Start
    void SlotsInfoOnStart()                          
    {
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];
        SettingUpInvSlots();

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }
        // startingitem Array
        for (int i = 0; i < startingItems.Length; i++)
        {
            //items[i] = startingItems[i];                           // need to double check
            Add(startingItems[i].item, startingItems[i].quantity);   // same here need to run some tests 
        }

    }

    void InventoryMenuKey()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            inventoryMenu.SetActive(!inventoryMenu.activeSelf);
            hotbarSlotObj.SetActive(!hotbarSlotObj.activeSelf);
            hotbarSelector.SetActive(!hotbarSelector.activeSelf);            
        }
    }

    void SettingUpInvSlots()
    {
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
           try
           {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].item.icon;

                if (items[i].item.isStackable)         // if newItem no stackable then dont dispaly the nummber
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].quantity.ToString();                
                }
                else
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
           }
           catch
           {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;                
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
           }
        }
        RefreshHotBar();
    }

    public bool Add(ItemClass newItem, int quantity)
    {
        // check if inventory contain the newItem
        SlotClass slot = contains(newItem);

        if(slot != null) 
        {
            //going to add newItem wth max size = quantity
            //there is already newItem = slot.quantity

            var quantityCanAdd = slot.item.stackSize - slot.quantity;  // if there is an newItem and you add newItem goes above maxstack
            var remainder = quantity - quantityCanAdd;                 // it should add the newItem here
            var quantityToAdd = Mathf.Clamp(quantity, 0, quantityCanAdd);

            slot.AddQuantity(quantityToAdd);

            if (remainder > 0)
            {
                Add(newItem, remainder);
            }
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].item == null)
                {
                    var quantityCanAdd = newItem.stackSize - items[i].quantity;  // if there is an newItem and you add newItem goes above maxstack
                    var remainder = quantity - quantityCanAdd;                   // it should add the newItem here
                    var quantityToAdd = Mathf.Clamp(quantity, 0, quantityCanAdd);

                    items[i].AddItem(newItem, quantityToAdd);

                    if (remainder > 0)
                    {
                        Add(newItem, remainder);
                    }

                    break;
                }
            }          
        }        
        
        RefreshUI();
        return true;             
    }

    public bool Remove(ItemClass item)
    {
        SlotClass slots = contains(item);
        if (slots != null)
        {
            if (slots.quantity >= 1)
            {
                slots.SubQuantity(1);
            }
            else
            {
                int slotToRemove = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].item == item)
                    {
                        slotToRemove = i;
                        break;
                    }
                }

                items[slotToRemove].Clear();
            }
        }
        else
        {
            return false;
        }
        
        RefreshUI();

        return true;
    }      

    public SlotClass contains(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item == item && items[i].item.isStackable && items[i].quantity < items[i].item.stackSize)                
            {
                return items[i];
            }    
        }
        return null;
    }

    public void UseSelected()
    {
        items[selectedSlotIndex + (hotbarSlots.Length * 1)].SubQuantity(1);
        RefreshUI();
    }

    public bool IsFull()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item == null)
            {
                return false;
            }           
        }       

        return true;
    }

    #endregion Inventory utlis

    #region Crafting methods

    void SettingUpRecipeSlot()           // We First set the Recipe UI Slots 
    {
        recipeSlot = new GameObject[craftingRecipeHolder.transform.childCount];

        for (int i = 0; i < craftingRecipeHolder.transform.childCount; i++)
        {            
            recipeSlot[i] = craftingRecipeHolder.transform.GetChild(i).gameObject;            
        }
    }

    void LoadCraftingRecipesFromResources()    // should not load on start only when i press to turn the menu 
    {
        craftingRecipes = Resources.LoadAll<CraftingRecipeClass>("Crafting");    // i can now get the recipes 
        //i = Resources.LoadAll<CraftingRecipeClass>("Crafting").ToList();    // i can now get the recipes 
        Debug.Log(craftingRecipes);
        AddRecipeInformationToRecipeSlot();
    }

    public void AddRecipeInformationToRecipeSlot()   // we add recipe UI Information to the list 
    {
        foreach (CraftingRecipeClass recipe in craftingRecipes)
        {
            
            RecipeRefreshUI();
            Debug.Log(recipe);

            // was making so the when the crafting window is active it should select first item
            //for (int i = 0; i < recipeSlot.Length; i++)       
            //{
            //    if (recipeSlot.Contains(recipeSlot[i]))
            //    {
            //        if(recipeSlot == null)
            //        {
            //            continue;
            //        }
            //        if(selectedRecipe != null)
            //        {
            //           selectedRecipe = craftingRecipes[currentSelectedRecipeIndex];
            //            InputItemSlotRefreshUI();                     
            //            RecipeSelection.Instance.SelectedRecipe();
            //        }                   
            //    }
            //    RecipeSelection.Instance.OnLeftMouseBtnClick += OnClickEvent;
            //}
        }
    }

    public void RecipeRefreshUI()
    {
        int loopLimit = Mathf.Min(recipeSlot.Length, craftingRecipes.Length);

        for (int i = 0; i < loopLimit; i++)
        {
            Debug.Log(loopLimit);
            recipeSlot[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = craftingRecipes[i].recipeName;            

        }

        for (int i = loopLimit; i < recipeSlot.Length; i++)
        {            
            //RecipeSelection.Instance.OnLeftMouseBtnClick -= OnClickEvent;
            recipeSlot[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    private void OnClickEvent(RecipeSelection selection)
    {
        for (int i = 0; i < recipeSlot.Length; i++)
        {
            if (currentSelectedRecipeIndex != craftingRecipes.Length)
            {
                selectedRecipe = craftingRecipes[currentSelectedRecipeIndex];                
                RecipeSelection.Instance.SelectedRecipe();
                
                Debug.Log(currentSelectedRecipeIndex);
                // so far it works now how do i fix how to select other recipes

                if(craftingRecipes[i].inputItems.Count() < i )
                {
                    InputItemSlotRefreshUI();
                }

                //for (int inputItem = 0; inputItem < craftingRecipes[i].inputItems.Length; inputItem++)
                //{
                //    InputItemSlotRefreshUI();
                //}
            }            

            onRecipeSelected?.Invoke(i);
        } 
    }

    void InputItemSlotRefreshUI()
    {
        for (int i = 0; i < inputItemSlot.Length; i++)
        {
            try
            {
                inputItemSlot[i].transform.GetComponent<Image>().enabled = true;
                inputItemSlot[i].transform.GetComponent<Image>().sprite = craftingRecipes[i].inputItems[i].item.icon;
                Debug.Log(craftingRecipes[i].inputItems[i].item.name);

                if (craftingRecipes[i].inputItems.Length > 0 && craftingRecipes[i].inputItems[i].item.isStackable)
                {
                    inputItemSlot[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                       craftingRecipes[i].inputItems[i].quantity.ToString();
                    Debug.Log(craftingRecipes[i].inputItems[i].quantity.ToString());
                }
            }
            catch
            {
                inputItemSlot[i].transform.GetComponent<Image>().sprite = null;
                inputItemSlot[i].transform.GetComponent<Image>().enabled = false;
                inputItemSlot[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }  
    
    void SettingInputItemSlot()
    {
        inputItemSlot = new GameObject[inputItemHolder.transform.childCount];

        for (int i = 0; i < inputItemHolder.transform.childCount; i++)
        {
            inputItemSlot[i] = inputItemHolder.transform.GetChild(i).gameObject;          
        }
    }   

    void ActivateCraftingMenu()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            craftingMenu.SetActive(!craftingMenu.activeSelf);
            RecipeSelection.Instance.OnLeftMouseBtnClick += OnClickEvent;
            SettingInputItemSlot();            
        }
    } 

    void PressKeyToCraft()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            StartCrafting(craftingRecipes[0]);
        }
    }

    void StartCrafting(CraftingRecipeClass recipe)
    {
        if(recipe.CanCraft(this))
        {
            recipe.Craft(this);
        }
        else
        {
            // show Error message
            Debug.Log("Not Enough items");
        }
    }

    public bool contains(ItemClass item, int quantity)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item == item && items[i].quantity >= quantity)
            {
                return true;
            }
        }
        return false;
    }

    public bool Remove(ItemClass item, int quantity)
    {
        SlotClass slots = contains(item);
        if (slots != null)
        {
            if (slots.quantity >= 1)
            {
                slots.SubQuantity(quantity);
            }
            else
            {
                int slotToRemove = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].item == item)
                    {
                        slotToRemove = i;
                        break;
                    }
                }

                items[slotToRemove].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshUI();

        return true;
    }


    #endregion Crafting Methods
}
