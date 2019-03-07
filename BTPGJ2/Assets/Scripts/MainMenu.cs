using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    float prevH = 0;
    float prevV = 0;

    float hTime = 0;
    float vTime = 0;

    bool canH = true;
    bool canV = true;
    float confirmTime = 0;

    int index;
    public GameObject[] options;
    private DelayedAction currentAction = null;
    private bool startCounting = false;
    private float delayTime = 0;
    private float elapsed = 0;

    public AudioClip move;
    public AudioClip click;

    // Start is called before the first frame update
    void Start()
    {
        // destroy managers that may have made it back to the beginning
        if(FindObjectOfType<GameManager>() != null)
            Destroy(FindObjectOfType<GameManager>().gameObject);
        if (FindObjectOfType<RunSettings>() != null)
            Destroy(FindObjectOfType<RunSettings>().gameObject);

        FindObjectOfType<Fade>().alpha = 1;
        FadeFromBlack();
    }

    // Update is called once per frame
    void Update()
    {
        confirmTime += Time.deltaTime;
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

        if (Input.GetButtonDown("Jump"))// || Input.GetButtonDown("Fire1"))
        {
            Confirm();
        }
        
        for(int i = 0; i< options.Length; i++)
        {
            if(i == index)
            {
                options[i].transform.localScale = new Vector3(1.5f, 1.5f, 1);
                options[i].GetComponent<Text>().color = Color.white;
            }

            else
            {
                options[i].transform.localScale = new Vector3(1, 1, 1);
                options[i].GetComponent<Text>().color = new Color(0.8f, 0.8f, 0.8f);
            }
        }

        prevH = h;
        prevV = v;


        if(startCounting)
        {
            elapsed += Time.deltaTime;
            if(elapsed > delayTime)
            {
                currentAction();
            }
        }

        options[1].GetComponent<Text>().text = "Music: " + (GameSettings.instance.musicOn ? "ON" : "OFF");
        options[2].GetComponent<Text>().text = "SFX: " + (GameSettings.instance.sfxOn ? "ON" : "OFF");
    }

    void Left()
    {
        if (index == 1 || index == 2)
            Confirm();

        canH = false;
    }

    void Right()
    {
        if (index == 1 || index == 2)
            Confirm();

        canH = false;
    }

    void Up()
    {
        index -= 1;
        if (index < 0)
            index = options.Length - 1;

        SfxManager.instance.PlaySFX(move);

        canV = false;
    }

    void Down()
    {
        index += 1;
        if (index >= options.Length)
            index = 0;

        SfxManager.instance.PlaySFX(move);

        canV = false;
    }

    public void Confirm()
    {
        if (confirmTime < 0.25f)
        {
            return;
        }

        SfxManager.instance.PlaySFX(click);

        confirmTime = 0;

        switch(index)
        {
            // start game
            case 0:
                FadeToBlack();
                // fade out, then load scene
                DelayAction(StartGame, 3);
                break;
            // music toggle
            case 1:
                GameSettings.instance.musicOn = !GameSettings.instance.musicOn;
                confirmTime = 0;
                break;
            // sfx toggle
            case 2:
                GameSettings.instance.sfxOn = !GameSettings.instance.sfxOn;
                confirmTime = 0;
                break;
            // credits
            case 3:
                FadeToBlack();
                // fade out, then load scene
                DelayAction(GoToCredits, 3);
                break;
            // exit
            case 4:
                FadeToBlack();
                DelayAction(Application.Quit, 3);
                break;
        }
    }

    public void SetIndex(int i)
    {
        index = i;
        SfxManager.instance.PlaySFX(move);
    }

    public void FadeToBlack()
    {
        FindObjectOfType<Fade>().active = true;
        FindObjectOfType<Fade>().mult = 1;
    }

    public void FadeFromBlack()
    {
        FindObjectOfType<Fade>().active = true;
        FindObjectOfType<Fade>().mult = -1;
    }

    void StartGame()
    {
        SceneManager.LoadScene("RunSettings");
    }

    void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
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
