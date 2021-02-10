using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversacionalActive : MonoBehaviour
{
    public GameObject con_Manager;
    Conversational Conversational;

    private void Awake()
    {
        Conversational = GetComponent<Conversational>();
    }

    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
            Caca();
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //        StartCoroutine(cacaa());
    //}

    // Update is called once per frame
    public void Caca()
    {
        con_Manager.SetActive(true);
        Conversational.TriggerConversation();
    }
}

//    public IEnumerator cacaa()
//    {
//        yield return new WaitForSeconds(5);
//        con_Manager.SetActive(false);

//    }
//}
