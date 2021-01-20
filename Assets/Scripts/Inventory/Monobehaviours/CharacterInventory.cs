using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventory : Singleton<CharacterInventory>
{
    #region Variable Declarations;

   

    public CharacterStats charStats; //una referencia al los stast
    public TMPro.TextMeshProUGUI textItemEntry;
    public Image[] hotBarDisplayHolders; //los slots de hechizos, las imagenes si po
    public GameObject InventoryDisplayHolder; //UI del inventory 
    public Image[] inventoryDisplaySlots; //los slots de inventario, las imagenes sipo, 
    public Button[] buttoninv = new Button[30];

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
        inventoryDisplaySlots = new Image[28];
        hotBarDisplayHolders = new Image[4];
        itemEntry = new InventoryEntry(0, null, null);
        itemsInInventory.Clear();
        

        //    buttoninv = InventoryDisplayHolder.GetComponentsInChildren<Button>();
        for (int i = 0; i < hotBarDisplayHolders.Length; i++)
        {
            Image image = GameObject.Find("Hotkeys").transform.GetChild(i).GetComponent<Image>();
            hotBarDisplayHolders[i] = image;
          //  hotBarDisplayHolders[i] = image;
            //Button button = GameObject.Find("grpHotBarDisplay").transform.GetChild(i).GetComponent<Button>();
            //buttoninv[i] = button; 
          
        }
        


       // InventoryDisplayHolder.SetActive(false);

        inventoryDisplaySlots = InventoryDisplayHolder.GetComponentsInChildren<Image>();

        charStats = GetComponent<CharacterStats>();
    }


    #endregion


    // Update is called once per frame
    void Update()
    {
        #region Watch for Hotbar Keypresses 

        if (Input.GetKeyDown(KeyCode.Alpha1) )
        {
            //int IDWeapon;
            //foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
            //{

            //    if (ie.Value.invEntry.itemDefinition.isIndestructable)
            //    {
            //        IDWeapon = ie.Key;
            //        TriggerItemUse(IDWeapon);
            //    }
            //}
            TriggerItemUse(101);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) )
        {
            //int IDWeapon;
            //foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
            //{

            //    if (ie.Value.invEntry.itemDefinition.isIndestructable)
            //    {
            //        IDWeapon = ie.Key;
            //        TriggerItemUse(IDWeapon);
            //    }
            //}
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

                if (itemEntry.invEntry.itemDefinition.IsStackable)
                {
                addedItem = AddItemToInv(addedItem);

                foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
                    {
                        if (itemEntry.invEntry.itemDefinition == ie.Value.invEntry.itemDefinition)
                        {
                            //Aqui Se añade un stack y se destruye la nueva instancia
                            AddItemToHotBar(ie.Value);
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
                    if(itemsInInventory.Count == inventoryItemCap)
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
            //    addedItem = AddItemToInv(addedItem);

    }
    public void StoreItem(ItemPickUp itemToStore) //para tomar las cosas que podrias tomar
    {
        addedItem = false;
        itemEntry.invEntry = itemToStore;
        itemEntry.stackSize = 1;
        itemEntry.hbSprite = itemToStore.itemDefinition.ItemIcon;
        textItemEntry.gameObject.SetActive(true);
        textItemEntry.text = "Has recogido: " + itemEntry.invEntry.itemDefinition.name;
        itemToStore.gameObject.SetActive(false);
    }
    public bool AddItemToInv (bool finishedAdding) //Si para ver si es stackable o no  y si lo hes para dar mas posiciones o sino darle un espacio mas
    {

        idCount = IncreaseID(idCount);

        //El tamaño de fila , ItemPickUP , Sprite
        itemsInInventory.Add(idCount, new InventoryEntry(itemEntry.stackSize, Instantiate(itemEntry.invEntry), itemEntry.hbSprite));
        //Ya tenemos los datos, se destruye
        FillInventoryDisplay();

        if (itemEntry.invEntry.itemDefinition.itemType == ItemTypeDefinition.WEAPON)
            AddItemToHotBar(itemsInInventory[idCount]);


        //El peso de arma, si excede no la tomara
        //  charStats.characterDefinition.currentEncumbrance += itemEntry.invEntry.itemDefinition.itemWeight;

        Destroy(itemEntry.invEntry.gameObject);

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

    private void AddItemToHotBar (InventoryEntry itemforHotBar) // para usar lo que tenemos en el inventario
    {
       int hotbarCounter = 0; //Contador de HotBar :V 
       bool increaseCount = false;

        //Check for open Hotbar slot
        #region Agregar Items de forma normal
        foreach (Image image in hotBarDisplayHolders)
        {
            hotbarCounter += 1;
            //Si es el primero
            if (itemforHotBar.hotBarSlot <= 0)
            {
                if (image.sprite == null)
                {
                    //Añadir item a hotbar slot
                    itemforHotBar.hotBarSlot = hotbarCounter; //Tomamos el slot
                    image.sprite = itemforHotBar.hbSprite;    //Añadimos el Srite
                    increaseCount = true; //Todo bien y es verdadero
                    break;
                }
            } //Si tomamos algo que es Stackeable
            else if (itemforHotBar.invEntry.itemDefinition.IsStackable)
            {
                increaseCount = true;
            }
        }
        #endregion
    



        if (increaseCount)
        {       //Si todo sale bien y bonito, pues en la barra saldra cuanto tienes de ese item;
            hotBarDisplayHolders[itemforHotBar.hotBarSlot].GetComponentInChildren<Text>().text = itemforHotBar.stackSize.ToString();
        }
        increaseCount = false;
    }
    void DisplayInventory()
    {
        if (InventoryDisplayHolder.activeSelf == true)
        {
            InventoryDisplayHolder.SetActive(false);
            textItemEntry.gameObject.SetActive(false);
        }
        else
        {
            InventoryDisplayHolder.SetActive(true);
            int slotCounter = 7;
            foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
            {
                slotCounter += 1;
                ie.Value.inventorySlot = slotCounter - 7;
                buttoninv[slotCounter].onClick.AddListener(delegate { TriggerItemUse(ie.Key); });
            }
            while (slotCounter < itemsInInventory.Count) //Para ver los Slot libres uwu
            {
                slotCounter++;
                inventoryDisplaySlots[slotCounter].sprite = null;
            }

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
    void FillInventoryDisplay()
    {
        int slotviewequiped = 8; // 9 porque no tiene que contar la armadura ni mistic y ni con el characterview

        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            slotviewequiped += 1;
            inventoryDisplaySlots[slotviewequiped].sprite = ie.Value.hbSprite;
            ie.Value.inventorySlot = slotviewequiped - 8;
          //  buttoninv[slotCounter - 9].onClick.AddListener(delegate { TriggerItemUse(ie.Key); });
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
                       if  (ie.Value.hotBarSlot <= 0)
                        {
                            hotBarDisplayHolders[ie.Value.hotBarSlot - 1].sprite = null;
                            hotBarDisplayHolders[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = "0";
                        }
                        ie.Value.invEntry.UseItem();
                        itemsInInventory.Remove(ie.Key);
                        break;
                    }
                    else
                    {
                        ie.Value.invEntry.UseItem(); //Si no es Stackeable, (por ejemplo un objeto de mision)
                        if (!ie.Value.invEntry.itemDefinition.isIndestructable)
                        {
                    //        Debug.Log("Mision");
                            itemsInInventory.Remove(ie.Key);
                            break;
                        }
                    }
                }else //Si es mayor que uno. se gasta un y se reescribe 
                {
               //     Debug.Log("Normal");
                    ie.Value.invEntry.UseItem();
                    ie.Value.stackSize -= 1;
                    hotBarDisplayHolders[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = ie.Value.stackSize.ToString();
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

}
