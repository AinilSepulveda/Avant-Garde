using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    public AttackDefinition demoAttack;
    public AttackDefinition Spell;
    public AttackDefinition Spell2;

    Animator animator; // reference to the animator component
    NavMeshAgent agent; // reference to the NavMeshAgent
   public CharacterStats stats;

    GameObject attackTarget;

    //Quitar al final
    public GameObject cofrefin;

    //Quest

    public QuestSO dataBase;
    public QuestTracker questTracker;

    public NPCController aliade;

    //Cooldown
   public float timercooldown;
    bool attackOnCooldown;
    Rigidbody rb;

    public Image Skill1;

    CharacterInventory characterInventory;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<CharacterStats>();
        questTracker = GetComponent<QuestTracker>();
        //   dataBase = GetComponent<QuestSO>();
        characterInventory = GetComponent<CharacterInventory>();
        aliade = GetComponent<NPCController>();
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            stats.characterDefinition.OnLevelUp.AddListener(GameManager.Instance.OnHeroLeveledUp);
            stats.characterDefinition.OnHeroDamaged.AddListener(GameManager.Instance.OnHeroDamaged);
            stats.characterDefinition.OnHeroGainedHealth.AddListener(GameManager.Instance.OnHeroGainedHealth);
            stats.characterDefinition.OnHeroGainedMana.AddListener(GameManager.Instance.OnHeroGainedMana);
            stats.characterDefinition.OnHeroDeath.AddListener(GameManager.Instance.OnHeroDied);
            stats.characterDefinition.OnHeroInitialized.AddListener(GameManager.Instance.OnHeroInit);
        }
        UIManager.Instance.UpdateUnitFrame(this);
        stats.characterDefinition.OnHeroInitialized.Invoke();
    }

    private void Update()
    {
        
        attackOnCooldown = timercooldown < Spell.Cooldown;

        animator.SetFloat("Speed", agent.velocity.magnitude);

        Skill1.fillAmount =  timercooldown / Spell.Cooldown; 


        if (Input.GetKeyDown(KeyCode.Q) && Spell is AoE && !attackOnCooldown)
        {
            timercooldown = 0;
            StompAttack();
            UIManager.Instance.UpdateUnitFrame(this);
         //   timercooldown = 0;
            
        }
        if (Input.GetKeyDown(KeyCode.W)  && !attackOnCooldown)
        {
            timercooldown = 0;
            AttackSpells2();
            UIManager.Instance.UpdateUnitFrame(this);
         //   timercooldown = 0;
            
        }
        if (attackOnCooldown)
            timercooldown += Time.deltaTime;


    }

    public void SetDestination(Vector3 destination)
    {
        StopAllCoroutines();
        agent.destination = destination;
        agent.isStopped = false;
      //  rb.isKinematic = true;
    }



    public void AttackTarget(GameObject target)
    {
        var weapon = stats.GetCurrentWeapon();

        if (weapon != null)
        {
            StopAllCoroutines();

            agent.isStopped = false;
            attackTarget = target;

            StartCoroutine(PursueAndAttackTarget());

        }
        else
        {
            StopAllCoroutines();

            agent.isStopped = false;
            attackTarget = target;

            StartCoroutine(PursueAndAttackTarget());

        }

    }
    private IEnumerator PursueAndAttackTarget()
    {
        agent.isStopped = false;
        var weapon = stats.GetCurrentWeapon();
        if (weapon != null)
        {
            while (Vector3.Distance(transform.position, attackTarget.transform.position) > weapon.Range)
            {
                agent.destination = attackTarget.transform.position;
                yield return null;
            }
            transform.LookAt(attackTarget.transform);
            animator.SetTrigger("Attack");
        }
        if (weapon == null)
        {
            while (Vector3.Distance(transform.position, attackTarget.transform.position) > demoAttack.Range)
            {
                //agent.destination = attackTarget.transform.position;
                yield return null;
            }
            transform.LookAt(attackTarget.transform);
            animator.SetTrigger("Attack");
        }
        agent.isStopped = true;



    }
    public void AttackSpellUno(GameObject target)
    {
        // var weapon = stats.GetCurrentWeapon();

        // var spellstats = Spell.Cooldown; 
        StopAllCoroutines();

        agent.isStopped = false;
        attackTarget = target;
        StartCoroutine(PursueAndStompAttack());



    }
    private IEnumerator PursueAndStompAttack()
    {
        agent.isStopped = false;


        while (Vector3.Distance(transform.position, attackTarget.transform.position) > Spell.Range)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;
        animator.SetTrigger("Stomp");
    }

    //Funciones de Animacion
    public void Hit()
    {
      
        /* if (attackTarget != null)
              stats.GetCurrentWeapon().ExecuteAttack(gameObject, attackTarget); */
        var weapon = stats.GetCurrentWeapon();
        if (weapon is Weapon)
        {
            stats.GetCurrentWeapon().ExecuteAttack(gameObject, attackTarget);

            //Aqui se pasa la mision de matar, si no funciona poner esto en otro lado.
            //   questTracker.MuerteEnemigo(attackTarget.GetComponent<NPCController>().IDenemy);

        }

    }

    public void StompAttack()
    {
        if (stats.characterDefinition.currentMana > Spell.costAmount)
        {

          
                ((AoE)Spell).Fire(gameObject, gameObject.transform.position, LayerMask.NameToLayer("PlayerSpells"));

          

            stats.TakeMana(Spell.costAmount);
        }

        else
        {

        }

    }

    public void AttackSpells2()
    {
        if (Spell2 is AoE)
        {
            if (stats.characterDefinition.currentMana > Spell2.costAmount)
            {
                Debug.Log("Es un ataque AOE");

                ((AoE)Spell2).Fire(gameObject, gameObject.transform.position, LayerMask.NameToLayer("PlayerSpells"));



                stats.TakeMana(Spell.costAmount);
            }
        }

        else if (Spell2 is AttackDefinition)
        {
            stats.GetCurrentWeapon().ExecuteAttack(gameObject, attackTarget);
        }

        else if (Spell2 is Spell)
        {
            if (stats.characterDefinition.currentMana > Spell2.costAmount)
            {
                ((Spell)Spell2).Cast(this.gameObject, stats.characterWeaponSlot.transform.position, attackTarget.gameObject.transform.position, LayerMask.NameToLayer("PlayerSpells"));

                Debug.Log("Es un ataque Spell");
            }
        }
    }

    public int GetCurrentHealth()
    {
        return stats.characterDefinition.currentHeath;
    }
    public int GetCurrentMana()
    {
        return stats.characterDefinition.currentMana;
    }
    public int GetMaxHealth()
    {
        return stats.characterDefinition.maxHealth;
    }
    public int GetMaxMana()
    {
        return stats.characterDefinition.maxMana;
    }
    public int GetCurrentLevel()
    {
        return stats.characterDefinition.charLevel;
    }


    #region Callback

    public void OnMobKilled(int exp)
    {
        stats.IncreaseXP(exp);

    }
    public void OnWaveCompleted(int exp)
    {
        stats.IncreaseXP(exp);
    }
    public void OnOutOfWave()
    {
        cofrefin.SetActive(true);
        MusicManager.Instance.PlaySoundEffect(MusicEnum.Ambient2, 1);
    }
    public void OnOutOfWave2()
    {
        Debug.Log("hola");
    }




    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destiny"))
        {
            other.GetComponent<DestinyQuest>().reached = true;
            questTracker.ActualizarQuest(other.GetComponent<DestinyQuest>().IDQuest, Quest.QuestType.Entrega);
        }
    }

    public void PortalEnd(Transform portaltransform)
    {

        agent.transform.position = portaltransform.position;
        StartCoroutine(EnabledAgent());
    }

    IEnumerator EnabledAgent()
    {
        agent.isStopped = true;
        agent.enabled = false;
        yield return new WaitForFixedUpdate();
        agent.enabled = true;
        agent.isStopped = false;
        //   SetDestination(portaltransform.position);
    }

}
    
