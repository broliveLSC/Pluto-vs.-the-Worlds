using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RetrySelect : MonoBehaviour
{
    public Transform[] points;
    public int index = 0;

    float prevH = 0;

    float hTime = 0;

    bool canH = true;

    float confirmTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
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

        transform.position = points[index].position;
    }

    void Left()
    {
        index -= 1;
        if (index < 0)
            index = 1;

        canH = false;
    }

    void Right()
    {
        index += 1;
        if (index > 1)
            index = 0;

        canH = false;
    }

    public void SetIndex(int i)
    {
        index = i;
    }

    public void Confirm()
    {
        switch(index)
        {
            case 0:
                DoRetry();
                break;
            case 1:
                DoGameOver();
                break;
        }
    }

    void DoRetry()
    {
        GameManager.instance.RestartBattle();
    }

    void DoGameOver()
    {
        GameManager.instance.AddTimer(0.5f, GameManager.instance.FadeToBlack);
        GameManager.instance.AddTimer(2.0f, GameManager.instance.GameOverMan);
    }
}
