using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedForce : MonoBehaviour, IAttackable
{
    public float forceToadd;
    private Rigidbody rbody;


    public void OnAttack(GameObject attacker, Attack attack)
    {
        var forceDirection = transform.position - attacker.transform.position;
        forceDirection.y += 0.5f;
        forceDirection.Normalize();

        rbody.AddForce(forceDirection * forceToadd, ForceMode.Force);

        
    }

    private void Awake()
    {
        rbody = GetComponent<Rigidbody>(); 
    }
}
