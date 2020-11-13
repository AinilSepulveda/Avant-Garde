using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock_script : MonoBehaviour
{
    private Vector3 posicion;
    private Vector3 direccion;
    private Vector3 combinado;

    [SerializeField]
    [Range (0, 100)]
    private float maxDireccion = 25;
    [SerializeField]
    [Range(0, 100)]
    private float maxCombinado = 25;

    private float tamanomundo = 100;

    [SerializeField]
    [Range(0, 200)]
    private float factorCohesion = 60;
    [SerializeField]
    [Range(0, 200)]
    private float factorSeparacion = 90;
    [SerializeField]
    [Range(0, 200)]
    private float factorAliniacion = 90;
    [Range(0, 200)]
    public  float rangoCohesion = 20;
    [Range(0, 200)]
    public  float rangoSeparacion = 6;
    [Range(0, 200)]
    public float rangoAlineacion = 10;

    private Flock_script[] agentes;
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
        combinado = factorCohesion * Cohesion() + factorSeparacion * Separacion() + factorAliniacion * Alineacion();

        combinado = Vector3.ClampMagnitude(combinado, maxCombinado);

        //Se calcula la direccion
        direccion = direccion + combinado * Time.deltaTime;
        direccion = Vector3.ClampMagnitude(direccion, maxDireccion);

        posicion = posicion + direccion  * Time.deltaTime;

        if (posicion.x > tamanomundo)
            posicion.x = -tamanomundo;
        else if (posicion.x < -tamanomundo)
            posicion.x = tamanomundo;

        if (posicion.z > tamanomundo)
            posicion.z = -tamanomundo;
        else if (posicion.z < -tamanomundo)
            posicion.z = tamanomundo;

        transform.position = posicion;



        //if (direccion.magnitude > 0)
            transform.LookAt(posicion + direccion);
    }

    private Vector3 Cohesion()
    {
        Vector3 posPromCohesion = new Vector3();
        Vector3 vectorCohesion = new Vector3();

        List<Flock_script> vecinos = ObtieneVecinos(rangoCohesion);

        if (vecinos.Count == 0)
            return posPromCohesion;

        //Para obtener la posicion promedio de cada vecino
        foreach (Flock_script vecino in vecinos)
        {
            posPromCohesion += vecino.posicion;
           

        }

        posPromCohesion /= vecinos.Count;

        //obtenemos la posicion de nosotros hacia la posicion promedio
        vectorCohesion = posPromCohesion - this.posicion;

        vectorCohesion.Normalize();

        return vectorCohesion;
    }

    private List<Flock_script> ObtieneVecinos(float rango)
    {
        List<Flock_script> vecinos = new List<Flock_script>();

        float d = 0;

        foreach (Flock_script a in agentes)
        {
            //obtenemos la distancia
            d = Vector3.Magnitude(transform.position - a.transform.position);

            //verificamos que no sea uno mismo
            if (d == 0)
                continue;

            //verificamos que estamos dentro del rango de vision
            if (d <= rango)
                vecinos.Add(a);
        }

        return vecinos;
    }

    private Vector3 Separacion()
    {
        Vector3 vectorSeparacion = new Vector3();

        List<Flock_script> vecinos = ObtieneVecinos(rangoSeparacion);

        if (vecinos.Count == 0)
            return vectorSeparacion;

        //Para obtener la posicion promedio de cada vecino
        foreach (Flock_script vecino in vecinos)
        {
            Vector3 vecinoAgente = this.posicion - vecino.posicion;
            //La fuerza es inversamente proporcional a la distancia

            vectorSeparacion += vecinoAgente.normalized / vecinoAgente.magnitude;
            

        }

        vectorSeparacion.Normalize();

        return vectorSeparacion;
    }

    private Vector3 Alineacion()
    {
        Vector3 vectorAlineacion = new Vector3();

        List<Flock_script> vecinos = ObtieneVecinos(rangoSeparacion);

        if (vecinos.Count == 0)
            return vectorAlineacion;

        //Para obtener la posicion promedio de cada vecino
        foreach (Flock_script vecino in vecinos)
        {
            vectorAlineacion += vecino.direccion / vecinos.Count;
        }

        vectorAlineacion.Normalize();

        return vectorAlineacion;
    }
}
