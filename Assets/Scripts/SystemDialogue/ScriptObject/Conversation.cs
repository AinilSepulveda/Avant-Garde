using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConversation", menuName = "ConversationSystem/Conversation", order = 0)]
[System.Serializable]
public class Conversation : ScriptableObject
{
    public string nameAnnouncer;

    public Dialogue[] dialogues;
}
[System.Serializable]
public class Dialogue
{
    [TextArea(3, 10)] //hace un cuadro de escribir "sentence" sea mas grande 3 largo y 10 de ancho
    public string sentence;

    public DialogueOptions[] options; //Deberia tener un limite OJO
}
[System.Serializable]
public class DialogueOptions
{
    [TextArea(3, 10)]
    public string text;

    public int optionNumber;

    public Conversation response; 
}

//Una conversacion tiene que tener una serie de dialogos, y cada linea pdorìa contener opciones de respuesta, que va iniciar otra conversacion. 
