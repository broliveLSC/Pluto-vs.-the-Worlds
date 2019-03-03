using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasPreview : MonoBehaviour
{
    public GameObject gasSplosion;
    public float duration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(duration, DoThing);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoThing()
    {
        Instantiate(gasSplosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
