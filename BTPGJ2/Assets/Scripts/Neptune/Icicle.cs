using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    public float speed = 7.0f;
    public bool go = true;
    // Start is called before the first frame update
    void OnEnable()
    {
        //GameManager.instance.AddTimer(10, DestroySelf);
    }

    // Update is called once per frame
    void Update()
    {
        if(go)
        transform.position = transform.position - transform.up * speed * Time.deltaTime * GameManager.instance.speed;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
