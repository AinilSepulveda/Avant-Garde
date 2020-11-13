using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell.asset", menuName = "Attack/Spell")]
public class Spell : AttackDefinition
{
    [Space(5)]
    [Header("Proyectile")]

    public Projectile projectileToFire;
    public float projectileSpeed;

    public void Cast(GameObject Caster, Vector3 HotSpot, Vector3 target, int Layer)
    {
        //fire Projectile at Target
        Projectile projectile = Instantiate(projectileToFire, HotSpot, Quaternion.identity);
        projectile.Fire(Caster, target, projectileSpeed, Range);

        //Set Projectile's Collision layer
        projectile.gameObject.layer = Layer;

        //Listen to projectile collided event
        projectile.ProjectileCollided += OnProjectileCollided;
    }

    private void OnProjectileCollided(GameObject Caster, GameObject Target)
    {
        //Attack landed on target, create attack an attack the target 

        //Make sure both the caster and target are still alive
        if (Caster == null || Target == null)
            return;

        //create the attack
        var casterStats = Caster.GetComponent<CharacterStats>();
        var TargetStats = Target.GetComponent<CharacterStats>();

        var attack = CreateAttack(casterStats, TargetStats);

        //send attack to all attackable behaviors of the target 
        var attackables = Target.GetComponentsInChildren(typeof(IAttackable));

        foreach (IAttackable a in attackables)
        {
            SpecialAttack(VariableAttack, Target, Caster);
            a.OnAttack(Caster, attack);
        }
    }
}
