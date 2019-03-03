using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleDropper : MonoBehaviour
{
    public int dir = 1;
    public Vector2 startL;
    public Vector2 startR;
    public float speed = 2.0f;
    public float dropRate = 1.0f;
    public GameObject icicle;

    public float elapsed = 0;

    // Start is called before the first frame update
    void Start ()
    {
        // repeat choice until we get a non-zero
        do
        {
            dir = Random.Range(-1, 2);
        } while (dir == 0);

        if (dir == 1)
        {
            transform.position = startL;
        }
        else
            transform.position = startR;

        GameManager.instance.AddTimer(dropRate, DropIcicle);

        
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * dir * Time.deltaTime * GameManager.instance.speed;

        elapsed += Time.deltaTime * GameManager.instance.speed;

        if (elapsed > 20)
            DestroySelf();
    }

    void DropIcicle()
    {
        //elapsedSinceDrop = 0;
        Debug.Log("Drop");
        Instantiate(icicle, transform.position, transform.rotation);

        GameManager.instance.AddTimer(dropRate, DropIcicle);
    }

    void DestroySelf()
    {
        //Destroy(gameObject);
    }
}
