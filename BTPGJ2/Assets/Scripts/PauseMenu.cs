using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    float prevH = 0;
    float prevV = 0;

    float hTime = 0;
    float vTime = 0;

    bool canH = true;
    bool canV = true;
    float confirmTime = 0;

    public AudioClip move;
    public AudioClip click;
    private int index;

    bool canClick = true;
    public Transform[] options;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i< options.Length; i++)
        {
            if(index == i)
            {
                options[i].localScale = new Vector3(1.1f, 1.1f, 1);
            }
            else
            {
                options[i].localScale = new Vector3(.8f, .8f, 1);
            }
        }
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

        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1"))
        {
            Confirm();
        }

        prevH = h;
        prevV = v;
    }

    void Left()
    {

    }

    void Right()
    {

    }

    void Up()
    {
        index -= 1;
        if (index < 0)
            index = 1;
        canV = false;
        SfxManager.instance.PlaySFX(move);

        
    }

    void Down()
    {
        index += 1;
        if (index > 1)
            index = 0;
        canV = false;
        SfxManager.instance.PlaySFX(move);

        
    }

    void Confirm()
    {
        if (!canClick)
            return;

        

        if(index == 0)
        {
            GameManager.instance.Resume();
        }

        else
        {
            canClick = false;
            GameManager.instance.FadeToBlack();
            //GameManager.instance.AddTimer(2, ReturnToTitle);
            ReturnToTitle();
        }

        SfxManager.instance.PlaySFX(click);
    }

    public void ReturnToTitle()
    {
        Debug.Log("Return to title");
        SceneManager.LoadScene("title");
    }
}
