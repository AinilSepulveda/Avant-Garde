using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStats : MonoBehaviour
{
    public CharacterStats_SO character_Template;
    public CharacterStats_SO characterDefinition;
    public CharacterInventory charInv;
    [Header ("WeaponSlot")]
    public GameObject characterWeaponSlot;
    [Header("ArmorSlots")]
    public GameObject characterArmorHeadSlot; //cabeza
    public GameObject characterArmorBootsSlot; //pies
    public GameObject characterArmorChestsSlot; //cinturon  
    public GameObject characterArmorHandsSlot; //Manos
    public GameObject characterArmorLegsSlot; //piernas

    //quitar eso despues
    public GameObject caca;


    //Status Effect Manager
    [SerializeField]
    List<int> burnTickTimers = new List<int>();
    public bool iskeepDamage;

    Rigidbody rbody;
    UnityEngine.AI.NavMeshAgent agent;

    //Ataques
    public AttackDefinition attackDefault;
    public AttackDefinition attackSpecial;

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

        rbody = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Start()
   { 

        if (characterDefinition.isHero) //no si controlamos la wea. 
        {
            characterDefinition.maxHealth = 350 ;
            characterDefinition.currentHeath = 350;

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
        else if (characterDefinition.GodMode)
        {
            characterDefinition.maxHealth = 150000;
            characterDefinition.currentHeath = 150000;

            characterDefinition.maxMana = 25000;
            characterDefinition.currentMana = 25000;

            characterDefinition.maxWealth = 500;
            characterDefinition.currentWealth = 0;

            characterDefinition.baseResistance = 0;
            characterDefinition.currentResistance = 0;

            characterDefinition.maxEncumbrance = 50f; //El peso
            characterDefinition.currentEncumbrance = 0f;

            characterDefinition.charExperience = 0;
            characterDefinition.charLevel = 1;

            characterDefinition.baseDamage = 5000;
            characterDefinition.currentDamage = 1;
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
        Debug.Log("Chanje wapom");
        if (!characterDefinition.UnEquipWeapon(weaponPickUp, charInv, characterWeaponSlot))
        {
            characterDefinition.EquipWeapon(weaponPickUp, charInv, characterWeaponSlot); //Se equipa otra arma
        }

    }

    public void ChangeArmor(ItemPickUp armorPick)
    {
        switch (armorPick.itemDefinition.ItemArmorSubType)
        {
            case ItemArmorSubType.Head:
                if (!characterDefinition.UnEquipArmor(armorPick, charInv, characterArmorHeadSlot))
                {
                    characterDefinition.EquipArmor(armorPick, charInv, characterArmorHeadSlot);
                    // caca.SetActive(true);
                }
                break;            case ItemArmorSubType.Boots:
                if (!characterDefinition.UnEquipArmor(armorPick, charInv, characterArmorBootsSlot))
                {
                    characterDefinition.EquipArmor(armorPick, charInv, characterArmorBootsSlot);
                    // caca.SetActive(true);
                }
                break;            case ItemArmorSubType.Chest:
                if (!characterDefinition.UnEquipArmor(armorPick, charInv, characterArmorChestsSlot))
                {
                    characterDefinition.EquipArmor(armorPick, charInv, characterArmorChestsSlot);
                    // caca.SetActive(true);
                }
                break;            case ItemArmorSubType.Hands:
                if (!characterDefinition.UnEquipArmor(armorPick, charInv, characterArmorHandsSlot))
                {
                    characterDefinition.EquipArmor(armorPick, charInv, characterArmorHandsSlot);
                    // caca.SetActive(true);
                }
                break;            case ItemArmorSubType.Legs:
                if (!characterDefinition.UnEquipArmor(armorPick, charInv, characterArmorLegsSlot))
                {
                    characterDefinition.EquipArmor(armorPick, charInv, characterArmorLegsSlot);
                    // caca.SetActive(true);
                }
                break;          
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
    public void Empujar(float Distancia, float Velocidad)
    {
        StartCoroutine(empujar(Distancia, Velocidad));
    }

    IEnumerator empujar(float Distancia, float Velocidad)
    {
        float timer = 0f;
        agent.enabled = false;
        float Tiempo = Distancia / Velocidad * Time.deltaTime ;

        Debug.Log(Tiempo + " " + this.gameObject.name);
        while (timer < Tiempo)
        {
            timer += Time.deltaTime;

            yield return null;
        }
        agent.enabled = true;
        rbody.isKinematic = true;
        agent.transform.position = rbody.transform.position;
        rbody.rotation = Quaternion.identity;



    }





    #endregion
}
