using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelStats : MonoBehaviour
{
    public CharacterStats character;
    public TextMeshProUGUI nivel;
    public TextMeshProUGUI heathpoints;
    public TextMeshProUGUI manapoints;
    public TextMeshProUGUI resistpoints;
    public TextMeshProUGUI damagepoints;

    private void OnEnable()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        mostrarcositas();
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
        mostrarcositas();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C) && gameObject.activeInHierarchy)
        //{
        //    this.gameObject.SetActive(false);
        //}
        //else
        //{
        //    this.gameObject.SetActive(true);
        //}
    }
    //private void OnBecameVisible()
    //{
    //    character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
    //    mostrarcositas();
    //}

    void mostrarcositas()
    {
        nivel.text = "Nivel: " + character.characterDefinition.charLevel.ToString();
        heathpoints.text = "Vida: " + character.characterDefinition.currentHeath.ToString() + "/" + character.characterDefinition.maxHealth.ToString();
        manapoints.text = "Mana: " + character.characterDefinition.currentMana.ToString() + "/" + character.characterDefinition.maxMana.ToString();
        damagepoints.text = "Resist: " + character.characterDefinition.baseResistance.ToString() + "/" + character.characterDefinition.currentResistance.ToString();
        resistpoints.text = "Daño: " + (character.characterDefinition.baseDamage + character.characterDefinition.currentDamage).ToString();
    }
}
