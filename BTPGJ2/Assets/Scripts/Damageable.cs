using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth = 0;
    public bool hitStop = true;
    public bool invulnerable = false;
    public bool died = false;

    public SpriteRenderer sprite;

    public Animator shakeTarget;

    public GameObject hitMark;

    public AudioClip hurtSound;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ContactDamager>() != null)
        {
            // make sure damager is enabled
            if (!collision.gameObject.GetComponent<ContactDamager>().enabled)
                return;

            // check if layer is appropriate before damage
            if ((collision.GetComponent<ContactDamager>().damagedLayers.value & 1 << gameObject.layer) == 0)
                return;

            TakeDamage(collision.gameObject.GetComponent<ContactDamager>().damageAmount, collision.transform.position);

            
        }
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // die
            died = true;
            invulnerable = true;
        }
    }

    /*void TakeDamage(float amount, Vector3 position)
    {
        TakeDamage(amount);

        
    }*/

    public void TakeDamage(float amount, Vector3 position)
    {
        if (invulnerable)
            return;

        

        currentHealth -= amount;
        invulnerable = true;
        GameManager.instance.AddTimer(0.25f, MakeVulnerable);
        
        // do shake
        if(shakeTarget != null)
            shakeTarget.SetTrigger("shake");

        
        sprite.material.shader = Shader.Find("GUI/Text Shader");
        sprite.color = Color.white;

        if (hitMark != null)
        {
            Instantiate(hitMark, position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-180, 180)))/*transform.rotation*/);
        }


        if (hitStop)
        {
            GameManager.instance.speed = 0;
            GameManager.instance.AddTimer(0.1f, GameManager.instance.RestoreTime, "hit-stop", false);
            
        }

        if (hurtSound != null)
        {
            if (GetComponent<PlayerController>() == null)
            {
                int rand = Random.Range(1, 11);
                if (rand > 5)
                {
                    SfxManager.instance.PlaySFX(hurtSound);
                }
            }
            else
            {
                SfxManager.instance.PlaySFX(hurtSound, true);
            }
        }

    }

    void MakeVulnerable()
    {
        invulnerable = false;
        sprite.material.shader = Shader.Find("Sprites/Default");
    }
}
