using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingManager : MonoBehaviour
{
    //[Header("List Crafting Recipes")]
    //[SerializeField] private List <CraftingRecipeClass> craftingRecipes = new List<CraftingRecipeClass>();
    //[SerializeField] private RectTransform recipeSlotHolder;
    //[SerializeField] private RecipeSelection recipeSlotItem;
    //[SerializeField] private List <RecipeSelection> recipeSlot = new List<RecipeSelection>();


    //void Start()
    //{
    //    LoadCraftingRecipesFromResources();
    //    SettingRecipeSlot();
    //}
    //void SettingRecipeSlot()
    //{
    //    RecipeSelection recpSelect = Instantiate(recipeSlotItem, Vector3.zero, Quaternion.identity);
    //    recpSelect.transform.SetParent(recipeSlotHolder);
    //    recipeSlot.Add(recpSelect);
    //}
    //void LoadCraftingRecipesFromResources()    // should not load on start only when i press to turn the menu 
    //{
    //    craftingRecipes = Resources.LoadAll<CraftingRecipeClass>("Crafting").ToList();    // i can now get the recipes         
    //    Debug.Log(craftingRecipes);      
    //}

}
