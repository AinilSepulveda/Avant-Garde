using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aoe.Asset", menuName = "Attack/AOE")]
public class AoE : AttackDefinition
{
    [Space(5)]
    [Header("AoE")]
    public float Radius;
    public GameObject AOEprefab;


    public void Fire(GameObject Caster, Vector3 position, int layer, bool ? keep = false)
    {


        var aoe = Instantiate(AOEprefab, position, Quaternion.identity);
        aoe.GetComponent<AoeCollider>().layer = layer;
        aoe.GetComponent<AoeCollider>().CasterStast = Caster.GetComponent<CharacterStats>();
        Destroy(aoe, TimeInScene);
        //get objects insede our aoe radius
     //   var collidedObjects = Physics.OverlapSphere(position, Radius);

        //SphereCollider caca = AOEprefab.GetComponent<SphereCollider>();
        //caca.radius = Radius;







        //loop through all collided objects
        //foreach (var collision in collidedObjects)
        //    {
        //        var collisionGo = collision.gameObject;
        //         //check if we are ignorin the collision's later, if so move on to the next object
        //        if (Physics.GetIgnoreLayerCollision(layer, collisionGo.layer))
        //            continue;


        //            //create attack and send ir to the attackable behaviors of our collision

        //        var casterStats = Caster.GetComponent<CharacterStats>();
        //        var collisionStats = collisionGo.GetComponent<CharacterStats>();



        //        var attack = CreateAttack(casterStats, collisionStats);

        //        var attackables = collisionGo.GetComponentsInChildren(typeof(IAttackable));

        //        foreach (IAttackable a in attackables)
        //        {
        //            SpecialAttack(VariableAttack, collisionGo, Caster);
        //            a.OnAttack(Caster, attack);
        //        }

        //}
        
    }
}
