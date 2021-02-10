using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Weapon.asset", menuName = "Weapon/Attack", order =0)]
public class Weapon : AttackDefinition
{
    public Rigidbody weaponPreb;
  //  private Rigidbody rbody;


    public void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        if (defender == null)
            return;

        //Checkear que si el defensor esta a rango de ataque 
        if (Vector3.Distance(attacker.transform.position, defender.transform.position) > Range)
            return;
        //Checkear si el defensro esta al frente del player
        if (!attacker.transform.IsFacingTarget(defender.transform))
            return;
         
        //Si el ataque conecta
        var attackerStats = attacker.GetComponent<CharacterStats>();
        var defenderStats = defender.GetComponent<CharacterStats>();

        var attack = CreateAttack(attackerStats, defenderStats);

        var attackables = defender.GetComponentsInChildren(typeof(IAttackable));

            Debug.Log("WATE POR LONJI");

        foreach (IAttackable a in attackables)
        {
            SpecialAttack(VariableAttack, defender, attacker);
            a.OnAttack(attacker, attack);

        }
        
    }


}
