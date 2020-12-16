using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelStats : MonoBehaviour
{
    public CharacterStats character;
    public GameObject buttons;
    public TextMeshProUGUI nivel;
    public TextMeshProUGUI heathpoints;
    public TextMeshProUGUI manapoints;
    public TextMeshProUGUI resistpoints;
    public TextMeshProUGUI damagepoints;

    private void OnEnable()
    {
       // character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        mostrarcositas();
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
        mostrarcositas();
    }

    private void Update()
    {
        if(character.characterDefinition.Levelpoints > 0 && buttons.activeInHierarchy == false)
        {
            buttons.SetActive(true);
        }
        else if (character.characterDefinition.Levelpoints <= 0 && buttons.activeInHierarchy == true)
        {
            buttons.SetActive(false);
        }

    }
    //private void OnBecameVisible()
    //{
    //    character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
    //    mostrarcositas();
    //}

    public void mostrarcositas()
    {
        nivel.text = "Nivel: " + character.characterDefinition.charLevel.ToString();
        heathpoints.text = "Vida: " + character.characterDefinition.currentHeath.ToString() + "/" + character.characterDefinition.maxHealth.ToString();
        manapoints.text = "Mana: " + character.characterDefinition.currentMana.ToString() + "/" + character.characterDefinition.maxMana.ToString();
        damagepoints.text = "Daño: " + (character.characterDefinition.baseDamage + character.characterDefinition.currentDamage).ToString();
        resistpoints.text = "Resist: "  + character.characterDefinition.baseResistance.ToString() + "/" + character.characterDefinition.currentResistance.ToString();
    }
}
