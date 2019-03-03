using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercScorch : MonoBehaviour
{
    public float speed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(10, DestroySelf);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -speed * Time.deltaTime * GameManager.instance.speed, 0);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

}
