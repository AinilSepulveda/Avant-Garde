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
    bool attackOnCooldown;
    bool acechar;

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
         attackOnCooldown = timeSinceLastAttack < characterStatsNPC.attack.Cooldown; //Cuando timeSinceLastAttack sea menor que attack.Cooldown es true;


        if (mobyType == MobyType.ClawGoblin && mobyType == MobyType.TankGoblin)
        {
            agent.isStopped = attackOnCooldown;  //Para si AttackOnCooldown es true pero si false se mueve
        }
      

        if (playerIsAlive)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            bool attackInRange = distanceFromPlayer < characterStatsNPC.attack.Range;

           // Debug.Log(attackOnCooldown);
            if (!attackOnCooldown && attackInRange)
            {
                agent.isStopped = true;

                transform.LookAt(player.transform);
                timeOfLastAttack = Time.time;
                animator.SetTrigger("Attack");
            }
        }
    }
    public void Hit()
    {
        if (!playerIsAlive)
            return;
        if(characterStatsNPC.attack is Weapon) //magia negra
        {
            ((Weapon)characterStatsNPC.attack).ExecuteAttack(gameObject, player.gameObject);


        }
        //Attack es AttackDefinicion class, weapon heredera de esta clase, y aq
        else if (characterStatsNPC.attack is Spell)
        {

            ((Spell)characterStatsNPC.attack).Cast(gameObject, SpellHotSpot.position, player.transform.position, LayerMask.NameToLayer("EnemySpells"));
            

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



        if (player != null && distance <  aggroRange)
        {
            agent.isStopped = false;
            if (mobyType == MobyType.ClawGoblin)
            {


                agent.destination = player.position;
                agent.speed = agentSpeed;

                if (distance < aggroRange/2)
                {                   
                    agent.destination = player.position;
                    agent.speed = agentSpeed * 2;
                }


            }
            if (mobyType == MobyType.CasterGoblin)
            {
              

                if (distance < aggroRange / 2)
                {
  

                    agent.updatePosition = true;
                    agent.destination += agent.nextPosition;
                    //agent.nextPosition = transform.position += LenardJonesMov();

                }



            }

            if (mobyType == MobyType.TankGoblin)
            {


                agent.destination = player.position;
                agent.speed = agentSpeed;
            }

        }
        else
        {
            agent.destination = waypoints[index].position;
            agent.speed = agentSpeed / 2;
        }


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
