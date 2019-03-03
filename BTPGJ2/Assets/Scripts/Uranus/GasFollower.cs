using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasFollower : MonoBehaviour
{
    float elapsed = 0;
    public float spacing = 1.0f;
    public GameObject gas;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(4.2f, DestroySelf);
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PlayerController>() == null)
            return;

        transform.position = FindObjectOfType<PlayerController>().transform.position;

        elapsed += Time.deltaTime * GameManager.instance.speed;

        if(elapsed > spacing)
        {
            Instantiate(gas, transform.position, transform.rotation);
            elapsed = 0;
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
