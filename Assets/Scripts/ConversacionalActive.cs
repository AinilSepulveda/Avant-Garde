using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversacionalActive : MonoBehaviour
{
    public GameObject con_Manager;
    [SerializeField]
    Conversational Conversational;

    private void Awake()
    {
        Conversational = GetComponent<Conversational>();


    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            con_Manager.SetActive(true);
            Caca();
            Caca();
        }

    }

    public void Caca()
    {
        Conversational.TriggerConversation();
        Conversational.TriggerConversation();
    }
}


