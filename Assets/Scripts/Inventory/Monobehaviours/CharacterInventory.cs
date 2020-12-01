using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventory : MonoBehaviour
{
    #region Variable Declarations;

    public static CharacterInventory instance;

    public CharacterStats charStats; //una referencia al los stast
    GameObject foundStats;
    public TMPro.TextMeshProUGUI textMeshPro;
    public Image[] hotBarDisplayHolders = new Image[4]; //los slots de hechizos, las imagenes si po
    public GameObject InventoryDisplayHolder; //UI del inventory 
    public Image[] inventoryDisplaySlots = new Image[30]; //los slots de inventario, las imagenes sipo, 
    public Button[] buttoninv = new Button[30]; 

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
      
        //    buttoninv = InventoryDisplayHolder.GetComponentsInChildren<Button>();
        for (int i = 0; i < hotBarDisplayHolders.Length; i++)
        {
            Image image = GameObject.Find("Hotkeys").transform.GetChild(i).GetComponent<Image>();
            hotBarDisplayHolders[i] = image;
            //Button button = GameObject.Find("grpHotBarDisplay").transform.GetChild(i).GetComponent<Button>();
            //buttoninv[i] = button; 
          
        }

       
        InventoryDisplayHolder.SetActive(false);
        itemEntry = new InventoryEntry(0, null, null);
        itemsInInventory.Clear();

        inventoryDisplaySlots = InventoryDisplayHolder.GetComponentsInChildren<Image>();

        charStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
    }


    #endregion


    // Update is called once per frame
    void Update()
    {
        #region Watch for Hotbar Keypresses 

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TriggerItemUse(101);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TriggerItemUse(102);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TriggerItemUse(103);
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

    public void StoreItem(ItemPickUp itemToStore) //para tomar las cosas que podrias tomar
    {
        addedItem = false;

        if((charStats.characterDefinition.currentEncumbrance + itemToStore.itemDefinition.itemWeight) <= charStats.characterDefinition.maxEncumbrance)
        {
            itemEntry.invEntry = itemToStore;
            itemEntry.stackSize = 1;
            itemEntry.hbSprite = itemToStore.itemDefinition.ItemIcon;
         //   Debug.Log("Tomando cosas");
            itemToStore.gameObject.SetActive(false);
        }
     //   Debug.Log(addedItem);
    }
    public void TryPickUP() //condiciones que tiene para tomar el item 
    {
        bool itsInInv = true;

        //Para ver si se puede meter al inventario 
        if (itemEntry.invEntry)
        {
            // si tienes no tiene este item - se añade 
            if(itemsInInventory.Count == 0)
            {
                addedItem = AddItemToInv(addedItem);
           //     Debug.Log("Se tomo el item");
            }
            //si este item ya lo tienes - Si 
            else
            {   
                //Si es Stackeable 
                if (itemEntry.invEntry.itemDefinition.IsStackable)
                {
                    foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
                    {
                        if (itemEntry.invEntry.itemDefinition == ie.Value.invEntry.itemDefinition)
                        {
                            //Aqui Se añade un stack y se destruye la nueva instancia
                            ie.Value.stackSize += 1;
                            AddItemToHotBar(ie.Value);
                            itsInInv = true;
                            Destroy(itemEntry.invEntry.gameObject);
                            break;
                            //Basicamente añade a nuestro inventario y al tocarlo
                        }
                        else //Si el item ya no existe del inventario 
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
        }

    }
    public bool AddItemToInv (bool finishedAdding) //Si para ver si es stackable o no  y si lo hes para dar mas posiciones o sino darle un espacio mas
    {
        textMeshPro.gameObject.SetActive(true);
        textMeshPro.text = "Has recogido: " + itemEntry.invEntry.itemDefinition.name;

        idCount = IncreaseID(idCount);

        //El tamaño de fila , ItemPickUP , Sprite
        itemsInInventory.Add(idCount, new InventoryEntry(itemEntry.stackSize, Instantiate(itemEntry.invEntry), itemEntry.hbSprite));
        //Ya tenemos los datos, se destruye
        FillInventoryDisplay();
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
        foreach (Image image in hotBarDisplayHolders)
        {
            
            //Si es el primero
            if(itemforHotBar.hotBarSlot == 0 && itemforHotBar.invEntry.itemDefinition.itemType == ItemTypeDefinition.WEAPON)
            {
                hotbarCounter = 1;
                if (image.sprite == null)
                {
                    Debug.Log("Caca");
                    //Añadir item a hotbar slot
                    itemforHotBar.hotBarSlot = hotbarCounter; //Tomamos el slot
                    image.sprite = itemforHotBar.hbSprite;    //Añadimos el Srite
                    increaseCount = true; //Todo bien y es verdadero
                    break;
                }

            }
           else if (itemforHotBar.hotBarSlot == 1 && itemforHotBar.invEntry.itemDefinition.itemType == ItemTypeDefinition.WEAPON)
           {
                hotbarCounter = 2;
                if (image.sprite == null)
                {
                    Debug.Log("Caca");
                    //Añadir item a hotbar slot
                    itemforHotBar.hotBarSlot = hotbarCounter; //Tomamos el slot
                    image.sprite = itemforHotBar.hbSprite;    //Añadimos el Srite
                    increaseCount = true; //Todo bien y es verdadero
                    break;
                }

           } 
            

            else if (itemforHotBar.hotBarSlot < 1 && itemforHotBar.invEntry.itemDefinition.itemType == ItemTypeDefinition.HEALTH)
            {
                hotbarCounter = 3;
                if (image.sprite == null)
                {
                    Debug.Log("Caca" + hotbarCounter);
                    //Añadir item a hotbar slot
                    itemforHotBar.hotBarSlot = hotbarCounter; //Tomamos el slot
                    image.sprite = itemforHotBar.hbSprite;    //Añadimos el Srite
                    increaseCount = true; //Todo bien y es verdadero
                    break;

                }
            }
            else if (itemforHotBar.hotBarSlot < 2 && itemforHotBar.invEntry.itemDefinition.itemType == ItemTypeDefinition.MANA)
            {
                hotbarCounter = 4;
                if (image.sprite == null)
                {
                    Debug.Log("Caca" + hotbarCounter);
                    //Añadir item a hotbar slot
                    itemforHotBar.hotBarSlot = hotbarCounter; //Tomamos el slot
                    image.sprite = itemforHotBar.hbSprite;    //Añadimos el Srite
                    increaseCount = true; //Todo bien y es verdadero
                    break;
                }
            }
            else if (itemforHotBar.invEntry.itemDefinition.IsStackable)
            {
                increaseCount = true;
            }
        }

        if (increaseCount)
        {       //Si todo sale bien y bonito, pues en la barra saldra cuanto tienes de ese item;
            hotBarDisplayHolders[itemforHotBar.hotBarSlot - 1].GetComponentInChildren<Text>().text = itemforHotBar.stackSize.ToString();
        }

        increaseCount = false;
    }
    void DisplayInventory()
    {
        if (InventoryDisplayHolder.activeSelf == true)
        {
            InventoryDisplayHolder.SetActive(false);
            textMeshPro.gameObject.SetActive(false);
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
               // inventoryDisplaySlots[slotCounter].sprite = null;
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
        int slotCounter = 9; // 9 porque no tiene que contar la armadura ni mistic y ni con el characterview

        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            slotCounter += 1;
            inventoryDisplaySlots[slotCounter].sprite = ie.Value.hbSprite;
            ie.Value.inventorySlot = slotCounter - 9;
          //  buttoninv[slotCounter - 9].onClick.AddListener(delegate { TriggerItemUse(ie.Key); });
        }
        while (slotCounter < itemsInInventory.Count) //Para ver los Slot libres uwu
        {
            slotCounter++;
            inventoryDisplaySlots[slotCounter].sprite = null;
        }
    }
    public void TriggerItemUse(int itemToUseID) //usar los items
    {

        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {      //Esto es solo para ver de donde se uso el items, es descartable
        bool triggerItem = false; // si lo hemos tengo usado
            if (itemToUseID > 100) //si es mayor a 100, se le resta 100 para ver de donde vino (por ejemplo de Hotbar) y así tienes el ID correcto
            {
                itemToUseID -= 100;
               // Debug.Log(ie.Key.ToString() + itemToUseID + "Triggeritemuse");

                if(ie.Value.hotBarSlot == itemToUseID)
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
            if (triggerItem)
            {
                if (ie.Value.stackSize == 1)
                {
                    if (ie.Value.invEntry.itemDefinition.IsStackable) // Si stackeable el objeto usado (Pociones por dar un ejemplo)s
                    {
                       if  (ie.Value.hotBarSlot != 0)
                        {
                            hotBarDisplayHolders[ie.Value.hotBarSlot - 1].sprite = null;
                            hotBarDisplayHolders[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = "0";
                        }
                      //  Debug.Log("Stack");
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
