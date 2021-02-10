using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTracker : MonoBehaviour
{
    public QuestSO db;
    //Quest comencadas (aun incompletas/ en proceso) 
    public List<Quest> activeQuest = new List<Quest>();

    //Quest terminadas( se completaron los requisitos) 
    public List<Quest> finishedQuest = new List<Quest>();

    //Quest ya terminadas (se cobre la recompensa) 
    public List<Quest> completedQuest = new List<Quest>();

    //Lista que almacena los NPCS ques nos dieron las quests 
    [HideInInspector] public List<QuestGiver> rewardes = new List<QuestGiver>();

    public void MuerteEnemigo(int enemy_ID)
    {
        if(activeQuest.Count > 0) //Si hay una quest Activa - del tipo Matar
        {
            for (int i = 0; i < activeQuest.Count; i++) 
            {
                if (activeQuest[i].idEnemy == enemy_ID) //
                {
                    activeQuest[i].currentAmount++;
                    if (activeQuest[i].currentAmount < activeQuest[i].totalamount)
                    {
                        print("Cantidad restante de enemigos" + (activeQuest[i].totalamount - activeQuest[i].currentAmount));

                    }
                    ActualizarQuest(activeQuest[i].id, activeQuest[i].type);
                    break;
                }
            }
        }
    }

    public void ActualizarQuest(int id_quest, Quest.QuestType type, int ? cantItems = null) 
    {
        //int ? cantItems = null dentro del void es una variable que es opcional 


        //expresion lamb, que busca una condicion, en este caso es Si X tiene una X.id y es igual id_quest, val pasa referenciar a ese objeto
        var val = activeQuest.Find(x => x.id == id_quest);



        if(type == Quest.QuestType.Matar)
        {
            if(val.currentAmount >= val.totalamount)
            {
                Debug.LogWarning("Quest: " + db.misions[val.id].name + " Complete");
                val.complete = true;
            }
            else
            {
                print("Aun no has completado la quest: " + db.misions[val.id].name);
            }
        }

        if (type == Quest.QuestType.Entrega)
        {
            if (val.destiny.GetComponent<DestinyQuest>().reached)
            {
                Debug.LogWarning("Quest: " + db.misions[val.id].name + " Complete!");
                val.complete = true;
            }
            else
            {
                print("Aun no has completado la quest: " + db.misions[val.id].name);
            }
        }

        if (type == Quest.QuestType.Recoleccion)
        {
            foreach (var item in val.itemsArecogers)
            {
                if (cantItems != null)
                {
                    if (cantItems == item.amount)
                    {
                        Debug.LogWarning("Quest: " + db.misions[val.id].name + " Complete!");
                        val.complete = true;
                    }
                    else
                    {
                        print("Aun no has completado la quest: " + db.misions[val.id].name + "Te faltan: " + (item.amount - cantItems));
                    }
                }
            }
        }
    }

    public void VerificarItem(int item_ID)
    {
        Quest q = null;

        if(activeQuest.Count > 0)
        { //Si exite la quest -> y que dentro de ella exista objetos para completar los items -> Y que se la misma Id de objetos 
            if(activeQuest.Exists(x => x.itemsArecogers.Exists(a => a.itemID == item_ID)))
            {
                q = activeQuest.Find(x => x.itemsArecogers.Exists(a => a.itemID == item_ID));
            }
            else
            {
                q = null;
                return;
            }
        }
        for (int i = 0; i < activeQuest.Count; i++)
        {
            if(q.itemsArecogers[0].itemID == item_ID && activeQuest[i].id == q.id)
            {
                int cantidad = DiscriminaciondeItems(db.misions[activeQuest[i].id].dataItems[0].itemID);
                ActualizarQuest(activeQuest[i].id, activeQuest[i].type, cantidad);
                q = null;
                break;
            }
        }
    }

    private int DiscriminaciondeItems(int itemID)
    {
        int itemsMatched = 0;

        foreach (var item in GetComponent<CharacterInventory>().itemsInInventory)
        {
            if(item.Key == itemID)
            {
                itemsMatched++;
                Debug.Log("Hola esto aunmento" + itemsMatched);
            }
        }
        return itemsMatched;
    }
}
