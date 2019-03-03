using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSplosionCharge : MonoBehaviour
{
    public float delay = 1.25f;
    public GameObject splosion;
    // Start is called before the first frame update
    void Start()
    {

        GameManager.instance.AddTimer(delay, SpawnSplosion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnSplosion()
    {
        Instantiate(splosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
