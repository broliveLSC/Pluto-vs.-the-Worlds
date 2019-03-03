using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercury : MonoBehaviour
{
    public enum MercuryState
    {
        ENTER,
        BURNING,
        IDLE,
        ANGRY,
        DYING,
        ATTACK1,
        ATTACK2,
        ATTACK3
    }

    public MercuryState state = MercuryState.ENTER;

    public Animator sprite;

    public float timeBetweenShots = 2.0f;

    public float elapsedSinceLastAttack = 0.0f;

    // attacks
    public GameObject bouncyFire;
    public GameObject fireMissile;
    public GameObject splosionCharge;

    // stuff for all bosses
    public GameObject soundBlast;
    public GameObject splosion;

    bool halfway = false;
    bool fall = false;
    private float splosionTimer = 0;

    public float initHealth = 40;
    public int bossIndex = 0;

    public AudioClip angry;
    public AudioClip attack;
    public AudioClip dying;

    //AudioSource aS;
    public BossTitle myTitle;

    // Start is called before the first frame update
    void Start()
    {
        //aS = GetComponent<AudioSource>();
        //aS.clip = angry;
        if(RunSettings.instance != null)
            initHealth = initHealth * RunSettings.instance.enemyHealthMult;

        GetComponent<Damageable>().maxHealth = initHealth;
        GetComponent<Damageable>().currentHealth = initHealth;
        //GameManager.instance.AddTimer(3, SwitchToAngry);
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case MercuryState.IDLE:
                UpdateIdle();
                break;

            case MercuryState.ANGRY:
                UpdateAngry();
                break;

            case MercuryState.ENTER:
                UpdateEnter();
                break;

            case MercuryState.DYING:
                UpdateDying();
                break;

            case MercuryState.ATTACK1:
                break;

            case MercuryState.ATTACK2:
                break;

            case MercuryState.ATTACK3:
                break;
        }
    }

    void UpdateIdle()
    {
        int range = 2;
        if (halfway)
            range = 3;
        elapsedSinceLastAttack += Time.deltaTime * GameManager.instance.speed;

        if (elapsedSinceLastAttack > timeBetweenShots)
        {
            int r = Random.Range(0, range);

            switch (r)
            {
                case 0:
                    // shoot fire missiles
                    FireMissile();
                    GameManager.instance.AddTimer(0.25f, FireMissile);
                    GameManager.instance.AddTimer(0.5f, FireMissile);
                    GameManager.instance.AddTimer(0.75f, FireMissile);
                    GameManager.instance.AddTimer(1f, FireMissile);
                    GameManager.instance.AddTimer(1.25f, FireMissile);
                    if(halfway)
                    {
                        GameManager.instance.AddTimer(1.5f, FireMissile);
                        GameManager.instance.AddTimer(1.75f, FireMissile);
                        GameManager.instance.AddTimer(2f, FireMissile);
                    }
                    break;
                case 1:
                    // shoot bouncy fireball
                    BouncyFire();

                    if (halfway)
                        GameManager.instance.AddTimer(1.75f, BouncyFire);

                    break;
                case 2:
                    // big ol splosion
                    BigSplosion();
                    if (halfway)
                    {
                        /*GameManager.instance.AddTimer(0.5f, BigSplosion);
                        GameManager.instance.AddTimer(.75f, BigSplosion);
                        GameManager.instance.AddTimer(1f, BigSplosion);*/
                    }
                    break;
            }

            state = MercuryState.ATTACK1;
            sprite.SetTrigger("attack");
            GameManager.instance.AddTimer(2.5f, ResetAttackTime);

            int rand = Random.Range(1, 11);
            if (rand > 4)
            {
                /*aS.Stop();
                aS.clip = attack;
                aS.Play();*/

                SfxManager.instance.PlaySFX(attack);
            }
        }

        if (!halfway)
        {
            float diff = GetComponent<Damageable>().maxHealth - GetComponent<Damageable>().currentHealth;
            if (diff >= GetComponent<Damageable>().maxHealth * 0.5f)
            {
                halfway = true;
                timeBetweenShots = timeBetweenShots * 0.8f;
                SwitchToAngry();
                GameManager.instance.AddTimer(2, SwitchToIdle);
            }
        }

        if (GetComponent<Damageable>().died)
        {
            GameManager.instance.AddTimer(1, StartFalling);
            SwitchToDying();
            sprite.SetTrigger("sad");
            GetComponent<ContactDamager>().enabled = false;

            GameManager.instance.AddTimer(0f, AddSoundBlast);
            GameManager.instance.AddTimer(0.25f, AddSoundBlast);
            GameManager.instance.AddTimer(0.4f, AddSoundBlast);
            GameManager.instance.AddTimer(0.65f, AddSoundBlast);
            GameManager.instance.AddTimer(0.8f, AddSoundBlast);
            GameManager.instance.AddTimer(0.95f, AddSoundBlast);

            // add explosions here?
        }
    }

    void UpdateEnter()
    {
        // Rise from below stage
        elapsedSinceLastAttack += Time.deltaTime * GameManager.instance.speed;

        //transform.position += 2.8f * Vector3.right * Time.deltaTime * GameManager.instance.speed;

        if (elapsedSinceLastAttack > 4.5f)
        {
            Instantiate(myTitle);
            SwitchToAngry();
            elapsedSinceLastAttack = 0;
        }
    }

    void UpdateAngry()
    {

    }

    void UpdateAttack()
    {

    }

    void StartFalling()
    {
        fall = true;
    }

    void UpdateDying()
    {
        GameObject.Find("Player").GetComponent<Damageable>().invulnerable = true;
        splosionTimer += Time.deltaTime * GameManager.instance.speed;
        if (splosionTimer > 0.5f)
        {
            Splode();
        }
        if (fall)
            transform.position -= new Vector3(0, 1, 0) * Time.deltaTime * GameManager.instance.speed;

        if (transform.position.y < FindObjectOfType<PlayerController>().killZ)
        {
            //FindObjectOfType<BossManager>().BeatBoss(bossIndex);
            FindObjectOfType<BossMenu2>().BeatBoss(bossIndex);
            gameObject.SetActive(false);
        }
    }

    void SwitchToIdle()
    {
        sprite.SetTrigger("idle");
        state = MercuryState.IDLE;

        if (halfway)
        {

        }
        
    }

    void SwitchToAngry()
    {
        sprite.SetTrigger("angry");
        state = MercuryState.ANGRY;
        GameManager.instance.AddTimer(1, MakeCameraShake);
        GameManager.instance.AddTimer(2, SwitchToIdle);

        GameManager.instance.AddTimer(1f, AddSoundBlast);
        GameManager.instance.AddTimer(1.25f, AddSoundBlast);
        GameManager.instance.AddTimer(1.4f, AddSoundBlast);
        GameManager.instance.AddTimer(1.65f, AddSoundBlast);
        GameManager.instance.AddTimer(1.8f, AddSoundBlast);
        GameManager.instance.AddTimer(1.95f, AddSoundBlast);

        //aS.Play();
        SfxManager.instance.PlaySFX(angry, true);
    }

    void SwitchToDying()
    {
        //aS.clip = dying;
        //aS.Play();
        SfxManager.instance.PlaySFX(dying, true);

        state = MercuryState.DYING;
        foreach (SpinningRing r in FindObjectsOfType<SpinningRing>())
        {
            r.End();
        }
    }

    void FireMissile()
    {
        Instantiate(fireMissile);
    }

    void BouncyFire()
    {
        int random = 0;
        while (random == 0)
            random = Random.Range(-1, 2);
        BouncyFire b = Instantiate(bouncyFire, 
                                   transform.position + new Vector3(3 * -random,2,0),
                                   transform.rotation).GetComponent<BouncyFire>();
        b.mySpeed = b.mySpeed * random;
    }

    void BigSplosion()
    {
        Instantiate(splosionCharge);
    }

    void AddSoundBlast()
    {
        Instantiate(soundBlast);
    }

    void Splode()
    {
        Instantiate(splosion,
            transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-3, 3), Random.Range(-5, 5)),
            Quaternion.Euler(new Vector3(0, 0, Random.Range(-180, 180))));
    }

    void MakeCameraShake()
    {
        GameManager.instance.ShakeCamera("hv");
    }
    

    private void ResetAttackTime()
    {
        elapsedSinceLastAttack = 0;
        state = MercuryState.IDLE;
    }
}