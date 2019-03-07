using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    AudioSource sauce;
    public AudioClip bgmMenu;
    public AudioClip bgmBattle;

    bool fadeOut = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        sauce = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(sauce.isPlaying && !GameSettings.instance.musicOn)
        {
            //if(sauce.isPlaying)
                sauce.Pause();
            sauce.volume = 0;
        }

        if(!sauce.isPlaying && GameSettings.instance.musicOn)
        {
            sauce.Play();
            sauce.volume = 0.15f;
        }

        if(fadeOut)
        {
            sauce.volume -= 0.1f * Time.deltaTime;
            if (sauce.volume <= 0)
            {
                fadeOut = false;
                sauce.Stop();
                sauce.clip = null;

                sauce.volume = 0.15f;
            }
        }
    }

    public void FadeOut()
    {
        if (!GameSettings.instance.musicOn)
            return;

        //Debug.Log("DO FADE OUT");
        fadeOut = true;
        
    }

    public void StartBattleMusic()
    {
        sauce.clip = bgmBattle;
        ResumeMusic();
    }

    public void PauseMusic()
    {
        sauce.Pause();
    }

    public void ResumeMusic()
    {
        sauce.Play();
    }
}
