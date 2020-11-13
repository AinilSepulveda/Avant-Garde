using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject caster;
    private float speed;
    private float range;
    private Vector3 travelDirection;

    private float distanceTraveled;

    public event Action<GameObject, GameObject> ProjectileCollided;
    // Start is called before the first frame update
   public void Fire(GameObject Caster, Vector3 target, float Speed, float Range)
    {
        caster = Caster;
        speed = Speed;
        range = Range;

        //Calculate travel Direction
        travelDirection = target - transform.position;
        travelDirection.y = 0f;
        travelDirection.Normalize();

        //initialize distance traveled
        distanceTraveled = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //move this projectile through space
        float distanceToTravel = speed * Time.deltaTime;

        transform.Translate(travelDirection * distanceToTravel);
        //Check to see if we traveled too far, if so destroy this projectile
        distanceTraveled += distanceToTravel;
        if (distanceTraveled > range)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Raise an event
        if(ProjectileCollided != null)
        {
            ProjectileCollided(caster, other.gameObject);
        }
        Destroy(gameObject);
    }
}
