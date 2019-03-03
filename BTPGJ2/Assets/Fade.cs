using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public bool active = false;
    public float mult = 1;
    public Image i;
    public float alpha = 0;
    public float fadeSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            alpha += fadeSpeed * mult * Time.deltaTime;
            
        }

        if (alpha > 1)
        {
            alpha = 1;
            active = false;
        }
        else if (alpha < 0)
        {
            alpha = 0;
            active = false;
        }

        i.color = new Color(0, 0, 0, alpha);
    }
}
