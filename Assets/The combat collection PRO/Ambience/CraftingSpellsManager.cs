using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSpellsManager : MonoBehaviour
{
    public Attacks attacks;
    public TypeSpells typeSpells;

    public int nivel;
    public int indexproyectile;
    public int indexprefabs;

    public List <GameObject> aoePrefabs;
    public List <Projectile> projectile;

    public HeroController hero;
    // Start is called before the first frame update
    void Start()
    {
        typeSpells = TypeSpells.Melee;
        nivel = 1;
        attacks = Attacks.Born; 
        
    }

    // Update is called once per frame
    void Update()
    {

    }

   public void SetTypeSpell(int type)
    {
        typeSpells = (TypeSpells)type;
    }
    public void SetIndexPrefabs(int index)
    {
        indexprefabs = index;
    }
    public void SetIndexProyectile(int index)
    {
        indexproyectile = index;
    }
    public void SetNivelSpell(int nivelSpells)
    {
        if (nivelSpells == 0)
            nivel = 1;

        if (nivelSpells == 1)
            nivel = 2;

        if (nivelSpells == 2)
            nivel = 3;

        if (nivelSpells == 3)
            nivel = 4;
    }
    public void SetAttacks(int type)
    {
        if(type == 0)
            attacks = Attacks.Born;

        if (type == 1)
            attacks = Attacks.freeze;

        if (type == 2)
            attacks = Attacks.Teletransportacion;

        if (type == 3)
            attacks = Attacks.Punch;
    }

    public void CreaftingSpell()
    {
        if(typeSpells == TypeSpells.Melee)
        {
            hero.Spell2 = CraftingMelee(attacks, nivel);

        }
        if (typeSpells == TypeSpells.AOE)
        {
            hero.Spell2 = CraftingAOE(attacks, nivel, aoePrefabs[indexprefabs]);
        }
        if (typeSpells == TypeSpells.Proyectile)
        {
            hero.Spell2 = CraftingProyectile(attacks, nivel, projectile[indexproyectile]);
        }

    }

    AttackDefinition CraftingMelee(Attacks attacks, int nivel)
    {
        AttackDefinition NewMeleeSpells = new AttackDefinition();
        NewMeleeSpells.VariableAttack = attacks;

        SettingLevelSpells(NewMeleeSpells, nivel);

        return NewMeleeSpells;
    }
    Spell CraftingProyectile(Attacks attacks, int nivel, Projectile projectile)
    {
        Spell NewMeleeSpells = new Spell();
         NewMeleeSpells.VariableAttack = attacks;
        NewMeleeSpells.projectileSpeed = Random.Range(5, 15) * nivel;
        NewMeleeSpells.projectileToFire = projectile;
        SettingLevelSpells(NewMeleeSpells, nivel);

        return NewMeleeSpells;
    }

    AoE CraftingAOE(Attacks attacks, int nivel, GameObject aoePrefabs)
    {
        AoE NewMeleeSpells = new AoE();
        NewMeleeSpells.VariableAttack = attacks;
        NewMeleeSpells.AOEprefab = aoePrefabs;
        NewMeleeSpells.Radius = Random.Range(5, 15) * nivel;
        SettingLevelSpells(NewMeleeSpells, nivel);

        return NewMeleeSpells;
    }

    void SettingLevelSpells(AttackDefinition spell, int nivel)
    {
        spell.Cooldown = Random.Range(5, 15) * nivel; 
        spell.Range = Random.Range(5, 15) * nivel;
        spell.costAmount = Random.Range(5, 15) * nivel;
             spell.minDamage = Random.Range(5, 15) * nivel;
        spell.maxDamage = Random.Range(5, 15) * nivel;
        spell.criticalMultiplier = Random.Range(5, 15) * nivel;
        spell.criticalChance = Random.Range(5, 15) * nivel;
    }
    //T CraftingSpell <T>(TypeSpells typeSpells, Attacks attacks, int nivel  )
    //{

    //    object NewSpells;
    //    return (T)NewSpells;
    //}
}
