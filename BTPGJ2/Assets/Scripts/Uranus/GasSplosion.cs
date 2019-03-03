using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(0.4f, DestroySelf);
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
