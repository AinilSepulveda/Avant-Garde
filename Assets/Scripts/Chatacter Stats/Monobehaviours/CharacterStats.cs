using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStats : MonoBehaviour
{
    public CharacterStats_SO character_Template;
    public CharacterStats_SO characterDefinition;
    public CharacterInventory charInv;
    public GameObject characterWeaponSlot;

    //quitar eso despues
    public GameObject caca;


    //Status Effect Manager
    [SerializeField]
    List<int> burnTickTimers = new List<int>();
    public bool iskeepDamage;

    //Ataques
    public AttackDefinition attack;

    #region Constructor

    public CharacterStats()
    {
        charInv = CharacterInventory.instance;
    }

    #endregion

    #region Initializations

    private void Awake()
    {
        
            if (character_Template != null) // Se crea un modelo de characterDefinition
                characterDefinition = Instantiate(character_Template);
        
    }

    private void Start()
   { 

        if (characterDefinition.isHero) //no si controlamos la wea. 
        {
            characterDefinition.maxHealth = 500;
            characterDefinition.currentHeath = 500;

            characterDefinition.maxMana = 25;
            characterDefinition.currentMana = 25;

            characterDefinition.maxWealth = 500;
            characterDefinition.currentWealth = 0;

            characterDefinition.baseResistance = 0;
            characterDefinition.currentResistance = 0;

            characterDefinition.maxEncumbrance = 50f; //El peso
            characterDefinition.currentEncumbrance = 0f;

            characterDefinition.charExperience = 0;
            characterDefinition.charLevel = 1;

            characterDefinition.baseDamage = 2;
            characterDefinition.currentDamage = 1;
        }
        else if(characterDefinition == null)
        {
            return;
        }
    }

    #endregion

    #region Stat Increasers
    public void ApplyHealth(int heathAmount)
    {
        characterDefinition.ApplyHealth(heathAmount);
    }
    public void ApplyMana(int manaAmount)
    {
        characterDefinition.ApplyMana(manaAmount);
    }
    public void GiveWealth(int wealthAmount)
    {
        characterDefinition.GiveWealth(wealthAmount);
    }
    public void IncreaseXP(int xp)
    {
        characterDefinition.GiveXP(xp);
    }
    public void IncreaseWealth(int gold)
    {
        characterDefinition.GiveMoney(gold);
    }
    #endregion
    #region Stat Reducers
    public void TakeDamage(int amount)
    {
        characterDefinition.TakeDamage(amount);
    }
    public void TakeMana(int amount)
    {
        characterDefinition.TakeMana(amount);
    }
    public void TakeWealth(int gold)
    {
        characterDefinition.GiveMoney(gold);
    }
    #endregion
    #region Weapon and Armor Change
    public void ChangeWeapon(ItemPickUp weaponPickUp) 
    {   //Basicamente cuando esta funcion hace que llame la funcion de abajo, pero si resulta negativo, te equipa el arma 
        if (!characterDefinition.UnEquipWeapon(weaponPickUp, charInv, characterWeaponSlot))
        {
            characterDefinition.EquipWeapon(weaponPickUp, charInv, characterWeaponSlot); //Se equipa otra arma
        }
    }

    public void ChangeArmor(ItemPickUp armorPick)
    {
        if(!characterDefinition.UnEquipArmor(armorPick, charInv))
        {
            characterDefinition.EquipArmor(armorPick, charInv);
            caca.SetActive(true);
        }
    }
    #endregion
    #region Reporters
    public int Getheath()
    {
        return characterDefinition.currentHeath; 
    }
    public Weapon GetCurrentWeapon()
    {
        if(characterDefinition.weapon != null)
        {
            return characterDefinition.weapon.itemDefinition.WeaponSpawnObject;
        }
        else
        {
            return null;
        }

    }
    public int GetDamage()
    {
        return characterDefinition.currentDamage;
    }
    public int GetDamageMagic()
    {
        return characterDefinition.currentDamageMagic;
    }
    public float GetResistence()
    {
        return characterDefinition.currentResistance;
    }
    #endregion

    #region Iniciatializers

    public void SetInitialHealth(int heath)
    {
        characterDefinition.maxHealth = heath;
        characterDefinition.currentHeath = heath;
    }
    public void SetInitialResistence(int resistance)
    {
        characterDefinition.baseResistance = resistance;
        characterDefinition.currentResistance = resistance;

    }
    public void SetInitialDamage(int Damage)
    {
        characterDefinition.baseDamage = Damage;
        characterDefinition.currentDamage = Damage;
    }


    #endregion

    #region StatusMachine

    public void ApplyBurn(int tick, int damage)
    {
        if (burnTickTimers.Count > 0)
        {
            burnTickTimers.Add(tick);
            StartCoroutine(Burn(damage));
        }
        else
        {
            burnTickTimers.Add(tick);
        }
    }

    IEnumerator Burn(int damage)
    {
        while (burnTickTimers.Count > 0)
        {
            for (int i = 0; i < burnTickTimers.Count; i++)
            {
                burnTickTimers[i]--;
            }
            TakeDamage(damage);
            burnTickTimers.RemoveAll(number => number == 0);
            yield return new WaitForSeconds(0.75f);
        }
    }


    IEnumerator AOEKeep()
    {
        iskeepDamage = true;
        float timer = 0f;
        while (timer < 0.75f)
        {
            timer += Time.deltaTime;
            
            yield return null;
        }
        iskeepDamage = false;
    }
    //Punch






    #endregion
}
