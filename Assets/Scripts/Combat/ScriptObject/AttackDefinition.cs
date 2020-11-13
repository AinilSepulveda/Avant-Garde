
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack.asset", menuName = "Attack/BaseAttack")]
public class AttackDefinition : ScriptableObject
{
    [Header("characteristics")]
    public float Cooldown;
    public int costAmount;
    public float Range;
    public float minDamage;
    public float maxDamage;
    public float criticalMultiplier;
    public float criticalChange;
    [Space(5)]
    [Header("Type Special Attacks")]
    public Attacks VariableAttack;

    [Header("Force")]
    public float forceToadd;
    [Space(5)]
    [Header("Is Speel o physis damage")]
    public bool isMagic;

    //typeAttack

    private Rigidbody rbody;
    private GameObject defender;
    private GameObject attacker;

    public bool Iskeep;
    public float TimeInScene;
    public List<int> TickTimers = new List<int>();

    public Attack CreateAttack(CharacterStats wielderStats, CharacterStats defenderStats) //wielder portador
    {
        float coreDamage;
        if (isMagic)
        {
            coreDamage = wielderStats.GetDamage();
        }

        else 
        {
            coreDamage = wielderStats.GetDamageMagic();
        }
            
            //El daño recibido
            coreDamage += Random.Range(minDamage, maxDamage); //un rango de daño

            bool isCritical = Random.value < criticalChange; //un random, tira un numero random de 1, 0 si es un 1 = true 0 = false
            if (isCritical)
            {
                coreDamage *= criticalMultiplier;
            }    
            if (defenderStats != null)
            {
                coreDamage -= defenderStats.GetResistence();
            }



        return new Attack((int)coreDamage, isCritical); 

    }

    public void SpecialAttack(Attacks typeAttack, GameObject defender, GameObject attacker )
    {
        Rigidbody rbody = defender.GetComponent<Rigidbody>();

        UnityEngine.AI.NavMeshAgent defnavMesh = defender.GetComponent<UnityEngine.AI.NavMeshAgent>();
        UnityEngine.AI.NavMeshAgent attackernavMesh = attacker.GetComponent<UnityEngine.AI.NavMeshAgent>();

        if ((typeAttack & Attacks.Punch) != 0)
        {
        
            //  rbody.isKinematic = defnavMesh.enabled = false;
            var forceDirection = defender.transform.position - attacker.transform.position;
            forceDirection.y += 0.5f;
            forceDirection.Normalize();

            rbody.AddForce(forceDirection * forceToadd, ForceMode.Impulse);



            defnavMesh.enabled = float.MinValue < Time.deltaTime;
            defnavMesh.transform.position = rbody.transform.position;
            rbody.rotation = Quaternion.identity;


        }
        if ((typeAttack & Attacks.freeze) != 0)
        {



        }
        if ((typeAttack & Attacks.Born) != 0)
        {
            CharacterStats chDefender = defender.GetComponent<CharacterStats>();
            chDefender.ApplyBurn(5, (int)minDamage);
        }
        if ((typeAttack & Attacks.Teletransportacion) != 0)
        {
            Vector3 r = defender.transform.position - attacker.transform.position;

            //U = -A/d^n + B/d^m

            float A = 700;
            float B = 250;
            float n = 3;
            float m = 2;
            float d = r.magnitude / 5;

            float U = -A / Mathf.Pow(d, n) + B / Mathf.Pow(d, m);


            //Seguridad para que no se venga tan arriba JODER, TIO, COÑO
            if (U < -8)
                U = -8;
            if (U > 8)
                U = 8;

            attacker.transform.LookAt(defender.transform.position);



            Vector3 lenardJones = attacker.transform.forward * Mathf.Clamp(U, -8, 8) * Time.deltaTime * attackernavMesh.speed;

            Debug.Log(lenardJones);

            attackernavMesh.updatePosition = true;
            attackernavMesh.destination += attackernavMesh.nextPosition;
            attackernavMesh.nextPosition = attacker.transform.position += lenardJones * 10;
        }
        if ((typeAttack & Attacks.none) != 0)
        {
            //Iskeep;

        }
    }
}


[System.Flags]
public enum Attacks
{
    Punch = 4, 
    Born = 2,
    freeze = 1,
    none = 0,
    Teletransportacion = 8
}
