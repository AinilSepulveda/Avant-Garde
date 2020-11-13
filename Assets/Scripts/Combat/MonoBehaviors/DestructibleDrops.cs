using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleDrops : MonoBehaviour, IDestructible
{
    private NPCController mob;
    private void Awake()
    {
        mob = GetComponent<NPCController>();
    }

    public void OnDestruction(GameObject destroyer)
    {
        mob.OnMobDeath.Invoke(mob.mobyType, transform.position);
    }
}
