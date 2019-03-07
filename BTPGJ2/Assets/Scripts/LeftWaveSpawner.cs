using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftWaveSpawner : MonoBehaviour
{
    public GameObject wave;
    public float timeBetween = 0.75f;

    public bool active = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnWave()
    {
        if (!active)
            return;

        Instantiate(wave, transform.position, transform.rotation);

        GameManager.instance.AddTimer(timeBetween, SpawnWave);
    }

    public void Activate()
    {
        active = true;
        GameManager.instance.AddTimer(timeBetween, SpawnWave);
    }

    public void Deactivate()
    {
        active = false;
    }
}
