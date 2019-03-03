using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSpawns : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject gas;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(1, DoThing);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoThing()
    {
        foreach(Transform t in spawnPoints)
        {
            Instantiate(gas, t.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-180, 180))));
        }

        Destroy(gameObject);
    }
}
