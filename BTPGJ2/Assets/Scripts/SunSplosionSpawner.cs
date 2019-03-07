using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSplosionSpawner : MonoBehaviour
{
    // this script is unneccessary :)
    bool active = false;
    public float timeBetween = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Activate()
    {
        active = true;
        //GameManager.instance.AddTimer(timeBetween, SpawnWave);
    }

    void Deactivate()
    {

    }
}
