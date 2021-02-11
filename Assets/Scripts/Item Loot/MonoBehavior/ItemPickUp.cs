using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(BoxCollider))]
public class ItemPickUp : MonoBehaviour
{
    public ItemPickUp_SO itemDefinition;
    public CharacterStats charStats;

    CharacterInventory charInventory;

    GameObject foundStats;

    #region Constructors 
    public ItemPickUp() //los metodos constructor se inicializan las variables, parecido un void Start
    {
        charInventory = CharacterInventory.instance;
    }

    #endregion

    private void Start()
    {
        if(charStats ==null)
        {
            foundStats = GameObject.FindGameObjectWithTag("Player");
            charStats = foundStats.GetComponent<CharacterStats>();
        }
    }
    void StoreItemInInventory()
    {
        charInventory.StoreItem(this);
    }
   public void UseItem()
    {
        Debug.Log("usa el item");
        switch (itemDefinition.itemType)
        {
            case ItemTypeDefinition.HEALTH:
                {
                    charStats.ApplyHealth(itemDefinition.itemAmount);
                    break;
                }
            case ItemTypeDefinition.MANA:
                {
                    charStats.ApplyMana(itemDefinition.itemAmount);
                    break;
                }
            case ItemTypeDefinition.WEALTH:
                {
                    charStats.GiveWealth(itemDefinition.itemAmount);
                    break;
                }
            case ItemTypeDefinition.WEAPON:
                {
                    charStats.ChangeWeapon(this);
                    break;
                }
            case ItemTypeDefinition.ARMOR:
                {
                    charStats.ChangeArmor(this);
                    break;
                }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (itemDefinition.isStorable)
            {
                StoreItemInInventory();
                Debug.Log(gameObject.name + "" + "He interactuado con" + other.name);
            }
            else
            {
                UseItem();
                Debug.Log("caca");
            }
        }
    }
}
