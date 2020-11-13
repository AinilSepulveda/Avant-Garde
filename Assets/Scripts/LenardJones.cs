using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LenardJones : MonoBehaviour
{
    public Transform objetivo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 r = objetivo.position - transform.position;

        //U = -A/d^n + B/d^m

        float A = 700;
        float B = 250;
        float n = 3;
        float m = 2;
        float d = r.magnitude/5;

        float U = -A / Mathf.Pow(d, n) + B / Mathf.Pow(d, m);


        //Seguridad para que no se venga tan arriba JODER, TIO, COÑO
        if (U < -10)
            U = -10;
        if (U > 10)
            U = 10;

        transform.LookAt(objetivo);

        //nos movemos
        transform.Translate(Vector3.forward * U * 5 *  Time.deltaTime);
    }
}
