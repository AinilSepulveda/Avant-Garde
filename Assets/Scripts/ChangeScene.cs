using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public string levelLoad;

    public Transform portalend;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<HeroController>().PortalEnd(portalend);
     //   other.transform.position = portalend.transform.position;
    }
}
