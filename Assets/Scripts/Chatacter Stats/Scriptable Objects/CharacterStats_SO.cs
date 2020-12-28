using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
// using UnityEditor;

[CreateAssetMenu (fileName ="NewStats", menuName ="Character/Stast", order =1)]

public class CharacterStats_SO : ScriptableObject
{
    public Events.EventIntegerEvent OnLevelUp;
    public Events.EventIntegerEvent OnHeroDamaged;
    public Events.EventIntegerEvent OnHeroGainedHealth;
    public Events.EventIntegerEvent OnHeroGainedMana;
    public UnityEvent OnHeroDeath;
    public UnityEvent OnHeroInitialized;


    [System.Serializable] //Diferentes character tiene sus diferentes estadisticas cuando sube de nivel, por eso definimos que debe subir
    public class CharLevelUps
    {
        public int maxHealth;
        public int maxMana;
        public int maxWealth;
        public int baseDamage;
        public float baseResistence;
        public float maxEncumberance;
        public int requiredXP;
    }
    #region Fields
    //Datos
    public bool isHero = false;
    public bool saveDataonClose = false;
    //Armaduras y Items 
    public ItemPickUp weapon { get; private set; }
    public ItemPickUp headArmor { get; private set; }
    public ItemPickUp chestArmor { get; private set; }
    public ItemPickUp handArmor { get; private set; }
    public ItemPickUp legArmor { get; private set; }
    public ItemPickUp footArmor { get; private set; }
    public ItemPickUp misc1 { get; private set; }
    public ItemPickUp misc2 { get; private set; }

    //Vida
    public int maxHealth = 0;
    public int currentHeath = 0;
    public int maxMana = 0;
    public int currentMana = 0;

    //Cuanta dinero
    public int maxWealth = 0;
    public int currentWealth = 0;
    //daño
    public int baseDamage = 0;
    public int currentDamage = 0;

    public int currentDamageMagic = 0;
    public int baseDamageMagic = 0;

    //resistencia
    public float baseResistance= 0f;
    public float currentResistance= 0f;
    //El peso
    public float currentEncumbrance = 0f;
    public float maxEncumbrance = 0f;

    public int charExperience = 0;
  //  public int LevelExperience = 0;
    public int charLevel = 0;

    public CharLevelUps[] charLevelups;
    #endregion

    public VisualEffectAsset LevelUP;

    public int Levelpoints =0; 

    public bool GodMode;

    #region Levelpoints Stat Increasers

    public void IncreaserDamage(int damages)
    {
        currentDamage += damages; 
    }
    public void IncreaserHealth(int Health)
    {
        maxHealth += Health;
        currentHeath += Health;
    }
    public void IncreaserMana(int Mana)
    {
        maxMana += Mana;
        currentMana += Mana;
    }
    public void IncreaserResistence(int Resistence)
    {
        baseResistance += Resistence;
    }

    #endregion

    #region Stat Increasers

    public void ApplyHealth(int heathAmount)
    {
        if ((currentHeath + heathAmount) > maxHealth)
        {
            currentHeath = maxHealth;
        }
        else
        {
            currentHeath += heathAmount;
        }
        if (isHero)
            OnHeroGainedHealth.Invoke(heathAmount);
    }
    public void ApplyMana(int ManaAmount)
    {
        if ((currentMana + ManaAmount) > maxMana)
        {
            currentHeath = maxHealth;
        }
        else
        {
            currentMana += ManaAmount;
        }
        if (isHero)
            OnHeroGainedHealth.Invoke(ManaAmount);
    }
    public void GiveWealth(int wealthMoney)
    {
        if ((currentWealth + wealthMoney) > maxWealth)
        {
            currentWealth = maxWealth;
        }
        else
        {
            currentWealth += wealthMoney;
        }
    }
    public void GiveXP(int xp)
    {
        charExperience += xp;
        if(charLevel < charLevelups.Length)
        {
            int levelTaget = charLevelups[charLevel].requiredXP;

            if (charExperience >= levelTaget)
            {
                LevelUp(charLevel);
            }
        }
    }
    public void GiveMoney(int gold)
    {
        currentWealth += gold;
    }

    public void EquipWeapon(ItemPickUp weaponPickUp, CharacterInventory characterInventory, GameObject weaponSlot)
    {
        Debug.Log(weaponPickUp.name);

        Rigidbody newWeapon;
        characterInventory.inventoryDisplaySlots[1].sprite = weaponPickUp.itemDefinition.ItemIcon;
        newWeapon = Instantiate(weaponPickUp.itemDefinition.WeaponSpawnObject.weaponPreb, weaponSlot.transform);


        weapon = weaponPickUp;
        currentDamage +=  (int)weapon.itemDefinition.bonusDamage;
        currentDamageMagic +=  (int)weapon.itemDefinition.bonusDamageMagic;
        maxHealth +=  (int)weapon.itemDefinition.bonusHealthPoint;
        currentResistance +=  (int)weapon.itemDefinition.bonusResistence;
        currentMana +=  (int)weapon.itemDefinition.bonusManaPoint;

        
    }
    public void EquipArmor(ItemPickUp armorPickup, CharacterInventory characterInventory, SkinnedMeshRenderer ArmorSlot)
    {
        switch (armorPickup.itemDefinition.ItemArmorSubType) //Se hace mejor con un switch por que asi tenemos de forma mas especifica
        {
            case ItemArmorSubType.Head:
                characterInventory.inventoryDisplaySlots[2].sprite = armorPickup.itemDefinition.ItemIcon;
                headArmor = armorPickup; //se pone en la cabeza
                                         //  currentResistance += armorPickup.itemDefinition.itemAmount; //la resistencia otorgada.
                currentDamage += (int)armorPickup.itemDefinition.bonusDamage;
                currentDamageMagic +=  (int)armorPickup.itemDefinition.bonusDamageMagic;
                maxHealth += (int)armorPickup.itemDefinition.bonusHealthPoint;
                currentResistance += (int)armorPickup.itemDefinition.bonusResistence;
                currentMana += (int)armorPickup.itemDefinition.bonusManaPoint;
                //Skinsrender
                Mesh newHead;
                newHead = armorPickup.itemDefinition.itemArmor;
                //newHead = Instantiate(armorPickup.itemDefinition.itemArmor, ArmorSlot.transform );
                ArmorSlot.sharedMesh = newHead;
                ArmorSlot.sharedMaterial = armorPickup.itemDefinition.itemMaterial;
                Debug.Log(newHead.name);


                break;
            case ItemArmorSubType.Chest:
                characterInventory.inventoryDisplaySlots[3].sprite = armorPickup.itemDefinition.ItemIcon;
                chestArmor = armorPickup; //se pone en la pechera
                currentDamage +=  (int)armorPickup.itemDefinition.bonusDamage;
                currentDamageMagic +=  (int)armorPickup.itemDefinition.bonusDamageMagic;
                maxHealth +=  (int)armorPickup.itemDefinition.bonusHealthPoint;
                currentResistance +=   (int)armorPickup.itemDefinition.bonusResistence;
                currentMana += (int)armorPickup.itemDefinition.bonusManaPoint;
                //Skinsrender
                Mesh newChest;
                newChest = armorPickup.itemDefinition.itemArmor;
              
                ArmorSlot.sharedMesh = newChest;
                ArmorSlot.sharedMaterial = armorPickup.itemDefinition.itemMaterial;
                Debug.Log(newChest.name);
                break;
            case ItemArmorSubType.Boots:
                characterInventory.inventoryDisplaySlots[6].sprite = armorPickup.itemDefinition.ItemIcon;
                footArmor = armorPickup; //se pone en los pies
                currentDamage +=  (int)armorPickup.itemDefinition.bonusDamage;
                currentDamageMagic +=  (int)armorPickup.itemDefinition.bonusDamageMagic;
                maxHealth +=  (int)armorPickup.itemDefinition.bonusHealthPoint;
                currentResistance +=  (int)armorPickup.itemDefinition.bonusResistence;
                currentMana += (int)armorPickup.itemDefinition.bonusManaPoint;
                //Skinsrender
                Mesh newBoots;
                newBoots = armorPickup.itemDefinition.itemArmor;
                Debug.Log(newBoots.name);
                ArmorSlot.sharedMesh = newBoots;
                ArmorSlot.sharedMaterial = armorPickup.itemDefinition.itemMaterial;
                break;
            case ItemArmorSubType.Hands:
                characterInventory.inventoryDisplaySlots[4].sprite = armorPickup.itemDefinition.ItemIcon;
                handArmor = armorPickup; //se pone en la manos
                currentDamage +=  (int)armorPickup.itemDefinition.bonusDamage;
                currentDamageMagic +=  (int)armorPickup.itemDefinition.bonusDamageMagic;
                maxHealth += (int)armorPickup.itemDefinition.bonusHealthPoint;
                currentResistance +=  (int)armorPickup.itemDefinition.bonusResistence;
                currentMana += (int)armorPickup.itemDefinition.bonusManaPoint;

                //Skinsrender
                Mesh newHands;
                newHands = armorPickup.itemDefinition.itemArmor;
                Debug.Log(newHands.name);
                ArmorSlot.sharedMesh = newHands;
                ArmorSlot.sharedMaterial = armorPickup.itemDefinition.itemMaterial;

                break;
            case ItemArmorSubType.Legs:
                characterInventory.inventoryDisplaySlots[5].sprite = armorPickup.itemDefinition.ItemIcon;
                headArmor = armorPickup; //se pone en la cabeza
                currentDamage +=  (int)armorPickup.itemDefinition.bonusDamage;
                currentDamageMagic +=  (int)armorPickup.itemDefinition.bonusDamageMagic;
                maxHealth +=  (int)armorPickup.itemDefinition.bonusHealthPoint;
                currentResistance += (int)armorPickup.itemDefinition.bonusResistence;
                currentMana +=  (int)armorPickup.itemDefinition.bonusManaPoint;
                //Skinnerender
                Mesh newLegs;
                newLegs = armorPickup.itemDefinition.itemArmor;
                Debug.Log(newLegs.name);
                ArmorSlot.sharedMesh = newLegs;
                ArmorSlot.sharedMaterial = armorPickup.itemDefinition.itemMaterial;
                break;

        }
    }
    #endregion

    #region Stat Reducers

    public void TakeDamage(int amount)
    {

        currentHeath -= amount;

        Debug.Log(this.name + " " + currentHeath);

        if (isHero)
            OnHeroDamaged.Invoke(amount);
        if(currentHeath <= 0)
        {
            Death();
        }
    }
    public void TakeMana(int amount)
    {
        currentMana -= amount;

        if (currentHeath <= 0)
        {
            currentMana = 0;
        }
    }
    public bool UnEquipWeapon(ItemPickUp weaponPickUp, CharacterInventory characterInventory, GameObject weaponSlot) //una funcion retorna bool
    {
        bool previousWeaponSame = false;
        if(weaponSlot.transform.childCount > 0)
        {
            if(weapon == weaponPickUp)
            {
                previousWeaponSame = true;
            }
            if (weapon != weaponPickUp)
            {
                previousWeaponSame = false;
                Debug.Log("previos false");
            }
            characterInventory.inventoryDisplaySlots[1].sprite = null;
            Destroy(weaponSlot.transform.GetChild(0).gameObject);//Se destruye el primer Arma
            Debug.Log("Se destruyo el hijo");
            currentDamage -=  (int)weapon.itemDefinition.bonusDamage;
            currentDamageMagic -=  (int)weapon.itemDefinition.bonusDamageMagic;
            maxHealth -=  (int)weapon.itemDefinition.bonusHealthPoint;
            currentResistance -=   (int)weapon.itemDefinition.bonusResistence;
            currentMana -= (int)weapon.itemDefinition.bonusManaPoint;
            weapon = null;
        }

        return previousWeaponSame;
    }
    public bool UnEquipArmor(ItemPickUp armorPickup, CharacterInventory characterInventory, SkinnedMeshRenderer weaponSlot)
    {
        bool previousArmorSame = false;

        switch (armorPickup.itemDefinition.ItemArmorSubType) //Se hace mejor con un switch por que asi tenemos de forma mas especifica
        {

            case ItemArmorSubType.Head:
                    if (headArmor != null) //Si tiene armadura
                    {
                        if (headArmor == armorPickup)
                        {
                            previousArmorSame = true;
                        }
                        weaponSlot.sharedMesh = null;
                        characterInventory.inventoryDisplaySlots[2].sprite = null;
                        currentDamage -= (int)armorPickup.itemDefinition.bonusDamage;
                        currentDamageMagic -= (int)armorPickup.itemDefinition.bonusDamageMagic;
                        maxHealth -=  (int)armorPickup.itemDefinition.bonusHealthPoint;
                        currentResistance -=  (int)armorPickup.itemDefinition.bonusResistence;
                        currentMana -= (int)armorPickup.itemDefinition.bonusManaPoint;
                        headArmor = null;
                    }
                
                break;
            case ItemArmorSubType.Chest:
                if (chestArmor != null) //Si tiene armadura
                {
                    if (chestArmor == armorPickup)
                    {
                        previousArmorSame = true;
                    }
                    weaponSlot.sharedMesh = null;
                    characterInventory.inventoryDisplaySlots[3].sprite = null;
                    currentDamage -= (int)armorPickup.itemDefinition.bonusDamage;
                    currentDamageMagic -=  (int)armorPickup.itemDefinition.bonusDamageMagic;
                    maxHealth -=  (int)armorPickup.itemDefinition.bonusHealthPoint;
                    currentResistance -=  (int)armorPickup.itemDefinition.bonusResistence;
                    currentMana -= (int)armorPickup.itemDefinition.bonusManaPoint;
                    chestArmor = null;
                }
                break;
            case ItemArmorSubType.Hands:
                if (handArmor != null) //Si tiene armadura
                {
                    if (handArmor == armorPickup)
                    {
                        previousArmorSame = true;
                    }
                    weaponSlot.sharedMesh = null;
                    characterInventory.inventoryDisplaySlots[4].sprite = null;
                    currentDamage -= (int)armorPickup.itemDefinition.bonusDamage;
                    currentDamageMagic -=  (int)armorPickup.itemDefinition.bonusDamageMagic;
                    maxHealth -= (int)armorPickup.itemDefinition.bonusHealthPoint;
                    currentResistance -=  (int)armorPickup.itemDefinition.bonusResistence;
                    currentMana -=(int)armorPickup.itemDefinition.bonusManaPoint;
                    handArmor = null;
                }
                break;
            case ItemArmorSubType.Legs:
                if (legArmor != null) //Si tiene armadura
                {
                    if (legArmor == armorPickup)
                    {
                        previousArmorSame = true;
                    }
                    weaponSlot.sharedMesh = null;
                    characterInventory.inventoryDisplaySlots[5].sprite = null;
                    currentDamage -=  (int)armorPickup.itemDefinition.bonusDamage;
                    currentDamageMagic -= (int)armorPickup.itemDefinition.bonusDamageMagic;
                    maxHealth -= (int)armorPickup.itemDefinition.bonusHealthPoint;
                    currentResistance -= (int)armorPickup.itemDefinition.bonusResistence;
                    currentMana -=  (int)armorPickup.itemDefinition.bonusManaPoint;
                    legArmor = null;
                }
                break;
            case ItemArmorSubType.Boots:
                if (footArmor != null) //Si tiene armadura
                {
                    if (footArmor == armorPickup)
                    {
                        previousArmorSame = true;
                    }
                    weaponSlot.sharedMesh = null;
                    characterInventory.inventoryDisplaySlots[6].sprite = null;
                    currentDamage -=  (int)armorPickup.itemDefinition.bonusDamage;
                    currentDamageMagic -= (int)armorPickup.itemDefinition.bonusDamageMagic;
                    maxHealth -=  (int)armorPickup.itemDefinition.bonusHealthPoint;
                    currentResistance -= (int)armorPickup.itemDefinition.bonusResistence;
                    currentMana -= (int)armorPickup.itemDefinition.bonusManaPoint;
                    footArmor = null;
                }
                break;


        }

        return previousArmorSame; 
    }
    #endregion

    #region Character Level up and Death

    private void Death()
    {
        if (isHero)
            OnHeroDeath.Invoke();
        Debug.Log("You're Dead");
        //Call to game manager for Death State, ponte vio
        //Con sus animciones sipo 
    }

    private void LevelUp(int newLevel)
    {
        charLevel = newLevel +1;
        //Efectos de subir de nivel

        maxHealth += charLevelups[newLevel].maxHealth;
        currentHeath += charLevelups[newLevel].maxHealth;

        maxMana += charLevelups[newLevel].maxMana;
        currentMana += charLevelups[newLevel].maxMana;

        maxWealth += charLevelups[newLevel ].maxWealth;

        maxEncumbrance = charLevelups[newLevel ].maxEncumberance;
        if(weapon == null)
            baseDamage += charLevelups[newLevel ].baseDamage;
        else
            baseDamage = charLevelups[newLevel ].baseDamage + (int)weapon.itemDefinition.bonusDamage;
            

        baseResistance += charLevelups[newLevel].baseResistence;
        currentResistance += charLevelups[newLevel].baseResistence;

        int asdas = 0;
        charLevelups[newLevel].requiredXP = asdas;

        charExperience = 0;
        GameManager.Instance.OnHeroInit();

        Levelpoints++;
        if (charLevel > 1)
        {
            OnLevelUp.Invoke(charLevel);
        }
    }

    #endregion

    #region SaveCharacterData

    public void SaveCharacterData()
    {
       // saveDataonClose = true;
       // EditorUtility.SetDirty(this);
    }


    #endregion
}
