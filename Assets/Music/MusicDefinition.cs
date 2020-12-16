
using UnityEngine;

[System.Serializable]
public struct MusicDefinition
{
    public MusicEnum Music;
    public AudioClip clip;
}

public enum MusicEnum
{
    Ambient,
    Combat,
    CombatEndLoop,
    Wave,
    WaveEndLoop,
    FinalBoss,
    FinalBossLoop,
    Ambient2,
    Menu
}

