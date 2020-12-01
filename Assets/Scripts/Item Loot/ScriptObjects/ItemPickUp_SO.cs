using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypeDefinition { HEALTH, WEALTH, MANA, WEAPON, ARMOR, BUFF, EMPTY, SPELLS };

public enum ItemArmorSubType {None, Head, Chest, Hands, Legs, Boots };
[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Spawnable Item/New pick up", order = 1)]
public class ItemPickUp_SO : ScriptableObject
{
    public string itemName = "New Item"; //el nombre del item

    [Header("type Item")]

    public ItemTypeDefinition itemType = ItemTypeDefinition.HEALTH;
    public ItemArmorSubType ItemArmorSubType = ItemArmorSubType.None;

    [Header ("Description Item")]

    public Material itemMaterial = null;
    public Sprite ItemIcon = null;
    public Rigidbody itemSpawnObject = null;

    public Weapon WeaponSpawnObject = null;
    [Header("PointArmor")]
    public Mesh itemArmor = null;
    public GameObject itemArmor2 = null;


    //  public int itemAmount = 0; //Cuantos items hay
    public int spawnChangeWeight = 0;
    
    [Space]
    [Header("Features Item")]

    public bool isEquipped = false; //Si se equipanle
    public bool isInteractable = false; //
    public bool isStorable = false; //Si es Almacenable
    public bool isUnique = false;  //si es unico
    public bool isIndestructable = false;
    public bool isQuestItem = false; //Si es un item de una mision 
    public bool IsStackable = false;
    public bool destrouOnUse = false;

    [Space]
    [Header("Attributes Item/Store")]
    public int itemAmount;


    [Space]
    [Header("Attributes Item")]

    public float itemWeight = 0f; //Peso del item 

    public float bonusResistence;
    public float bonusHealthPoint;
    public float bonusDamage; 
    public float bonusDamageMagic; 
    public float bonusManaPoint; 

    //Estaba pensando colocar funciones para las Armas y armaduras, pasivas que siempre hay, tiene que ser funciones. 
}
