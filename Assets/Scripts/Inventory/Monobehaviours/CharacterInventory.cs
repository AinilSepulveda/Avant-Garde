using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventory : MonoBehaviour
{
    #region Variable Declarations;
    public static CharacterInventory instance;
    public TMPro.TextMeshProUGUI textItemEntry;
    public TMPro.TextMeshProUGUI textItemTutorial;
    public Image[] hotBarDisplayHoldersWeapons; //los slots de hechizos, las imagenes si po
    public Image[] hotBarDisplayHoldersPotions; //los slots de hechizos, las imagenes si po
    public GameObject InventoryDisplayHolder; //UI del inventory 
    public GameObject HotskeysDisplayHolderWeapons; //UI del inventory 
    public GameObject HotskeysDisplayHolder; //UI del inventory 
    public Image[] inventoryDisplaySlots; //los slots de inventario, las imagenes sipo, 
    public Button[] buttoninv = new Button[30];

    public GameObject uIInvetario;
    public int slotCounterinv = 9;

    int inventoryItemCap = 20; //Limite del slots
    int idCount = 1; //ID de los objetos
    bool addedItem = true;

    public Dictionary<int, InventoryEntry> itemsInInventory = new Dictionary<int, InventoryEntry>(); // Para guardar el ID del item y además el item en si
    public InventoryEntry itemEntry;
    public GameObject stastHUD;
    #endregion

    #region Initializations
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        ResetInventory();
        Debug.Log("RESET COMPLETE");



        hotBarDisplayHoldersWeapons = HotskeysDisplayHolderWeapons.GetComponentsInChildren<Image>();
        hotBarDisplayHoldersPotions = HotskeysDisplayHolder.GetComponentsInChildren<Image>();
        // InventoryDisplayHolder.SetActive(false);

        inventoryDisplaySlots = InventoryDisplayHolder.GetComponentsInChildren<Image>();

     
    }

    public void ResetInventory()
    {
        inventoryDisplaySlots = new Image[28];
        hotBarDisplayHoldersWeapons = new Image[4];
        itemEntry = new InventoryEntry(0, null, null);
        itemsInInventory.Clear();
    }


    #endregion


    // Update is called once per frame
    void Update()
    {
        #region Watch for Hotbar Keypresses 

        if (Input.GetKeyDown(KeyCode.Alpha1) )
        {

            TriggerItemUse(101);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) )
        {

            TriggerItemUse(102);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            int Idhealth;
            foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
            {
                
                if (ie.Value.invEntry.itemDefinition.IsStackable) 
                {
                    Idhealth = ie.Key;
                    TriggerItemUse(Idhealth);
                }
            }
         //   TriggerItemUse(103);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TriggerItemUse(104);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TriggerItemUse(105);

        }
        if (Input.GetKeyDown(KeyCode.I) && GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            if (uIInvetario.activeSelf == true)
            {
                uIInvetario.SetActive(false);
                textItemEntry.gameObject.SetActive(false);
                textItemTutorial.gameObject.SetActive(false);
            }
            else if ((uIInvetario.activeSelf == false))
            {
                uIInvetario.SetActive(true);
            }
            // Debug.Log("abriendo el inventario");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uIInvetario.activeSelf == true)
                uIInvetario.SetActive(false);

            if (stastHUD.activeSelf == true)
                stastHUD.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (stastHUD.activeSelf == true)
            {
                stastHUD.SetActive(false);

            }
            else
            {
                stastHUD.SetActive(true);
            }
            // Debug.Log("abriendo el inventario");
        }

        #endregion

        if (!addedItem)
        {
            TryPickUP();
          //  Debug.Log("Tratando de tomar el item");
        }
    }

    public void TryPickUP() //condiciones que tiene para tomar el item 
    {
        bool itsInInv = true;

        //Para ver si se puede meter al inventario 
        if (itemEntry.invEntry)
        {
            if (itemsInInventory.Count == 0)
            {
                addedItem = AddItemToInv(addedItem);

            }
            else
            {

                if (itemEntry.invEntry.itemDefinition.IsStackable)
                {

                    foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
                    {
                        if (itemEntry.invEntry.itemDefinition == ie.Value.invEntry.itemDefinition)
                        {
                            //Aqui Se añade un stack y se destruye la nueva instancia
                            if (itemEntry.invEntry.itemDefinition.itemType == ItemTypeDefinition.WEAPON)
                            {
                                AddItemToHotBarWeapons(ie.Value);
                            }
                            else if (itemEntry.invEntry.itemDefinition.itemType == ItemTypeDefinition.HEALTH || itemEntry.invEntry.itemDefinition.itemType == ItemTypeDefinition.MANA)
                            {

                                AddItemToHotBarPotions(ie.Value);
                            }
                            ie.Value.stackSize += 1;
                            itsInInv = true;
                            Destroy(itemEntry.invEntry.gameObject);
                            break;
                            //Basicamente añade a nuestro inventario y al tocarlo
                        }
                        else
                        {
                            itsInInv = false;
                        }
                    }

                }//Si el item no es stackeable - If not
                else
                {
                    itsInInv = false;

                    //If no space y el item no es stackeable - say inventory full
                    if (itemsInInventory.Count == inventoryItemCap)
                    {
                        itemEntry.invEntry.gameObject.SetActive(true);
                        Debug.Log("Inventary is Full");

                    }
                }
                // Si tieenes  espacio - se añade
                if (!itsInInv)
                {
                    addedItem = AddItemToInv(addedItem);
                    itsInInv = true;
                }
            }
            
        }
            //    addedItem = AddItemToInv(addedItem);

    }
    public void StoreItem(ItemPickUp itemToStore) //para tomar las cosas que podrias tomar
    {
        addedItem = false;
        itemEntry.invEntry = itemToStore;
        itemEntry.stackSize = 1;
        itemEntry.hbSprite = itemToStore.itemDefinition.ItemIcon;
        itemToStore.gameObject.SetActive(false);
        textItemEntry.gameObject.SetActive(true);
        textItemEntry.text = "Has recogido: " + itemEntry.invEntry.itemDefinition.name;

    }
    public bool AddItemToInv (bool finishedAdding) //Si para ver si es stackable o no  y si lo hes para dar mas posiciones o sino darle un espacio mas
    {

        idCount = IncreaseID(idCount);

        //El tamaño de fila , ItemPickUP , Sprite
        itemsInInventory.Add(idCount, new InventoryEntry(itemEntry.stackSize, Instantiate(itemEntry.invEntry), itemEntry.hbSprite));
        //Ya tenemos los datos, se destruye
        FillInventoryDisplay();




        if (itemEntry.invEntry.itemDefinition.itemType == ItemTypeDefinition.WEAPON)
        {
            Debug.Log("Weapons");
            AddItemToHotBarWeapons(itemsInInventory[idCount]);
        }
        else if (itemEntry.invEntry.itemDefinition.itemType == ItemTypeDefinition.HEALTH || itemEntry.invEntry.itemDefinition.itemType == ItemTypeDefinition.MANA)
        {

            AddItemToHotBarPotions(itemsInInventory[idCount]);
        }
        Destroy(itemEntry.invEntry.gameObject);


        //El peso de arma, si excede no la tomara
        //  charStats.characterDefinition.currentEncumbrance += itemEntry.invEntry.itemDefinition.itemWeight;


        #region Reset ItemEntry
        itemEntry.invEntry = null;
        itemEntry.stackSize = 0;
        itemEntry.hbSprite = null;
        #endregion

        finishedAdding = true;

        return finishedAdding;
    }
    int IncreaseID(int currentID)
    {
        int newID = 1;
        for (int itemCount = 1; itemCount <= itemsInInventory.Count; itemCount++)
        {
            if (itemsInInventory.ContainsKey(newID))
            {
                newID++;
            }
            else return newID;
        }

        return newID;
    }

    private void AddItemToHotBarWeapons (InventoryEntry itemforHotBar) // para usar lo que tenemos en el inventario
    {
       int hotbarCounter = 0; //Contador de HotBar :V 
       bool increaseCount = false;

        //Check for open Hotbar slot
         #region Agregar Items de forma normal
         foreach (Image image in hotBarDisplayHoldersWeapons)
         {
            // hotbarCounter += 1;
             //Si es el primero
             if (itemforHotBar.hotBarSlot == 0)
             {
                hotbarCounter = 1;
                if (image.sprite == null)
                 {
                     //Añadir item a hotbar slot
                     itemforHotBar.hotBarSlot = hotbarCounter; //Tomamos el slot
                     image.sprite = itemforHotBar.hbSprite;
                    image.color = Color.white;//Añadimos el Srite
                    increaseCount = true; //Todo bien y es verdadero
                     break;
                 }
             }

            if (itemforHotBar.hotBarSlot == 1)
            {
                hotbarCounter = 2;
                if (image.sprite == null)
                {
                    //Añadir item a hotbar slot
                    itemforHotBar.hotBarSlot = hotbarCounter; //Tomamos el slot
                    image.sprite = itemforHotBar.hbSprite;    //Añadimos el Srite
                    image.color = Color.white;
                    increaseCount = true; //Todo bien y es verdadero
                    break;
                }
            }
            //Si tomamos algo que es Stackeable
            //else if (itemforHotBar.invEntry.itemDefinition.IsStackable)
            // {
            //     increaseCount = true;
            // }
         }
         #endregion 

        //if (increaseCount)
        //{       //Si todo sale bien y bonito, pues en la barra saldra cuanto tienes de ese item;
        //    hotBarDisplayHoldersWeapons[itemforHotBar.hotBarSlot -1].GetComponentInChildren<Text>().text = itemforHotBar.stackSize.ToString();
        //}
        increaseCount = false;
    
    }
    private void AddItemToHotBarPotions(InventoryEntry itemforHotBar) // para usar lo que tenemos en el inventario
    {
        int hotbarCounter = 0; //Contador de HotBar :V 
        bool increaseCount = false;

        //Check for open Hotbar slot
        #region Agregar Items de forma normal
        foreach (Image image in hotBarDisplayHoldersPotions)
        {
             hotbarCounter += 1;
            //Si es el primero
            if (itemforHotBar.hotBarSlot == 0)
            {
               // hotbarCounter = 1;
                if (image.sprite == null)
                {
                    //Añadir item a hotbar slot
                    itemforHotBar.hotBarSlot = hotbarCounter; //Tomamos el slot
                    image.sprite = itemforHotBar.hbSprite;    //Añadimos el Srite
                    image.color = Color.white;
                    increaseCount = true; //Todo bien y es verdadero
                    break;
                }
            }

            //if (itemforHotBar.hotBarSlot == 1)
            //{
            //    hotbarCounter = 2;
            //    if (image.sprite == null)
            //    {
            //        //Añadir item a hotbar slot
            //        itemforHotBar.hotBarSlot = hotbarCounter; //Tomamos el slot
            //        image.sprite = itemforHotBar.hbSprite;    //Añadimos el Srite
            //        increaseCount = true; //Todo bien y es verdadero
            //        break;
            //    }
            //}
            //Si tomamos algo que es Stackeable
            else if (itemforHotBar.invEntry.itemDefinition.IsStackable)
            {
                increaseCount = true;
            }
        }
        #endregion

        if (increaseCount)
        {       //Si todo sale bien y bonito, pues en la barra saldra cuanto tienes de ese item;
            hotBarDisplayHoldersPotions[itemforHotBar.hotBarSlot - 1].GetComponentInChildren<Text>().text = itemforHotBar.stackSize.ToString();
        }
        increaseCount = false;

    }
    void FillInventoryDisplay()
    {
        int slotviewequiped = 7; // 9 porque no tiene que contar la armadura ni mistic y ni con el characterview

        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            slotviewequiped += 1;
            inventoryDisplaySlots[slotviewequiped].sprite = ie.Value.hbSprite;
            ie.Value.inventorySlot = slotviewequiped - 7;
         
        }
        while (slotviewequiped < itemsInInventory.Count) //Para ver los Slot libres uwu
        {
            slotviewequiped++;
            inventoryDisplaySlots[slotviewequiped].sprite = null;
        }
    }
    public void TriggerItemUse(int itemToUseID) //usar los items
    {

        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {      //Esto es solo para ver de donde se uso el items, es descartable
            bool triggerItem = false; // si lo hemos tengo usado
            if (itemToUseID > 100) //si es mayor a 100, se le resta 100 para ver de donde vino (por ejemplo de Hotbar) y así tienes el ID correcto
            {
                // Debug.Log(ie.Key.ToString() + itemToUseID + "Triggeritemuse");
                itemToUseID -= 100;
                if (ie.Value.hotBarSlot == itemToUseID)
                {
                    triggerItem = true;
                }
            }
            else
            {
                if (ie.Value.inventorySlot == itemToUseID)
                {

                    triggerItem = true;               
                }
            }
            //      Debug.Log("te pillamos" + ie.Key + " " + triggerItem + " " + ie.Value.hotBarSlot); //Es por los items
            if (triggerItem) //Se usa el items 
            {
                if (ie.Value.stackSize == 1) //Si es 1 significa es puede ser un objeto
                {
                    if (ie.Value.invEntry.itemDefinition.IsStackable) // Si stackeable el objeto usado (Pociones por dar un ejemplo)s
                    {
                       if  (ie.Value.hotBarSlot >= 0)
                        {
                            hotBarDisplayHoldersPotions[ie.Value.hotBarSlot - 1].sprite = null;
                            hotBarDisplayHoldersPotions[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = "0";
                            hotBarDisplayHoldersPotions[ie.Value.hotBarSlot - 1].color = Color.clear;
                        }
                        ie.Value.invEntry.UseItem();
                        inventoryDisplaySlots[idCount + 7].sprite = null;
                        itemsInInventory.Remove(ie.Key);
                        break;
                    }
                    else
                    {
                        ie.Value.invEntry.UseItem(); //Si no es Stackeable, (por ejemplo un objeto de mision)
                    }
                }
                else //Si es mayor que uno. se gasta un y se reescribe 
                {

                    ie.Value.invEntry.UseItem();
                    ie.Value.stackSize -= 1;
                    if (ie.Value.invEntry.itemDefinition.itemType == ItemTypeDefinition.WEAPON)
                    {
                        hotBarDisplayHoldersWeapons[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = ie.Value.stackSize.ToString(); ;
                    }
                    else if (ie.Value.invEntry.itemDefinition.itemType == ItemTypeDefinition.HEALTH || itemEntry.invEntry.itemDefinition.itemType == ItemTypeDefinition.MANA)
                    {

                        hotBarDisplayHoldersPotions[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = ie.Value.stackSize.ToString();
                    }
                    
                }
                Debug.Log(ie.Value.invEntry.itemDefinition + "Is use");
            }
        }

        FillInventoryDisplay();
    }
    public void RemoveItems(InventoryEntry entry)
    {
        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            if (entry == ie.Value)
            {
                itemsInInventory.Remove(ie.Key);
            }
        }
    }

    public void WeaponInt()
    {
        int idItem = 0;
        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            if (ie.Value.invEntry.itemDefinition.itemType == ItemTypeDefinition.WEAPON)
            {
                idItem = ie.Key;
                TriggerItemUse(idItem);
            }
        }
        Debug.Log("WeaponInt");
    }
    public void ArmorBootsInt()
    {
        int idItem = 0;
        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            if (ie.Value.invEntry.itemDefinition.itemType == ItemTypeDefinition.ARMOR && ie.Value.invEntry.itemDefinition.ItemArmorSubType == ItemArmorSubType.Boots)
            {
                idItem = ie.Key;
                TriggerItemUse(idItem);
            }
        }
        Debug.Log("WeaponInt");
    }
    public void ArmorHeadInt()
    {
        int idItem = 0;
        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            if (ie.Value.invEntry.itemDefinition.itemType == ItemTypeDefinition.ARMOR && ie.Value.invEntry.itemDefinition.ItemArmorSubType == ItemArmorSubType.Boots)
            {
                idItem = ie.Key;
                TriggerItemUse(idItem);
            }
        }
        Debug.Log("WeaponInt");
    }
    public void ArmorChestInt()
    {
        int idItem = 0;
        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            if (ie.Value.invEntry.itemDefinition.itemType == ItemTypeDefinition.ARMOR && ie.Value.invEntry.itemDefinition.ItemArmorSubType == ItemArmorSubType.Head)
            {
                idItem = ie.Key;
                TriggerItemUse(idItem);
            }
        }
        Debug.Log("WeaponInt");
    }
    public void ArmorLegsInt()
    {
        int idItem = 0;
        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            if (ie.Value.invEntry.itemDefinition.itemType == ItemTypeDefinition.ARMOR && ie.Value.invEntry.itemDefinition.ItemArmorSubType == ItemArmorSubType.Legs)
            {
                idItem = ie.Key;
                TriggerItemUse(idItem);
            }
        }
        Debug.Log("WeaponInt");
    }
    public void ArmorHandsInt()
    {
        int idItem = 0;
        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            if (ie.Value.invEntry.itemDefinition.itemType == ItemTypeDefinition.ARMOR && ie.Value.invEntry.itemDefinition.ItemArmorSubType == ItemArmorSubType.Hands)
            {
                idItem = ie.Key;
                TriggerItemUse(idItem);
            }
        }
        Debug.Log("WeaponInt");
    }
}
