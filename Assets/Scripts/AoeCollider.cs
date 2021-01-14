using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeCollider : MonoBehaviour
{
    public AttackDefinition AoE;
    public LayerMask layer;

    public CharacterStats CasterStast;

    public void OnTriggerStay(Collider other)
    {
       
        
            var collisionGo = other.gameObject;
        //check if we are ignorin the collision's later, if so move on to the next object
        if (Physics.GetIgnoreLayerCollision(layer, collisionGo.layer))
        { return; }


            //create attack and send ir to the attackable behaviors of our collision


            CharacterStats collisionStats;
            collisionStats = collisionGo.GetComponent<CharacterStats>();

        //Si el es false, se genera una vez el CreataAttack

         if (!collisionStats.iskeepDamage)
        {
            collisionStats.StartCoroutine("AOEKeep"); //Timer
            Attack attack = AoE.CreateAttack(CasterStast, collisionStats);
            AttackedTakeDamage attackables = collisionGo.GetComponent<AttackedTakeDamage>();



            AoE.SpecialAttack(AoE.VariableAttack, collisionGo, CasterStast.gameObject);
            attackables.OnAttack(CasterStast.gameObject, attack);
            
        }
         



        




    }
}
