using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour, ISpawns
{
    public ItemPickUp_SO[] itemDefinitions;

    private int whichToSpawn = 0; //Cuantos items pueden a salir
    private int totalSpawnWeight = 0; //cuanto peso
    private int chosen = 0; //Que item va a salir. Este elige, el otro es como un index del array


    //ISpaws
    public Rigidbody itemSpawned { get; set; }
    public Renderer itemMaterial { get; set; }
    public ItemPickUp itemType { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        foreach (ItemPickUp_SO ip  in itemDefinitions)
        {
            totalSpawnWeight += ip.spawnChangeWeight; 
        }
    }

    // Update is called once per frame
   public void CreateSpawn()
    {
        //Spawn with weighted possibilities
        chosen = Random.Range(0, totalSpawnWeight);

        foreach (ItemPickUp_SO ip in itemDefinitions)
        {
            whichToSpawn += ip.spawnChangeWeight;
            if (whichToSpawn >= chosen)
            {
                itemSpawned = Instantiate(ip.itemSpawnObject, transform.position, Quaternion.identity);

                itemMaterial = itemSpawned.GetComponent<Renderer>();
                if (itemMaterial != null)
                    itemMaterial.material = ip.itemMaterial;

                itemType = itemSpawned.GetComponent<ItemPickUp>();
                itemType.itemDefinition = ip;
                break; //por si salen mas de 2 items tiene que tener un break porque estamos en un foreach
                    
            }
        }
    } 
}

