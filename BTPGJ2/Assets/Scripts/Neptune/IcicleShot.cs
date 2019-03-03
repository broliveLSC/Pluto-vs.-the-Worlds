using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleShot : MonoBehaviour
{
    public float scaleSpeed = 2.5f;
    bool grow = true;

    public float rotMin = -10;
    public float rotMax = -10;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = transform.localScale * 0.1f;
        GameManager.instance.AddTimer(0.5f, StopGrowing);
        GameManager.instance.AddTimer(1f, Shoot);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(rotMin, rotMax)));
    }

    // Update is called once per frame
    void Update()
    {
        if (grow)
            transform.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime * GameManager.instance.speed; ;
    }

    void StopGrowing()
    {
        grow = false;
    }

    void Shoot()
    {
        
        foreach (Icicle i in GetComponentsInChildren<Icicle>())
        {
            i.go = true;
        }
        //Destroy(gameObject);
        GameManager.instance.AddTimer(10, DestroySelf);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
