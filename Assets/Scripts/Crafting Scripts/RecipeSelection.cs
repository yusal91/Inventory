using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSelection : MonoBehaviour, IPointerClickHandler
{
    public static RecipeSelection Instance;

    [SerializeField] private TMP_Text recipeTxt;
    [SerializeField] private Image slectedRecipeIcon; 
    
    //[SerializeField] public Button recipeButton;

    bool recipeSelected = false;

    public event Action<RecipeSelection> OnItemClicked, OnLeftMouseBtnClick;

    private void Awake()
    {
        Instance = this;
    }

    public void SetRecipeData(string recipeName)
    {
        this.recipeTxt.text = recipeName;
        //recipeButton.interactable = true;
        Debug.Log(recipeName);
    }
    public  void ResetRecipeData()
    {
        this.recipeTxt.text = "";             
    }

    public void SelectedRecipe()
    {
        slectedRecipeIcon.gameObject.SetActive(true);
        //recipeButton.interactable = true;
        recipeSelected = true;
    }

    public void DeSelectRecipe()
    {
        slectedRecipeIcon.gameObject.SetActive(false);
        //recipeButton.interactable = false;
        recipeSelected = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftMouseBtnClick?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }
}
