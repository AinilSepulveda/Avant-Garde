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
        NewMeleeSpells.name = "NewMeleeSpells";

        return NewMeleeSpells;
    }
    Spell CraftingProyectile(Attacks attacks, int nivel, Projectile projectile)
    {
        Spell NewProyectileSpells = new Spell();
         NewProyectileSpells.VariableAttack = attacks;
        NewProyectileSpells.projectileSpeed = Random.Range(5, 15) * nivel;
        NewProyectileSpells.projectileToFire = projectile;
        SettingLevelSpells(NewProyectileSpells, nivel);
        NewProyectileSpells.name = "NewProyectileSpells";

        return NewProyectileSpells;
    }

    AoE CraftingAOE(Attacks attacks, int nivel, GameObject aoePrefabs)
    {
        AoE NewAoESpells = new AoE();
        NewAoESpells.VariableAttack = attacks;
        NewAoESpells.AOEprefab = aoePrefabs;
        NewAoESpells.Radius = Random.Range(5, 15) * nivel;
        SettingLevelSpells(NewAoESpells, nivel);
        NewAoESpells.name = "NewAoESpells";
        NewAoESpells.TimeInScene = 5;

        return NewAoESpells;
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
