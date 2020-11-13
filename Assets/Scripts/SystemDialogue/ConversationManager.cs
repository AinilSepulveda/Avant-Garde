using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConversationManager : Singleton <ConversationManager>
{
    public Dialogue currentDialogue;

    public Queue<Dialogue> dialogues; //Queue es tipo fifo, primero entrar y es el primero en salir. es una lista

    private bool waitingAnswer = false;

    public Transform optionPanel; //es para el padre

    public GameObject conversationPanel; 

    public Button optionPrefabs;
    //Constructor

    public TMP_Text dialogueText;
    public TMP_Text nameText;
    protected ConversationManager()
    {

    }

    private void Start()
    {
        dialogues = new Queue<Dialogue>();
    }
    public void StartConversation(Conversation conversation)
    {
        conversationPanel.SetActive(true);

        nameText.text = conversation.nameAnnouncer;

      //  dialogues.Clear(); //Para no mostrar una conversacion que ya paso 

        foreach(Dialogue dialogue in conversation.dialogues)
        {
            dialogues.Enqueue(dialogue); //para meter en cola todas las conversaciones
        }

        NextDialogue();
    }

    public void NextDialogue()
    {
        if (!waitingAnswer)
        {
            if (dialogues.Count == 0)
            {
                EndConversation();
                return;
            }

            currentDialogue = dialogues.Dequeue(); //Desencolar los dialogos, por que ya estan ocurriendo

            DisplayDialogue();
        }
    }
    public void DisplayDialogue() //muestra los dialogos
    {
        ClearOptionsPanel(); //Se limpian las opciones

        dialogueText.text = currentDialogue.sentence;
      //  Debug.Log(currentDialogue.sentence);

        if (currentDialogue.options.Length != 0)
        {
            waitingAnswer = true;
            foreach (DialogueOptions options in currentDialogue.options)
            {   //Cada elemento del array en cada Dialogo-opciones
                Debug.Log(options.optionNumber + " - " + options.text);

                Button optionButton = GameObject.Instantiate<Button>(optionPrefabs, optionPanel.transform);
                optionButton.onClick.AddListener(delegate { SelectOptions(options.optionNumber); }); 
                //Se pone la funcion, de onclick de button, luego con delegate se le pasa la funcion "SelectOptions" 

                optionButton.GetComponentInChildren<TMP_Text>().text = options.text;

            }
        }

    }
    public void SelectOptions (int selected)
    {
        foreach (DialogueOptions options in currentDialogue.options)
        {
            if (options.optionNumber == selected)
            {
                EndConversation();
                if (options.response)
                {
                    StartConversation(options.response);
                    return;
                }
                else
                {
                    EndConversation();
                }
            }
        }
    }

    public void ClearOptionsPanel()
    {
        for (int i = 0; i < optionPanel.childCount; i++)
        {
            Destroy(optionPanel.GetChild(i).gameObject);
        }
    }

   public void EndConversation()
    {
        Debug.Log("End Conversation");
        waitingAnswer = false;
        dialogues.Clear();
        currentDialogue = null;
        conversationPanel.SetActive(false);
    }
}
