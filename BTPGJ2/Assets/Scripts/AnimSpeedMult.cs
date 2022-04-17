using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSpeedMult : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance != null)
            GetComponent<Animator>().SetFloat("SpeedMult", GameManager.instance.speed);
    }
}
