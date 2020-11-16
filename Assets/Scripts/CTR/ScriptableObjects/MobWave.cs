﻿using UnityEngine;

[CreateAssetMenu (fileName = "MobWave.asset", menuName ="Mob Wave")]
public class MobWave : ScriptableObject
{
    public int MobHealth;
    public int MobDamage;
    public int MobResistance;
    public int NumberOfMobs;
    public int PointsPerkill;
    public int WaveValue;

    public Waypoint[] waypoints;

}
