using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSplosionSmall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(0.8f, DestroySelf);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
