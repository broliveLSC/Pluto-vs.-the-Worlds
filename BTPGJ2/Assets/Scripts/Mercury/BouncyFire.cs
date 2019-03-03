using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyFire : PhysicsObject
{
    bool prevGrounded = false;
    public float jumpForce = 4;
    public float mySpeed = 3;

    public Transform sprite;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(15, DestroySelf);
    }

    protected override void ComputeVelocity()
    {
        sprite.transform.Rotate(Vector3.forward, 500 * Time.deltaTime);

        if (!prevGrounded && grounded)
        {
            // hit ground so squash down
            //squashAnim.SetTrigger("squash");
            velocity.y = jumpForce;
        }

        if (prevGrounded && !grounded)
        {
            // hit ground so squash down
            //squashAnim.SetTrigger("stretch");
        }

        prevGrounded = grounded;

        targetVelocity = Vector2.right * mySpeed;
        //transform.position += new Vector3(mySpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // make sure damager is enabled
            if (!collision.gameObject.GetComponent<PlayerController>().enabled)
                return;

            // check if layer is appropriate before damage
            /*if ((collision.GetComponent<ContactDamager>().damagedLayers.value & 1 << gameObject.layer) == 0)
                return;*/

            //TakeDamage(collision.gameObject.GetComponent<ContactDamager>().damageAmount, collision.transform.position);
            Disable();

        }
    }

    void Disable()
    {
        FindObjectOfType<PlayerController>().GetComponent<Damageable>().TakeDamage(1, transform.position);
        foreach(BoxCollider2D b in GetComponents<BoxCollider2D>())
            b.enabled = false;
        GetComponent<ContactDamager>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        
    }

    void DestroySelf()
    {
        if(gameObject != null)
            Destroy(gameObject);
    }
}
