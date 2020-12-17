using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    public List<MusicDefinition> Musics;
    public AudioSource MusicFXSource;
    public AudioSource Music2;

    private bool firstMusicSourceIsplaying;

    public void PlaySoundEffect(MusicEnum music, float transitionTime = 1.0f, bool IsCrossFade = false)
    {
        AudioClip effect = Musics.Find(sfx => sfx.Music == music).clip;

        MusicFXSource.Play();
        if(IsCrossFade == false)
        StartCoroutine(MusicFades(MusicFXSource, effect, transitionTime));

        if (IsCrossFade)
        {
            AudioSource activeSource = (firstMusicSourceIsplaying) ? MusicFXSource : Music2;
            AudioSource newSource = (firstMusicSourceIsplaying) ? Music2  : MusicFXSource;

            firstMusicSourceIsplaying = !firstMusicSourceIsplaying;

            newSource.clip = effect;
            newSource.Play();

            StartCoroutine(MusicCrossFade(MusicFXSource, Music2, transitionTime));
        }

    }

    IEnumerator MusicFades (AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        if (!activeSource.isPlaying)
            activeSource.Play();

        float t = 0.0f;

        //Fade out
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (1 - (t / transitionTime));
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        //Fade in
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = ((t / transitionTime));
            yield return null;
        }
    }

    IEnumerator MusicCrossFade(AudioSource original, AudioSource newSource, float transitionTime)
    {
        float t = 0.0f;

        for (t = 0; t <= transitionTime; t+= Time.deltaTime )
        {
            original.volume = (1 - (t / transitionTime));
            newSource.volume = (t / transitionTime);
            yield return null;
        }

        original.Stop();
    }
}
