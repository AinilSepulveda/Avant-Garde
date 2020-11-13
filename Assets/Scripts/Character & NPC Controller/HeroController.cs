using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroController : MonoBehaviour
{
    public AttackDefinition demoAttack;
    public AttackDefinition Spell;
    public AttackDefinition Spell2;

    Animator animator; // reference to the animator component
    NavMeshAgent agent; // reference to the NavMeshAgent
    CharacterStats stats;

    GameObject attackTarget;

    //Quitar al final
    public GameObject cofrefin;

    //Quest

    public QuestSO dataBase;
    public QuestTracker questTracker;
    //  public QuestTrackerPanel TrackerPanel;
    //  public questPanel Panel;





    CharacterInventory characterInventory;
    void Awake()
    {

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<CharacterStats>();
        questTracker = GetComponent<QuestTracker>();
        //   dataBase = GetComponent<QuestSO>();
        characterInventory = GetComponent<CharacterInventory>();

    }

    private void Start()
    {
        stats.characterDefinition.OnLevelUp.AddListener(GameManager.Instance.OnHeroLeveledUp);
        stats.characterDefinition.OnHeroDamaged.AddListener(GameManager.Instance.OnHeroDamaged);
        stats.characterDefinition.OnHeroGainedHealth.AddListener(GameManager.Instance.OnHeroGainedHealth);
        stats.characterDefinition.OnHeroDeath.AddListener(GameManager.Instance.OnHeroDied);
        stats.characterDefinition.OnHeroInitialized.AddListener(GameManager.Instance.OnHeroInit);

        stats.characterDefinition.OnHeroInitialized.Invoke();
    }

    private void Update()
    {


        animator.SetFloat("Speed", agent.velocity.magnitude);


        if (Input.GetKeyDown(KeyCode.Q) && Spell is AoE)
        {
            StompAttack();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //TrackerPanel.ActualizarBotones();
            //TrackerPanel.ActualizarDescripcionesConInfo(-1);
            //TrackerPanel.gameObject.SetActive(!TrackerPanel.gameObject.activeSelf);
        }

    }

    public void SetDestination(Vector3 destination)
    {
        StopAllCoroutines();
        agent.destination = destination;
        agent.isStopped = false;
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
            //transform.LookAt(attackTarget.transform);
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

    public int GetCurrentHealth()
    {
        return stats.characterDefinition.currentHeath;
    }
    public int GetMaxHealth()
    {
        return stats.characterDefinition.maxHealth;
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

    //public void Rewards(Quest quest)
    //{
    //    // TrackerPanel.ActualizarBotones();

    //    stats.IncreaseXP(dataBase.misions[quest.id].xp);
    //    stats.IncreaseWealth(dataBase.misions[quest.id].gold);

    //    //Panel.accept_button.gameObject.SetActive(false);
    //    //Panel.deny_button.gameObject.SetActive(false);

    //    if (dataBase.misions[quest.id].hasSpecialR) //Recompensa especial
    //    {
    //        if (dataBase.misions[quest.id].specials.Length > 1)
    //        {
    //            string s = "�Bien hecho!, Completaste " + dataBase.misions[quest.id].name + ", como recompensa has obtenido Oro(" + dataBase.misions[quest.id].gold
    //            + "), " + "Experiencia(" + dataBase.misions[quest.id].xp + ") y los siguientes items ";

    //            for (int i = 0; i < dataBase.misions[quest.id].specials.Length; i++)
    //            {
    //                s = string.Format("{0} {1}", s, dataBase.misions[quest.id].specials[i].nombre);
    //                //{0} Representa s, {1} presenta dataBase.misions[quest.id].specials[i].nombre
    //            }
    //            //  Panel.ActualizarPanel(quest.name, s);
    //        }
    //        else
    //        {
    //            //Panel.ActualizarPanel("�Bien hecho!, Completaste " + dataBase.misions[quest.id].name + ", como recompensa has obtenido Oro(" + dataBase.misions[quest.id].gold
    //            //    + "), " + "Experiencia(" + dataBase.misions[quest.id].xp);
    //        }
    //    }
    //    else
    //    {
    //        //Panel.ActualizarPanel("�Bien hecho!, Completaste " + dataBase.misions[quest.id].name + ", como recompensa has obtenido Oro(" + dataBase.misions[quest.id].gold
    //        //    + "), " + "Experiencia(" + dataBase.misions[quest.id].xp);
    //    }

    //    if (quest.stayitems)
    //    {
    //        List<InventoryEntry> its = new List<InventoryEntry>();
    //        int cantidadaAeliminar = quest.itemsArecogers[0].amount;

    //        for (int i = 0; i < characterInventory.itemsInInventory.Count; i++)
    //        {
    //            if (characterInventory.itemsInInventory.ContainsKey(quest.itemsArecogers[0].itemID) && cantidadaAeliminar > 0)
    //            {
    //                its.Add(characterInventory.itemsInInventory[quest.itemsArecogers[0].itemID]);

    //                characterInventory.TriggerItemUse(quest.itemsArecogers[0].itemID);

    //                cantidadaAeliminar--;
    //            }
    //        }
    //    }
    //}
}
    
