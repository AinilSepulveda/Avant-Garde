using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemAmount
{
    public InventoryEntry item;
    [Range (1, 999)]
    public int Amount;
   // public int IDitem;
    
}

[CreateAssetMenu(fileName = "RecipeItems", menuName = "Items/CreateRecipeItem", order = 0 )]
public class CraftingRecipe : ScriptableObject
{
    public ItemAmount[] materials;
    public ItemAmount[] Results;

    public bool CanCraft(CharacterInventory inventory)
    {
        foreach (KeyValuePair<int, InventoryEntry> ie in inventory.itemsInInventory)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                if (ie.Value.stackSize < materials[i].Amount)
                {
                    return false;
                }
            } 

        }

        return true;
    }

    public void Craft(CharacterInventory inventory)
    {
        if (CanCraft(inventory))
        {
            foreach (KeyValuePair<int, InventoryEntry> ie in inventory.itemsInInventory)
            {
                for (int i = 0; i < inventory.itemsInInventory.Count; i++)
                {
                    inventory.RemoveItems(materials[i].item);
                }
            }
            foreach (KeyValuePair<int, InventoryEntry> ie in inventory.itemsInInventory)
            {
                for (int i = 0; i < inventory.itemsInInventory.Count; i++)
                {
                    inventory.StoreItem(materials[i].item.invEntry);
                }
            }
        }
    }

}
