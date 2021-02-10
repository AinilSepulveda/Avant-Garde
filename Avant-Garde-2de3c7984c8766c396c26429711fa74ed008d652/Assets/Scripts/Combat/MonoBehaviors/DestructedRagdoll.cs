using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedRagdoll : MonoBehaviour, IDestructible
{
    public Ragdoll ragdollObject;
    public float force;
    public float Lift;

    public void OnDestruction(GameObject destroyer)
    {
        var ragdoll = Instantiate(ragdollObject, transform.position, transform.rotation);
        Debug.Log("Hola" + " " + ragdoll.name);
        var vectorFromDestroy = transform.position - destroyer.transform.position;
        vectorFromDestroy.Normalize();
        vectorFromDestroy.y += Lift;

        ragdoll.ApplyForce(vectorFromDestroy * force); 
        //Esto hace que si se destruye crea el ragdoll y lo levanta y lo empuja hacia arriba
    }
}
