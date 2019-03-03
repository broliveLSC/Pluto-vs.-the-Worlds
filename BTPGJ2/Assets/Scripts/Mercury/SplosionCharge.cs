using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplosionCharge : MonoBehaviour
{
    public GameObject bigSplosion;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(2, Splode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Splode()
    {
        // spawn a splosion
        Instantiate(bigSplosion);//, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
