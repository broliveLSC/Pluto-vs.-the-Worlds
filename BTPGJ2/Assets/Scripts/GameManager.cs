using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float speed = 1.0f;
    private bool paused = false;
    List<Timer> timers = new List<Timer>();

    List<Timer> toAdd = new List<Timer>();

    public bool debug = true;

    public Transform cam;

    public float savedSpeed = 1;

    public bool canRetry = true;
    public GameObject gameOverParent;
    public GameObject retryParent;

    public GameObject player;
    public BossMenu2 bosses;
    public Animator blackBars;

    public GameObject[] hud;
    private bool canPause = true;

    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Awake() // Start?
    {
        if (instance == null)
            instance = this;

        if (instance != this)
            Destroy(gameObject);

        if(RunSettings.instance != null)
        {
            speed = RunSettings.instance.gameSpeed;
            canRetry = RunSettings.instance.canRetry;
        }

        savedSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();

        pauseMenu.SetActive(paused);

        if(FindObjectOfType<PlayerController>() != null)
        {
            if (Input.GetButtonDown("Pause"))
            {
                if (speed != 0 && !paused && canPause)
                {
                    Pause();
                }

                else if (speed == 0 && paused && canPause)
                {
                    Resume();
                }
            }
        }
    }
    public void Pause()
    {
        speed = 0;
        paused = true;
        canPause = false;
        AddTimer(0.5f, AllowPause, "allowpause", false);

    }
    public void Resume()
    {
        speed = savedSpeed;
        paused = false;
        canPause = false;
        AddTimer(0.5f, AllowPause, "allowpause", false);
    }

    public void AllowPause()
    {
        canPause = true;
    }

    public void AddTimer(float duration, TimerEvent tE, string name, bool useSpeed)
    {
        Timer t = new Timer(duration, tE, name, useSpeed);
        //timers.Add(t);
        toAdd.Add(t);
        if(debug)
            Debug.Log("Added timer");
    }

    public void AddTimer(float duration, TimerEvent tE)
    {
        /*Timer t = new Timer(duration, tE);
        //timers.Add(t);
        toAdd.Add(t);
        if (debug)
            Debug.Log("Added timer");*/
        AddTimer(duration, tE, "", true);
    }

    public void UpdateTimer()
    {
        foreach(Timer t in timers)
        {
            if(!t.fired)
                t.Update();
        }

        for(int i = 0; i < timers.Count; i++)
        {
            if(timers[i].fired)
            {
                
                timers.RemoveAt(i);
            }
        }

        // any new timers get added now, so that it doesn't interfere with the loop
        foreach(Timer t in toAdd)
        {
            timers.Add(t);
        }

        toAdd.Clear();
    }

    public void ShakeCamera(string axis)
    {
        switch(axis)
        {
            case "h":
                cam.GetComponent<Animator>().SetTrigger("shake_h");
                break;

            case "v":
                cam.GetComponent<Animator>().SetTrigger("shake_v");
                break;

            case "hv":
                cam.GetComponent<Animator>().SetTrigger("shake_all");
                break;
        }
    }

    public void RestoreTime()
    {
        speed = savedSpeed;
    }

    public void StartGameOverProcess()
    {
        HideHUD();
        blackBars.SetTrigger("join");

        gameOverParent.SetActive(true);

        if(canRetry)
        {
            AddTimer(1.5f, EnableRetry);
        }

        else
        {
            AddTimer(1.5f, FadeToBlack);
            AddTimer(3.0f, GameOverMan);
        }
    }

    void HideHUD()
    {
        foreach (GameObject g in hud)
        {
            g.SetActive(false);
        }
    }

    void ShowHUD()
    {
        foreach (GameObject g in hud)
        {
            g.SetActive(true);
        }
    }

    public void HideEnemyHealthBar()
    {
        for(int i = 0; i < hud.Length; i++)
        {
            if(i != 0)
            {
                hud[i].SetActive(false);
            }
        }
    }

    public void ShowEnemyHealthBar()
    {
        for (int i = 0; i < hud.Length; i++)
        {
            if (i != 0)
            {
                hud[i].SetActive(true);
            }
        }
    }

    public void GameOverMan()
    {
        // go back to menu
        ReturnToTitle();
    }

    public void FadeToBlack()
    {
        FindObjectOfType<Fade>().active = true;
        FindObjectOfType<Fade>().mult = 1;
    }

    public void ReturnToTitle()
    {
        Debug.Log("Return to title");
        SceneManager.LoadScene("title");
    }

    void EnableRetry()
    {
        bosses.DestroyCurrentBoss();
        retryParent.SetActive(true);
    }

    public void RestartBattle()
    {
        ShowHUD();
        blackBars.SetTrigger("separate");
        gameOverParent.SetActive(false);
        retryParent.SetActive(false);

        player.SetActive(true);
        player.GetComponent<PlayerController>().Respawn();
        bosses.RespawnBoss();
    }
}

public delegate void TimerEvent();

public class Timer
{
    public float duration = 0;
    public float elapsed = 0;
    public bool fired = false;
    public TimerEvent onFire;
    public string timerName = "";
    bool affectedBySpeed = true;

    public Timer(float d, TimerEvent e)
    {
        duration = d;
        onFire = e;
    }

    public Timer(float d, TimerEvent e, string name)
    {
        duration = d;
        onFire = e;
        timerName = name;
    }

    public Timer(float d, TimerEvent e, string name, bool changeSpeed)
    {
        duration = d;
        onFire = e;
        timerName = name;
        affectedBySpeed = changeSpeed;
    }

    public void Update()
    {
        elapsed += Time.deltaTime * (affectedBySpeed ?  GameManager.instance.speed : 1);

        if(elapsed >= duration)
        {
            if(GameManager.instance.debug)
                Debug.Log("Timer fired");
            onFire();
            fired = true;
        }
    }

    public void PrintTimerProgress()
    {
        string log = "Timer " + timerName + " - " + elapsed + "/" + duration;
        Debug.Log(log);
    }
}
