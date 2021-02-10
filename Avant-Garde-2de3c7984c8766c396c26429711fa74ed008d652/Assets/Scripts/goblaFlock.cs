using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblaFlock : MonoBehaviour
{
    public GameObject fishPrefabs;
    public static int tankSize = 5;

    public static int numberFish = 10;
    public static GameObject[] allfish = new GameObject[numberFish];

    //podria ser los vector, los waypoints
    public static Vector3 goalPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberFish; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-tankSize, tankSize), transform.position.y, Random.Range(-tankSize, tankSize));

            allfish[i] = (GameObject)Instantiate(fishPrefabs, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0,10000) < 50)
        {
            goalPos = new Vector3(Random.Range(-tankSize, tankSize), transform.position.y, Random.Range(-tankSize, tankSize));
        }
    }
}
