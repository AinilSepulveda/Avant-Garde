using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public float patrolTime = 5; // time in seconds to wait before seeking a new patrol destination
    public float aggroRange = 10; // distance in scene units below which the NPC will increase speed and seek the player
    public Transform[] waypoints; // collection of waypoints which define a patrol area


    int index; // the current waypoint index in the waypoints array
    float speed, agentSpeed; // current agent speed and NavMeshAgent component speed
    Transform player; // reference to the player object transform

    public Events.EventMobDeath OnMobDeath;
    public MobyType mobyType;
    //Attack
    CharacterStats characterStatsNPC;
    private float timeOfLastAttack;
    //Spell 
    public Transform SpellHotSpot;
    //Quest
    public int IDenemy;

    Animator animator; // reference to the animator component
    NavMeshAgent agent; // reference to the NavMeshAgent

    [Header("Usalo para hacer un debugg del AgrroRange")]
    public bool showAgroo = true;

    private bool playerIsAlive;

    float timeSinceLastAttack;
    float timeSinceLastAttack2;
    bool attackOnCooldownSpecial;
    bool attackOnCooldownDefault;
    bool acechar;

    //Offset distancia 
   public float offsetDistanciaMin;
   public float offsetDistanciaMax;

    bool attackInRange;
    bool attackInRangeSpecial;

  //  bool isSpecial;

    void Awake()
    {
        characterStatsNPC = GetComponent<CharacterStats>();

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if (agent != null) { agentSpeed = agent.speed; }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        index = UnityEngine.Random.Range(0, waypoints.Length);

        MobManager mobManager = FindObjectOfType<MobManager>();
        if (mobManager != null)
            OnMobDeath.AddListener(mobManager.OnMobDeath);

        InvokeRepeating("Ticks", 0, 0.5f);

        if( waypoints.Length > 0)
        {
            InvokeRepeating("Patrol", UnityEngine.Random.Range(0, patrolTime), patrolTime);
        }
        playerIsAlive = true;
        timeOfLastAttack = float.MinValue; //Valor minimo de un Single

        player.gameObject.GetComponent<DestructedEvent>().IDied += PlayerDied;
    }

    private void PlayerDied()
    {
        playerIsAlive = false;
    }

    private void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

         timeSinceLastAttack = Time.time - timeOfLastAttack;
         timeSinceLastAttack2 = Time.time - timeSinceLastAttack2;
        attackOnCooldownSpecial = timeSinceLastAttack2 < characterStatsNPC.attackSpecial.Cooldown; //Cuando timeSinceLastAttack sea menor que attack.Cooldown es true;
        attackOnCooldownDefault = timeSinceLastAttack < characterStatsNPC.attackDefault.Cooldown; //Cuando timeSinceLastAttack sea menor que attack.Cooldown es true;


        if (mobyType == MobyType.ClawGoblin && mobyType == MobyType.TankGoblin)
        {
            agent.isStopped = attackOnCooldownSpecial;
            agent.isStopped = attackOnCooldownDefault;   //Para si AttackOnCooldown es true pero si false se mueve
        }
      

        if (playerIsAlive)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            attackInRange = distanceFromPlayer < characterStatsNPC.attackDefault.Range;
            attackInRangeSpecial = distanceFromPlayer < characterStatsNPC.attackSpecial.Range;

       //     isSpecial = isSpecial;
            if (!attackOnCooldownSpecial && attackInRangeSpecial)
            {
               // agent.isStopped = true;

                transform.LookAt(player.transform);
                timeSinceLastAttack2 = Time.time;

                animator.SetTrigger("Attack");

            }

            else if (!attackOnCooldownDefault && attackInRange)
            {
                agent.isStopped = true;

                transform.LookAt(player.transform);
                timeOfLastAttack = Time.time;


                
                animator.SetTrigger("Attack");
                
            }
            // Debug.Log(attackOnCooldown);


        }
    }
    public void Hit()
    {
        if (!playerIsAlive)
            return;
        //Ataque Default
        if (characterStatsNPC.attackDefault is Weapon && attackOnCooldownSpecial) //magia negra
        {
            ((Weapon)characterStatsNPC.attackDefault).ExecuteAttack(gameObject, player.gameObject);

        }
        else if (characterStatsNPC.attackSpecial is Weapon && !attackOnCooldownSpecial) //magia negra
        {
            ((Weapon)characterStatsNPC.attackSpecial).ExecuteAttack(gameObject, player.gameObject);

        }
        //Attack es AttackDefinicion class, weapon heredera de esta clase, y aq
        else if (characterStatsNPC.attackSpecial is Spell && !attackOnCooldownSpecial)
        {

            ((Spell)characterStatsNPC.attackSpecial).Cast(gameObject, SpellHotSpot.position, player.transform.position, LayerMask.NameToLayer("EnemySpells"));


        }




    }
    void Patrol()
    {
        index = index == waypoints.Length - 1 ? 0 : index + 1;
        //Si index es menor que waypoints.length, es 0, sino es index + 1
    }
    void Ticks()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        Vector3 direccion = (player.position - transform.position);
        direccion.Normalize();
        Vector3 destino = player.position - direccion * 2;

        if (player != null && distance <  aggroRange)
        {
            agent.isStopped = false;
            //ClawGoblin
            if (mobyType == MobyType.ClawGoblin)
            {


                agent.destination = destino;
                agent.speed = agentSpeed;

                if (distance < aggroRange/2)
                {                   
                    agent.destination = destino;
                    agent.speed = agentSpeed * 2;
                }


            }

            //Caster
            if (mobyType == MobyType.CasterGoblin)
            {
              

                if (distance < aggroRange / 2)
                {

                   // agent.speed = agentSpeed * 2;
                    agent.updatePosition = true;
                    agent.destination += LenardJones();


                }



            }
            //Tanke
            if (mobyType == MobyType.TankGoblin)
            {


                agent.destination = destino;
                agent.speed = agentSpeed;
            }

        }
        else
        {
            agent.destination = waypoints[index].position;
            agent.speed = agentSpeed / 2;
        }


    }

    public Vector3 LenardJones()
    {
        Vector3 r = agent.transform.position - player.transform.position;

        //U = -A/d^n + B/d^m

        float A = 700;
        float B = 250;
        float n = 3;
        float m = 2;
        float d = r.magnitude / 5;

        float U = -A / Mathf.Pow(d, n) + B / Mathf.Pow(d, m);


        //Seguridad para que no se venga tan arriba JODER, TIO, COÑO
        if (U < -10)
            U = -10;
        if (U > 10)
            U = 10;

        agent.transform.LookAt(player.transform.position);

        

        Vector3 lenardJones = agent.transform.forward * Mathf.Clamp(U, -8, 8) * Time.deltaTime * agent.speed * 2;

        Debug.Log("lenardJones" + lenardJones);

        return lenardJones;
    }


    private void OnDrawGizmos()
    {
        if (showAgroo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
            //para ver el rango <3
        }
        else { return; }
    }

    


    //private void OnDestroy()
    //{
    //    OnMobDeath.Invoke();
    //} 
}

[System.Serializable]
public enum MobyType
{
    CasterGoblin,
    ClawGoblin,
    TankGoblin
}
