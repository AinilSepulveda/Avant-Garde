using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRompible : MonoBehaviour, IDestructible
{
    public GameObject fractured;
    public float breakforce;
    public UnityEngine.AI.NavMeshObstacle meshObstacle;
    // Start is called before the first frame update
    void Awake()
    {
        meshObstacle = GetComponent<UnityEngine.AI.NavMeshObstacle>();
    }

    // Update is called once per frame
   public void Breakthething()
   {
        meshObstacle.enabled = false;

        foreach (Rigidbody rb in fractured.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = (rb.transform.position - transform.position).normalized * -breakforce;
            rb.AddForce(force);
            rb.useGravity = true;
        }
        Destroy(this.gameObject, 3f);
   }

    public void OnDestruction(GameObject destroyer)
    {
        Breakthething();
    }
}
