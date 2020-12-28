using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class MobManager : MonoBehaviour
{
    public GameObject[] Mobs;
    

    private int currentWaveIndex = 0;
    [SerializeField]
    private int activeMobs;

    public List<GameObject> activeMobss = new List<GameObject>();
    //drops
    public List<DropTable> dropTables;

    public MobWave[] Waves;
    public Spawnpoint[] spawnpoints;
    //Eventos
    public Events.EventIntegerEvent OnMobKilled;
    public Events.EventIntegerEvent OnWaveCompleted;
    public UnityEvent OnOutOfWave;
    //ALAScript
    alasScript alas;
    bool isActiveWaves;

    public GameObject cacaV2;
    public GameObject cacaV3;
  //  public GameObject cacaV3;
    // Start is called before the first frame update
    void Start()
    {
        alas = GetComponent<alasScript>();
        //Spawn() y //spawnpoints
        // SpawnWave();
        //spawnpoints =  alasScripts[Alas].spawnpoints;

        //spawnpoints = FindObjectsOfType<Spawnpoint>();
    }

    public void SpawnWave()
    {
        isActiveWaves = true;
        spawnpoints = FindObjectsOfType<Spawnpoint>();
        MusicManager.Instance.PlaySoundEffect(MusicEnum.Wave, 1);


        if (Waves.Length < currentWaveIndex)
        {
            MusicManager.Instance.PlaySoundEffect(MusicEnum.WaveEndLoop, 1, true);
            Debug.Log("No hay mas waves");
            OnOutOfWave.Invoke();
         //   alas.spawnpoints = null;
            cacaV2.SetActive(true);
            cacaV3.SetActive(false);
            return;
        }

        if(currentWaveIndex > 0)
        {
            SoundManager.Instance.PlaySoundEffect(SoundEffect.NextWave);
            //musica de waves

            cacaV2.SetActive(true);
            cacaV3.SetActive(false);
        }



        for (int i = 0; i <= Waves[currentWaveIndex].NumberOfMobs - 1; i++)
        {
            
            Spawnpoint spawnpoint = selectRamdomSpawnpoint();
            GameObject mobs = Instantiate(selectRamdonMob(), spawnpoint.transform.position, Quaternion.identity);

            activeMobss.Add(mobs);



           // Mobs[Waves[currentWaveIndex].NumberOfMobs] = mobs; 
            mobs.GetComponent<NPCController>().waypoints = findClosestWayPoints(mobs.transform);
            //Para tomar los stats de MobWaves
            CharacterStats stats = mobs.GetComponent<CharacterStats>();
            MobWave currentWave = Waves[currentWaveIndex];

            stats.SetInitialDamage(currentWave.MobDamage);
            stats.SetInitialHealth(currentWave.MobHealth);
            stats.SetInitialResistence(currentWave.MobResistance);
         //   currentWaveIndex--;
        }
        activeMobs = activeMobss.Count;
        currentWaveIndex = Waves.Length;
    }
    public void OnMobDeath(MobyType mobyType, Vector3 position)
    {
        SoundManager.Instance.PlaySoundEffect(SoundEffect.MobDeath);
        MobWave currentWave = Waves[currentWaveIndex];

        OnMobKilled.Invoke(currentWave.PointsPerkill);

        SpawnDrops(mobyType, position);
        Debug.LogFormat("{0} killed at {1}", mobyType, position);

        if (isActiveWaves)
        {
        activeMobss.RemoveAt(activeMobs); 
        activeMobs -= 1;

        }

        if (activeMobss.Count <= 0 && isActiveWaves == true)
        {
            Debug.Log("Next Wave");
            OnWaveCompleted.Invoke(currentWave.WaveValue);
            currentWaveIndex += 1;
            SpawnWave();
        }
    }

    //Selectcionar un mobs
    private GameObject selectRamdonMob()
    {
        int mobIndex = Random.Range(0, Mobs.Length);
        return Mobs[mobIndex];
    }


    //void que retorna un Spawnpoint -> spawnpoints ramdom mente; 
    private Spawnpoint selectRamdomSpawnpoint()
    {
        int pointIndex = Random.Range(0, spawnpoints.Length -1);
     //   alasScripts[alanumber].spawnpoints = spawnpoints;
        return spawnpoints[pointIndex];
    }

    //Find to Waypoints
    private Transform[] findClosestWayPoints(Transform mobTranform)
    {
        Vector3 mobPosition = mobTranform.position;
        //Encuentra el objeto que tenga Waypoint -> los ordena -> cada w por y la distancia de mob a w -> va al más cerca 
        Waypoint closestPoint = FindObjectsOfType<Waypoint>().OrderBy(w => (w.transform.position - mobPosition).sqrMagnitude).First();
        //parent = Waypoints transform parent (por que esto es un script) 
        Transform parent = closestPoint.transform.parent;
        //alltransforms = todos waypoints transform
        Transform[] alltransforms = parent.GetComponentsInChildren<Transform>();
        //Lista de linq se consulta una lista - t == alltransforms - t != parent -> transforms == t 
        var transforms = 
            from t in alltransforms
            where t != parent select t;
        //retorna transforms
        return transforms.ToArray();
    }

    //SpawnDrops
    private void SpawnDrops(MobyType mobyType, Vector3 position)
    {
        ItemPickUp_SO item = GetDrop(mobyType);
        Debug.Log(item.name);
        if (item != null)
        {
            Instantiate(item.itemSpawnObject, position, Quaternion.identity);

        }
    }


    private ItemPickUp_SO GetDrop(MobyType mobyType)
    {
        DropTable mobDrops = dropTables.Find(mt => mt.mobyType == mobyType);

        Debug.Log(mobDrops.name);

        if (mobDrops == null)
            return null;

        mobDrops.drops.OrderBy(d => d.DropChance);

        foreach (DropDefinition dropDef in mobDrops.drops)
        {
            bool shouldDrop = Random.value < dropDef.DropChance;
            Debug.Log("desmedrar:" + shouldDrop);
            if (shouldDrop)
                    return dropDef.Drop;
            

        }

        return null;
    }
    public  int Mobkilled(int cacaa)
    {
        MobWave currentWave = Waves[currentWaveIndex];
        cacaa = currentWave.PointsPerkill;
        return cacaa;
    }
}
