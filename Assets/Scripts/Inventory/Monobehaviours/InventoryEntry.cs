using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEntry //Es un contructor
{
    public ItemPickUp invEntry; //Almacena la informacion del item
    public int stackSize;       //El tamañano de fila
    public int inventorySlot;  //los slots asignado para los items
    public int hotBarSlot;      //para los barra 
    public Sprite hbSprite;
    public int idItem;

    public InventoryEntry(int stackSize, ItemPickUp invEntry, Sprite hbSprite)
    {
        this.invEntry = invEntry;

        this.stackSize = stackSize;
        this.hotBarSlot = 0;
        this.inventorySlot = 0;
        this.hbSprite = hbSprite;
    }
}
