using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : Singleton<Inventory>
{
    public CharacterStats Stats;
    public TMPro.TextMeshProUGUI textItemEntry;

    public int inventoryItemCap = 20;
    public Image[] inventoryDisplaySlots;
    public Dictionary<int,InventoryEntry> itemsInInventory = new Dictionary<int,InventoryEntry>();
    //UI
    public GameObject inventoryUI;
    public GameObject stastHUD;

    private InventoryEntry itemEntry;
    // Start is called before the first frame update
    void Start()
    {
        inventoryDisplaySlots = new Image[28];
        itemEntry = new InventoryEntry(0, null, null);
        itemsInInventory.Clear();


        inventoryDisplaySlots = GetComponentsInChildren<Image>();
        Stats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            DisplayInventory();
            // Debug.Log("abriendo el inventario");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            DisplayStast();
            // Debug.Log("abriendo el inventario");
        }
    }
    public void StoreItem(ItemPickUp itemToStore) //para tomar las cosas que podrias tomar
    {
        itemEntry.invEntry = itemToStore;
        itemEntry.stackSize = 1;
        itemEntry.hbSprite = itemToStore.itemDefinition.ItemIcon;
        textItemEntry.gameObject.SetActive(true);
        textItemEntry.text = "Has recogido: " + itemEntry.invEntry.itemDefinition.name;
        itemToStore.gameObject.SetActive(false);
        TryPickUP();
    }
    public void TryPickUP()
    {

    }
    #region Display UI
    void DisplayInventory()
    {
        if (inventoryUI.activeSelf)
        {
            inventoryUI.SetActive(false);
            textItemEntry.gameObject.SetActive(false);
        }
        else
        {
            inventoryUI.SetActive(true);
        }
    }
    void DisplayStast()
    {
        if (stastHUD.activeSelf == true)
        {
            stastHUD.SetActive(false);

        }
        else
        {
            stastHUD.SetActive(true);
        }
    }
    #endregion
}
