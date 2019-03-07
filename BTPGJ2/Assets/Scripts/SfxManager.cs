using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager instance;
    public AudioSource[] sources;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        foreach (AudioSource sauce in sources)
        {
            if (!GameSettings.instance.sfxOn)
            {
                sauce.volume = 0;
            }

            if (GameSettings.instance.sfxOn)
            {
                sauce.volume = 0.3f;
            }
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        PlaySFX(clip, false, false);
    }

    public void PlaySFX(AudioClip clip, bool interrupt)
    {
        PlaySFX(clip, interrupt, false);
    }

    public void PlaySFX(AudioClip clip, bool interrupt, bool looping)
    {
        AudioSource toUse = null;
        foreach (AudioSource sauce in sources)
        {
            if(!sauce.isPlaying)
            {
                toUse = sauce;
                break;
            }
        }

        if(toUse == null && interrupt)
        {
            AudioSource furthestAlong = null;
            float percentage = 0;
            foreach(AudioSource sauce in sources)
            {
                if(sauce.time > percentage)
                {
                    furthestAlong = sauce;
                    percentage = sauce.time;
                }
            }

            toUse = furthestAlong;
        }

        
        if (toUse != null)
        {
            if (looping)
                toUse.loop = true;
            else
                toUse.loop = false;

            toUse.clip = clip;
            toUse.Play();
        }

    }

    public void StopSFX(AudioClip clip)
    {
        foreach(AudioSource s in sources)
        {
            if (s.clip == clip)
                s.Stop();
        }
    }
}