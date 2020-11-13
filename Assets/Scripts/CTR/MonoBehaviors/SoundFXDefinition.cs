
using UnityEngine;

[System.Serializable]
public struct SoundFXDefinition 
{
    public SoundEffect effect;
    public AudioClip clip;
}

public enum SoundEffect
{
    HeroHit,
    LevelUp,
    MobDamage,
    MobDeath, 
    NextWave
}
