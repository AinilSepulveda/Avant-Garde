using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPatos : MonoBehaviour
{
    public static ManagerPatos instance;

    public Vector3 posicion;
    public Vector3 direccion;
    public Vector3 combinado;

    [SerializeField]
    [Range(0, 100)]
    public float maxDireccion = 25;
    [SerializeField]
    [Range(0, 100)]
    public float maxCombinado = 25;

    private float tamanomundo = 100;

    [SerializeField]
    [Range(0, 200)]
    public float factorCohesion = 60;
    [SerializeField]
    [Range(0, 200)]
    public float factorSeparacion = 90;
    [SerializeField]
    [Range(0, 200)]
    public float factorAliniacion = 90;
    [Range(0, 200)]
    public float rangoCohesion = 20;
    [Range(0, 200)]
    public float rangoSeparacion = 6;
    [Range(0, 200)]
    public float rangoAlineacion = 10;

    public Flock_script[] agentes;
    // Start is called before the first frame update
    void Start()
    {
        agentes = FindObjectsOfType<Flock_script>();

        posicion = transform.position;

        direccion = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
