using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject buyButton;
    public TextMeshProUGUI itemName, itemPrice;
    public TextMeshProUGUI livesCount, hintsCount, autoCompleteCount;
    public Animation notEnough;

    public Transform items;
    private List<GameObject> gameItems;
    private int itemIndex = 0;
    private ItemManager itemManager;
    private NodeController nodeController;

    void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        nodeController = FindObjectOfType<NodeController>();
        LoadItems();
        UpdateUI();
    }

    public void Previous()
    {
        if (itemIndex > 0)
        {
            itemIndex--;
            LoadItems();
        }
    }

    public void Next()
    {
        if (itemIndex < gameItems.Count - 1)
        {
            itemIndex++;
            LoadItems();
        }
    }

    public void Buy()
    {
        Item item = gameItems[itemIndex].GetComponent<Item>();
        if (Wallet.GetAmount() >= item.price)
        {
            Wallet.SetAmount(Wallet.GetAmount() - item.price);
            UpdateItemCount(item);
        }
        else
        {
            notEnough.Play("Not-Enough-In");
        }
    }

private void LoadItems()
{
    // Initialize gameItems list
    gameItems = new List<GameObject>();

    // Add all children of 'Item Background' to the gameItems list
    foreach(Transform item in items)
    {
        gameItems.Add(item.gameObject);
    }

    // Update the displayed item
    UpdateDisplayedItem();
}

private void UpdateDisplayedItem()
{
    // Loop through all items and only display the current one
    for(int i = 0; i < gameItems.Count; i++)
    {
        gameItems[i].SetActive(i == itemIndex);
    }

    // Get the item component of the current item
    Item currentItem = gameItems[itemIndex].GetComponent<Item>();

    // Update the UI with the item's details
    itemName.text = gameItems[itemIndex].name;
    itemPrice.text = currentItem.price.ToString();
}


    private void UpdateItemCount(Item item)
    {
        switch (item.gameObject.name)
        {
            case "Life":
                itemManager.AddItem("Life", 1);
                break;
            case "Hint":
                itemManager.AddItem("Hint", 1);
                break;
            case "AutoComplete":
                itemManager.AddItem("AutoComplete", 1);
                break;
            default:
                Debug.LogError("Unknown item name");
                break;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (nodeController != null) {
            nodeController.UpdateLives();
            nodeController.UpdateHintCountUI();
            nodeController.UpdateCompleteCountUI();
        }
    }
}

