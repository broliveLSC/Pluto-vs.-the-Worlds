using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTitle : MonoBehaviour
{
    public bool sun = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(2, DestroySelf, "", false);
        if (!sun)
        {
            
            if (GameSettings.instance.musicOn)
            {
                MusicManager.instance.StartBattleMusic();
            }
        }

        else
        {
            GameManager.instance.speed = 0;

            if (GameSettings.instance.musicOn)
            {
                MusicManager.instance.PauseMusic();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroySelf()
    {
        if(sun)
        {
            //restore time
            GameManager.instance.speed = GameManager.instance.savedSpeed;

            if (GameSettings.instance.musicOn)
            {
                MusicManager.instance.ResumeMusic();
            }
        }
        Destroy(gameObject);
    }
}
