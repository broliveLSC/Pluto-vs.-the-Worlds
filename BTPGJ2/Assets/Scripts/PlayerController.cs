using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpForce = 7;

    public SpriteRenderer sprite;

    float initGravityMult = 1;
    float facing = 1;

    // dash
    public float dashMult = 2;
    public float dashLength = 0.5f;
    bool canDash = true;
    public bool isDashing = false;
    float dashVelocity;

    public float killZ = -10;

    public Animator face;
    //public Animator scalar;
    public Animator squashAnim;
    public Transform rotator;
    Quaternion initRotation;

    public GameObject jumpFX;
    public GameObject splosion;
    public GameObject bolt;

    bool prevGrounded = false;
    private float splosionTimer = 0;

    public bool canInput = true;
    float prevDiff = 0;

    Vector2 startPos;
    private float dyingTime;

    public AudioClip hit;
    public AudioClip attack;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        initGravityMult = gravityMult;
        initRotation = rotator.rotation;
        startPos = transform.position - new Vector3(0, 3, 0);
        //gravityMult = 0;

        if (RunSettings.instance != null)
        {
            GetComponent<Damageable>().maxHealth = RunSettings.instance.playerHealth;
            GetComponent<Damageable>().currentHealth = RunSettings.instance.playerHealth;

            
        }

        FindObjectOfType<PlayerHealthDisplay>().Init();
        //Spawn();


    }

    public void Spawn()
    {
        bolt.GetComponent<SpriteRenderer>().enabled = true;
        //bolt.transform.position = new Vector3( transform.position.x, bolt.transform.position.y, bolt.transform.position.z);
        GameManager.instance.AddTimer(0.5f, DisableBolt);
    }

    void DisableBolt()
    {
        bolt.GetComponent<SpriteRenderer>().enabled = false;
        gravityMult = initGravityMult;
    }

    // instead of update
    protected override void ComputeVelocity()
    {
        if (GetComponent<Damageable>().died)
        {
            UpdateDying();
            return;
        }

        float diff = GetComponent<Damageable>().maxHealth - GetComponent<Damageable>().currentHealth;
        if (diff - prevDiff != 0)
        {
            // we've been hit!
            face.SetTrigger("hit");
        }
        prevDiff = diff;

        Vector2 move = Vector2.zero;
        float h_input = Input.GetAxis("Horizontal");
        if (!canInput)
            h_input = 0;

        move.x = h_input;
        if(move.x != 0)
            facing = Mathf.Sign(move.x);
        
        if(isDashing)
        {
            move.x = dashVelocity;
        }

        if(Input.GetButtonDown("Fire1") && canDash && canInput) // dash forward
        {
            
            face.SetTrigger("dash");
            squashAnim.SetTrigger("squash");
            GetComponent<Damageable>().invulnerable = true;
            rotator.rotation = initRotation;
            canDash = false;
            isDashing = true;
            dashVelocity = move.x = dashMult * facing;
            
            gravityMult = 0;
            velocity.y = 0;

            GameManager.instance.AddTimer(dashLength, StopDash);

            //gameObject.AddComponent<ContactDamager>();
            GetComponent<ContactDamager>().enabled = true;
            //GetComponent<ContactDamager>().damageAmount = 5;
            SfxManager.instance.PlaySFX(attack, true);
            Instantiate(jumpFX, transform.position + new Vector3(1.5f * facing, 0f, 0), Quaternion.Euler(0, 0, 90 * facing));
        }

        if(canInput && Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpForce;
            //scalar.SetBool("squash", true);
            //GameManager.instance.AddTimer(1, SquashAndStretch);
            Instantiate(jumpFX, transform.position + new Vector3(0, -0.25f, 0), Quaternion.Euler(0,0,0));
        }

        else if (canInput && Input.GetButtonUp("Jump"))
        {
            if(velocity.y > 0)
            {
                velocity.y = velocity.y * .5f;
            }
        }

        targetVelocity = move * maxSpeed;

        if(!isDashing)
            rotator.Rotate(Vector3.forward, move.x * -20);

        if(!prevGrounded && grounded)
        {
            // hit ground so squash down
            squashAnim.SetTrigger("squash");
        }

        if (prevGrounded && !grounded)
        {
            // hit ground so squash down
            squashAnim.SetTrigger("stretch");
        }
        prevGrounded = grounded;
        
        
    }

    public new void Update()
    {
        base.Update();
        if (grounded && !isDashing)
            canDash = true;

        if(transform.position.y < killZ)
        {
            bolt.transform.position = new Vector3(transform.position.x, bolt.transform.position.y, bolt.transform.position.z);

            KillByFall();
            
        }

        sprite.flipX = facing != 1;
    }

    public void KillByFall()
    {
        velocity.y = 0;
        transform.position = startPos;
        //bolt.getComponent<SpriteRenderer>().enabled = true;
        Spawn();
        //GameManager.instance.AddTimer(0.5f, FallDamage);
        FallDamage();
    }

    void FallDamage()
    {
        GetComponent<Damageable>().TakeDamage(1, transform.position);
    }

    void Splode()
    {
        
        Instantiate(splosion,
            transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-5, 5)),
            Quaternion.Euler(new Vector3(0, 0, Random.Range(-180, 180))));
    }

    void UpdateDying()
    {
        dyingTime += Time.deltaTime * GameManager.instance.speed;

        if (dyingTime > 2.5f)
            PlayerDeath();

        canInput = false;

        splosionTimer += Time.deltaTime * GameManager.instance.speed;
        if (splosionTimer > 0.15f)
        {
            splosionTimer = 0;
            Splode();
        }
        //if (fall)
            //transform.position -= new Vector3(0, 1, 0) * Time.deltaTime * GameManager.instance.speed;
    }

    void PlayerDeath()
    {
        //SfxManager.instance.PlaySFX(hit, true);
        dyingTime = 0;
        GameManager.instance.StartGameOverProcess();
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        GetComponent<Damageable>().currentHealth = GetComponent<Damageable>().maxHealth;
        GetComponent<Damageable>().died = false;

        canInput = true;
        velocity.y = 0;
        transform.position = startPos;
        //gravityMult = 0;

        /*if (RunSettings.instance != null)
        {
            GetComponent<Damageable>().maxHealth = RunSettings.instance.playerHealth;
            GetComponent<Damageable>().currentHealth = RunSettings.instance.playerHealth;
        }*/

        FindObjectOfType<PlayerHealthDisplay>().Init();
    }

    void StopDash()
    {
        GetComponent<Damageable>().invulnerable = false;
        Debug.Log("StopDash");
        isDashing = false;
        velocity.x = 0;
        gravityMult = initGravityMult;
        //Destroy(GetComponent<ContactDamager>());
        GetComponent<ContactDamager>().enabled = false;
    }

    /*void SquashAndStretch()
    {
        scalar.SetBool("squash", false);
        scalar.SetBool("stretch", false);
    }*/
    
    
}