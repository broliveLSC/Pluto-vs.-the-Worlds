using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballMissile : MonoBehaviour
{
    public GameObject firesplosion;

    float elapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        GetComponentInChildren<Animator>().SetTrigger(Random.Range(0, 2) > 0 ? "L" : "R" );
        //GameManager.instance.AddTimer(1, DestroyThis);
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime * GameManager.instance.speed;
        if (elapsed > 0.75f)
            DestroyThis();
    }

    void DestroyThis()
    {
        // spawn explosion
        Instantiate(firesplosion, 
                    GetComponentInChildren<BoxCollider2D>().transform.position + new Vector3(0, 1.75f, 0), 
                    Quaternion.Euler(Vector3.zero));

        Destroy(gameObject);
    }

}
