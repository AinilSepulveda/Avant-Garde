using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "QuestSystem", menuName = "Quest/CreateMision", order = 0)]
public class QuestSO : ScriptableObject
{
    [System.Serializable]
    public struct Mision
    {
        //struct = se puede manejar la struct el cuerpo del codigo no como object
        public string name;
        public string description;
        public int id;
        public QuestType typequest;

        [System.Serializable]
        public enum QuestType
        {
            Recolectar,
            Matar, 
            Entrega
        }

        //Tambien puede usarse este tipo de mision para hacer que el juego deba ir hasta cierot lugar o persona a la que debe llevar X item
        //(con esto logramos crear una mision de "Entrega" + "Recoleccion de items"

        [Header("Misiones de Recoleccion")]
        public bool stayItem;
        public bool sometypesItems;
        public List<ItemsArecoger> dataItems;

        [System.Serializable]
        public struct ItemsArecoger
        {
            public string Name;
            public int amount;
            public int itemID;
        }
        [Header("Misiones de Matar")]
        public int amountEnemies;
        public int enemyID;

        public bool DiferentesEnemigos;
        public int[] enemiesID;

        [Header("Recompensas")]
        public int gold;
        public int xp;
        public bool hasSpecialR;
        public SpecialRewards[] specials;

        [System.Serializable]
        public struct SpecialRewards
        {
            public string nombre;
            public GameObject reward; 
        }
    }

    public Mision[] misions;
    public float Destinyaprox = 1.5f;

}
