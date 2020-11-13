using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flockFish : MonoBehaviour
{
    public float speed = 0.5f;
    float rotationSpeed = 4.0f;

    Vector3 averageHeading;
    Vector3 averagePosition;

    float neighbourDistance = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.5f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 5) < 1)
            ApplyRules();

        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = goblaFlock.allfish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = goblaFlock.goalPos;

        float dist;

        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);

                if(dist <= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if(dist < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    flockFish anotherFlock = go.GetComponent<flockFish>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        if(groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion quaternion = new Quaternion(transform.rotation.x,0, 0, transform.rotation.w);
                transform.rotation = Quaternion.Slerp(quaternion, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
