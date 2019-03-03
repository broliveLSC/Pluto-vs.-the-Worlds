using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float seconds = 5;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, seconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
