using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neptune : MonoBehaviour
{
    public enum NeptuneState
    {
        SLEEP,
        IDLE,
        WANDER,
        ANGRY,
        DYING,
        ATTACK1,
        ATTACK2,
        ATTACK3
    }

    public NeptuneState state = NeptuneState.SLEEP;

    public Animator sprite;

    public float timeBetweenShots = 2.0f;

    public float elapsedSinceLastAttack = 0.0f;

    public IcicleDropper dropper;
    public IcicleShot shotL;
    public IcicleShot shotR;
    public GameObject soundBlast;
    public GameObject splosion;

    bool halfway = false;
    bool fall = false;
    private float splosionTimer = 0;

    public float initHealth = 30;
    public int bossIndex = 0;

    public AudioClip snoring;
    public AudioClip hit;
    public AudioClip attack;
    public AudioClip dying;
    public AudioClip laughing;
    public AudioClip angry;

    public BossTitle myTitle;
    private bool doMove = true;

    //AudioSource aS;

    // Start is called before the first frame update
    void Start()
    {
        //aS = GetComponent<AudioSource>();
        if (RunSettings.instance != null)
            initHealth = initHealth * RunSettings.instance.enemyHealthMult;

        GetComponent<Damageable>().maxHealth = initHealth;
        GetComponent<Damageable>().currentHealth = initHealth;

        
        SfxManager.instance.PlaySFX(snoring, true, true);

        //aS.clip = snoring;
        //aS.Play();

    }

    // Update is called once per frame
    void Update()
    {

        switch(state)
        {
            case NeptuneState.IDLE:
                UpdateIdle();
                break;

            case NeptuneState.ANGRY:
                UpdateAngry();
                break;

            case NeptuneState.SLEEP:
                UpdateSleep();
                break;

            case NeptuneState.DYING:
                UpdateDying();
                break;

            case NeptuneState.ATTACK1:
                break;

            case NeptuneState.ATTACK2:
                break;

            case NeptuneState.ATTACK3:
                break;
        }
    }

    void UpdateIdle()
    {
        elapsedSinceLastAttack += Time.deltaTime * GameManager.instance.speed;

        if(elapsedSinceLastAttack > timeBetweenShots)
        {
            int r = Random.Range(0, 2);

            switch(r)
            {
                case 0:
                    SpawnIcicleDropper();
                    break;
                case 1:
                    SpawnIcicleShots();
                    break;
                case 2:
                    break;
            }

            state = NeptuneState.ATTACK1;
            sprite.SetTrigger("attack");

            int rand = Random.Range(1, 11);
            if (rand > 4)
            {
                SfxManager.instance.PlaySFX(attack);
                /*aS.Stop();
                aS.clip = attack;
                aS.Play();*/
            }
        }

        if (!halfway)
        {
            float diff = GetComponent<Damageable>().maxHealth - GetComponent<Damageable>().currentHealth;
            if (diff >= GetComponent<Damageable>().maxHealth * 0.5f)
            {
                halfway = true;
                SwitchToAngry();
                GameManager.instance.AddTimer(2, SwitchToIdle);
            }
        }

        if(GetComponent<Damageable>().died)
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

    void UpdateSleep()
    {
        doMove = false;
        elapsedSinceLastAttack += Time.deltaTime * GameManager.instance.speed;

        if(doMove)
            transform.position += 2.8f * -Vector3.right * Time.deltaTime * GameManager.instance.speed;

        if (elapsedSinceLastAttack > 4.5f)
        {
            doMove = false;

        }

        float diff = GetComponent<Damageable>().maxHealth - GetComponent<Damageable>().currentHealth;
        if (diff > 0 && !doMove)
        {
            elapsedSinceLastAttack = 0;
            Instantiate(myTitle);
            // we've been hit! time to wake up
            SwitchToAngry();
            GameManager.instance.AddTimer(2.5f, SwitchToIdle);
            SfxManager.instance.StopSFX(snoring);
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
        if(splosionTimer > 0.5f)
        {
            Splode();
        }
        if(fall)
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
        state = NeptuneState.IDLE;
    }

    void SwitchToAngry()
    {
        SfxManager.instance.PlaySFX(angry, true);
        /*
        aS.loop = false;
        aS.Stop();
        aS.clip = angry;
        aS.Play();*/

        sprite.SetTrigger("angry");
        state = NeptuneState.ANGRY;
        GameManager.instance.AddTimer(1, MakeCameraShake);

        GameManager.instance.AddTimer(1f, AddSoundBlast);
        GameManager.instance.AddTimer(1.25f, AddSoundBlast);
        GameManager.instance.AddTimer(1.4f, AddSoundBlast);
        GameManager.instance.AddTimer(1.65f, AddSoundBlast);
        GameManager.instance.AddTimer(1.8f, AddSoundBlast);
        GameManager.instance.AddTimer(1.95f, AddSoundBlast);
    }

    void SwitchToDying()
    {
        SfxManager.instance.PlaySFX(dying, true);
        /*
        aS.Stop();
        aS.clip = dying;
        aS.Play();*/

        state = NeptuneState.DYING;
    }

    void AddSoundBlast()
    {
        Instantiate(soundBlast);
    }

    void Splode()
    {
        Instantiate(splosion, 
            transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-3, 3), Random.Range(-5,5)),
            Quaternion.Euler(new Vector3(0, 0, Random.Range(-180, 180))));
    }

    void MakeCameraShake()
    {
        GameManager.instance.ShakeCamera("hv");
    }

    void SpawnIcicleShots()
    {
        // choose direction to shoot from
        SpawnIcicleL();

        // shoot from other direction after brief delay
        GameManager.instance.AddTimer(0.25f, SpawnIcicleR);

        // if halfway, do it again

        if (halfway)
        {
            GameManager.instance.AddTimer(1.5f, SpawnIcicleL);
            GameManager.instance.AddTimer(1.75f, SpawnIcicleR);
        }

        ResetAttackTime();
        GameManager.instance.AddTimer(1.5f, SwitchToIdle);

        //ResetAttackTime();
    }

    void SpawnIcicleL()
    {
        Instantiate(shotL);
    }

    void SpawnIcicleR()
    {
        Instantiate(shotR);
    }

    void SpawnIcicleDropper()
    {
        IcicleDropper d = Instantiate(dropper);

        if(halfway)
        {
            IcicleDropper e = Instantiate(dropper);
            e.dir = d.dir * -1;
        }

        ResetAttackTime();
        GameManager.instance.AddTimer(1.5f, SwitchToIdle);
    }

    private void ResetAttackTime()
    {
        elapsedSinceLastAttack = 0;
    }
}
