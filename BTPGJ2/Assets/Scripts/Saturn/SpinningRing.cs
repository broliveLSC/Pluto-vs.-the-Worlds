using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningRing : MonoBehaviour
{
    public Transform img;
    public float rotSpeed = 5.0f;

    public Vector3 speed = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        img.Rotate(Vector3.forward, rotSpeed * Time.deltaTime * GameManager.instance.speed);

        transform.position += speed * Time.deltaTime * GameManager.instance.speed;
    }

    public void End()
    {
        GameObject splosion = FindObjectOfType<Saturn>().splosion;
        Instantiate(splosion, transform.position, transform.rotation);
        Destroy(gameObject, 0.25f);
    }
}
