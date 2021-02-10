using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest  
{
    public string name;
    public bool complete = false;
    public int id;
    public QuestType type;

    [Header("Para Destino")]
    public GameObject destiny;

    [Header("Para enemigo")]
    public int idEnemy;
    public int totalamount;
    public int currentAmount;

    [Header("Para Items")]
    public bool stayitems;
    public List<QuestSO.Mision.ItemsArecoger> itemsArecogers = new List<QuestSO.Mision.ItemsArecoger>();

    public enum QuestType
    {
        Recoleccion,
        Matar, 
        Entrega
    }
}
