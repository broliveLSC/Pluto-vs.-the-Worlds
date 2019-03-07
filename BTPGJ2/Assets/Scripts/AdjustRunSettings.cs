using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AdjustRunSettings : MonoBehaviour
{
    public int page = 1;

    int index = 0;

    public Transform[] p1Points;
    public Transform[] p2Points;

    public Transform page1;
    public Transform page2;

    float prevH = 0;
    float prevV = 0;

    float hTime = 0;
    float vTime = 0;

    bool canH = true;
    bool canV = true;
    float confirmTime = 0;

    public Text[] options;
    public bool chooseEnemies = false; // choose or random
    public bool healAfterFight = true; // heal or no heal
    public bool canRetry = false; // retry or game over
    public Color confirmColor;

    public Slider[] sliders;
    public string fightScene;

    public RunSettings rs;

    public AudioClip click;
    public AudioClip move;

    DelayedAction currentAction;
    float delayTime = 0;
    bool startCounting = false;
    float elapsed = 0;

    public Transform back;
    public Transform next;
    public Transform restore;

    public Text playerHealth;
    public Text enemyHealth;
    public Text gameSpeed;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<Fade>().alpha = 1;
        FadeFromBlack();
    }

    public void FadeFromBlack()
    {
        FindObjectOfType<Fade>().active = true;
        FindObjectOfType<Fade>().mult = -1;
    }

    public void FadeToBlack()
    {
        FindObjectOfType<Fade>().active = true;
        FindObjectOfType<Fade>().mult = 1;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePage1();
        UpdatePage2();
        /*switch(page)
        {
            case 1:
                UpdatePage1();
                break;
            case 2:
                UpdatePage2();
                break;
        }*/
        page1.gameObject.SetActive(page == 1);
        page2.gameObject.SetActive(page == 2);

        confirmTime += Time.deltaTime;
        rs.canRetry = canRetry;
        rs.chooseEnemies = chooseEnemies;
        rs.healAfterFight = healAfterFight;

        if(startCounting)
        {
            elapsed += Time.deltaTime;
            if(elapsed > delayTime)
            {
                currentAction();
            }
        }

        next.GetComponent<Text>().text = (page == 1 ? "NEXT" : "START GAME");
        next.localScale = (index == 3 ? new Vector3(1.25f, 1.25f, 1) : new Vector3(1, 1, 1));
        restore.localScale = (index == 4 ? new Vector3(1.25f, 1.25f, 1) : new Vector3(1, 1, 1));
        back.localScale = (index == 5 ? new Vector3(1.25f, 1.25f, 1) : new Vector3(1,1,1));
    }

    

    void UpdatePage1()
    {
        options[0].color = chooseEnemies ? confirmColor : Color.gray;
        options[1].color = !chooseEnemies ? confirmColor : Color.gray;
        options[2].color = healAfterFight ? confirmColor : Color.gray;
        options[3].color = !healAfterFight ? confirmColor : Color.gray;
        options[4].color = canRetry ? confirmColor : Color.gray;
        options[5].color = !canRetry ? confirmColor : Color.gray;

        options[0].transform.localScale = (chooseEnemies ? new Vector3(1.5f, 1.5f, 1) : new Vector3(1f, 1f, 1)) *
            (index == 0 ? 1.1f : 1);
        options[1].transform.localScale = (!chooseEnemies ? new Vector3(1.5f, 1.5f, 1) : new Vector3(1f, 1f, 1)) *
            (index == 0 ? 1.1f : 1);
        options[2].transform.localScale = (healAfterFight ? new Vector3(1.5f, 1.5f, 1) : new Vector3(1.1f, 1.1f, 1)) *
            (index == 1 ? 1.1f : 1);
        options[3].transform.localScale = (!healAfterFight ? new Vector3(1.5f, 1.5f, 1) : new Vector3(1f, 1f, 1)) *
            (index == 1 ? 1.1f : 1);
        options[4].transform.localScale = (canRetry ? new Vector3(1.5f, 1.5f, 1) : new Vector3(1f, 1f, 1)) *
            (index == 2 ? 1.1f : 1);
        options[5].transform.localScale = (!canRetry ? new Vector3(1.5f, 1.5f, 1) : new Vector3(1f, 1f, 1)) *
            (index == 2 ? 1.1f : 1);

    }

    void UpdatePage2()
    {
        playerHealth.transform.localScale = (index == 0 ? new Vector3(1.25f, 1.25f, 1) : new Vector3(1, 1, 1));
        playerHealth.color = (index == 0 ? Color.white : Color.gray);
        enemyHealth.transform.localScale = (index == 1 ? new Vector3(1.25f, 1.25f, 1) : new Vector3(1, 1, 1));
        enemyHealth.color = (index == 1 ? Color.white : Color.gray);
        gameSpeed.transform.localScale = (index == 2 ? new Vector3(1.25f, 1.25f, 1) : new Vector3(1, 1, 1));
        gameSpeed.color = (index == 2 ? Color.white : Color.gray);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (!canV)
        {
            if (v > -0.1f && v < 0.1f)
                canV = true;

            if (prevV == v)
            {
                vTime += Time.deltaTime;

                if (vTime > 0.25f)
                {
                    canV = true;
                    vTime = 0;
                }
            }

            else
                vTime = 0;
        }

        if (!canH)
        {
            if (h > -0.1f && h < 0.1f)
                canH = true;

            if (prevH == h)
            {
                hTime += Time.deltaTime;

                if (hTime > 0.25f)
                {
                    canH = true;
                    hTime = 0;
                }
            }

            else
                hTime = 0;
        }

        if (canV && v < -0.1f)
        {
            // move down
            Down();

            
        }

        if (canV && v > 0.1f)
        {
            // move up
            Up();

            
        }

        if (canH)
        {
            if (h > 0.1f)
            {
                Right();
                //IncrementSlider();
            }

            else if (h < -0.1f)
            {
                Left();
                //DecrementSlider();
            }
        }

        if(Input.GetButtonDown("Jump")/* || Input.GetButtonDown("Fire1")*/)
        {
            Confirm();
        }

        if (page == 1)
            transform.position = p1Points[index].position;

        if (page == 2)
            transform.position = p2Points[index].position;


        prevH = h;
        prevV = v;
    }

    void Left()
    {
        if (page == 1)
        {
            if (index == 0)
            {
                chooseEnemies = !chooseEnemies;
                SfxManager.instance.PlaySFX(click);
            }
            else if (index == 1)
            {
                healAfterFight = !healAfterFight;
                SfxManager.instance.PlaySFX(click);
            }
            else if (index == 2)
            {
                canRetry = !canRetry;
                SfxManager.instance.PlaySFX(click);
            }

            else if (index == 3)
            {
                index = 5;
                SfxManager.instance.PlaySFX(move);
            }

            else if (index == 4)
            {
                index = 3;
                SfxManager.instance.PlaySFX(move);
            }
            
        }

        if (page == 2)
        {
            if(index == 0 || index == 1 || index == 2)
            {
                DecrementSlider();
            }

            else if (index == 3)
            {
                index = 5;
                SfxManager.instance.PlaySFX(move);
            }

            else if(index == 4)
            {
                index = 3;
                SfxManager.instance.PlaySFX(move);
            }
        }

        canH = false;
    }

    void Right()
    {
        if (page == 1)
        {
            if (index == 0)
            {
                chooseEnemies = !chooseEnemies;
                SfxManager.instance.PlaySFX(click);
            }
            else if (index == 1)
            {
                healAfterFight = !healAfterFight;
                SfxManager.instance.PlaySFX(click);
            }
            else if (index == 2)
            {
                canRetry = !canRetry;
                SfxManager.instance.PlaySFX(click);
            }

            else if (index == 3)
            {
                index = 4;
                SfxManager.instance.PlaySFX(move);
            }

            else if (index == 5)
            {
                index = 3;
                SfxManager.instance.PlaySFX(move);
            }
        }

        if (page == 2)
        {
            if (index == 0 || index == 1 || index == 2)
            {
                IncrementSlider();
            }

            else if (index == 3)
            {
                index = 4;
                SfxManager.instance.PlaySFX(move);
            }

            else if (index == 5)
            {
                index = 3;
                SfxManager.instance.PlaySFX(move);
            }
        }

        canH = false;
    }

    void Up()
    {
        //if (page == 2)
        {
            if (index < 3)
            {
                index -= 1;


                if (index < 0)
                    index = 0;
            }
            else
            {
                index = 2;
            }
        }

        SfxManager.instance.PlaySFX(move);

        canV = false;
    }

    void Down()
    {
        //if(page == 2)
        {
            if(index < 3)
            {
                canV = false;
                index += 1;

                /*if (index > 2)
                    index = 0;*/
            }
        }

        SfxManager.instance.PlaySFX(move);

        canV = false;
    }

    void IncrementSlider()
    {
        sliders[index].value += 1;
        canH = false;
        SfxManager.instance.PlaySFX(click);
    }

    void DecrementSlider()
    {
        sliders[index].value -= 1;
        canH = false;
        SfxManager.instance.PlaySFX(click);
    }

    public void SetIndex(int i)
    {
        index = i;
        SfxManager.instance.PlaySFX(move);
    }

    void RestoreDefaults()
    {
        rs.playerHealthSlider.value = 8;
        rs.gameSpeedSlider.value = 3;
        rs.enemyHealthSlider.value = 3;

        chooseEnemies = true;
        healAfterFight = true;
        canRetry = false;

        SfxManager.instance.PlaySFX(click);
    }

    void Back()
    {
        if(page == 2)
        {
            page = 1;
        }

        else
        {
            FadeToBlack();
            //return to title
            DelayAction(GoToTitleScreen, 2);
        }

        SfxManager.instance.PlaySFX(click);
    }

    void GoToTitleScreen()
    {
        SceneManager.LoadScene("title");
    }

    void Next()
    {
        // next button
        if(page == 1)
        {
            page = 2;
        }

        // start button
        else if(page == 2)
        {
            FadeToBlack();
            MusicManager.instance.FadeOut();
            DelayAction(StartGame, 2);
        }

        SfxManager.instance.PlaySFX(click);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(fightScene);
    }

    public void Confirm()
    {
        if(confirmTime < 0.25f)
        {
            return;
        }

        confirmTime = 0;

        if(page == 1)
        {
            if (index == 0)
            {
                chooseEnemies = !chooseEnemies;
                SfxManager.instance.PlaySFX(click);
            }
            else if (index == 1)
            {
                healAfterFight = !healAfterFight;
                SfxManager.instance.PlaySFX(click);
            }
            else if (index == 2)
            {
                canRetry = !canRetry;
                SfxManager.instance.PlaySFX(click);
            }

            else if (index == 3)
                Next();

            else if (index == 4)
                RestoreDefaults();

            else if (index == 5)
            {
                Back();
            }
        }

        else
        {
            if(index == 0 || index == 1 || index == 2)
            {
                IncrementSlider();
            }

            else if(index == 3)
            {
                // start game
                Next();
            }

            else if (index == 4)
            {
                RestoreDefaults();
            }

            else if(index == 5)
            {
                Back();
            }
        }
    }

    void DelayAction(DelayedAction da, float duration)
    {
        if (currentAction != null)
            return;

        delayTime = duration;
        currentAction = da;
        startCounting = true;
    }

    public delegate void DelayedAction();
}
