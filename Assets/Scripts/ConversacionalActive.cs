using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversacionalActive : MonoBehaviour
{
    public GameObject con_Manager;
    public TMPro.TextMeshProUGUI text_mision;
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

            if(text_mision != null)
            {
                text_mision.gameObject.SetActive(true);
                text_mision.text = "Consigue un casco con filtro";
            }
        }

    }

    public void Caca()
    {
        Conversational.TriggerConversation();

    }
}


