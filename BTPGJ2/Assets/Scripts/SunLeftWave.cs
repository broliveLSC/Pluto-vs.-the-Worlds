using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLeftWave : MonoBehaviour
{
    public float speed = 7.0f;
    public Vector3 direction = new Vector2(1, 0);
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(3.0f, DestroySelf);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= direction * speed * Time.deltaTime * GameManager.instance.speed;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
