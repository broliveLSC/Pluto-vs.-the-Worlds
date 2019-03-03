using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saturn : MonoBehaviour
{
    public enum SaturnState
    {
        ENTER,
        IDLE,
        WANDER,
        ANGRY,
        DYING,
        ATTACK1,
        ATTACK2,
        ATTACK3
    }

    public SaturnState state = SaturnState.ENTER;

    public Animator sprite;

    public float timeBetweenShots = 2.0f;

    public float elapsedSinceLastAttack = 0.0f;

    // attacks
    public GameObject smallRing;
    public GameObject bigRing;
    public GameObject spinnerH;
    public GameObject spinnerV;

    // stuff for all bosses
    public GameObject soundBlast;
    public GameObject splosion;

    bool halfway = false;
    bool fall = false;
    private float splosionTimer = 0;

    public float initHealth = 50;
    public int bossIndex = 0;

    public AudioClip hit;
    public AudioClip attack;
    public AudioClip dying;
    public AudioClip angry;

    public BossTitle myTitle;

    // Start is called before the first frame update
    void Start()
    {
        if (RunSettings.instance != null)
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
            case SaturnState.IDLE:
                UpdateIdle();
                break;

            case SaturnState.ANGRY:
                UpdateAngry();
                break;

            case SaturnState.ENTER:
                UpdateEnter();
                break;

            case SaturnState.DYING:
                UpdateDying();
                break;

            case SaturnState.ATTACK1:
                break;

            case SaturnState.ATTACK2:
                break;

            case SaturnState.ATTACK3:
                break;
        }
    }

    void UpdateIdle()
    {
        elapsedSinceLastAttack += Time.deltaTime * GameManager.instance.speed;

        if (elapsedSinceLastAttack > timeBetweenShots)
        {
            int r = Random.Range(0, 1);

            switch (r)
            {
                case 0:
                    AddBigSpinningBlade();
                    SfxManager.instance.PlaySFX(attack);
                    break;
                case 1:
                    
                    break;
                case 2:
                    break;
            }

            //state = SaturnState.ATTACK1;
            //sprite.SetTrigger("attack");
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
        /*float diff = GetComponent<Damageable>().maxHealth - GetComponent<Damageable>().currentHealth;
        if (diff > 0)
        {
            // we've been hit! time to wake up
            SwitchToAngry();
            GameManager.instance.AddTimer(2.5f, SwitchToIdle);
        }*/

        // Rise from below stage
        elapsedSinceLastAttack += Time.deltaTime * GameManager.instance.speed;

        transform.position += 2.0f * Vector3.up * Time.deltaTime * GameManager.instance.speed;

        if(elapsedSinceLastAttack > 4.0f)
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
        if (FindObjectOfType<PlayerController>() == null)
            return;

        GameObject.Find("Player").GetComponent<Damageable>().invulnerable = true;
        splosionTimer += Time.deltaTime * GameManager.instance.speed;
        if (splosionTimer > 0.5f)
        {
            Splode();
        }
        if (fall)
            transform.position -= new Vector3(0, 1, 0) * Time.deltaTime * GameManager.instance.speed;

        if(transform.position.y < FindObjectOfType<PlayerController>().killZ)
        {
            //FindObjectOfType<BossManager>().BeatBoss(bossIndex);
            FindObjectOfType<BossMenu2>().BeatBoss(bossIndex);
            gameObject.SetActive(false);
        }
    }

    void SwitchToIdle()
    {
        sprite.SetTrigger("idle");
        state = SaturnState.IDLE;

        if(halfway)
        {
            Instantiate(spinnerV);
        }

        else
            Instantiate(spinnerH);
    }

    void SwitchToAngry()
    {
        SfxManager.instance.PlaySFX(angry, true);
        sprite.SetTrigger("angry");
        state = SaturnState.ANGRY;
        GameManager.instance.AddTimer(1, MakeCameraShake);
        GameManager.instance.AddTimer(2, SwitchToIdle);

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
        state = SaturnState.DYING;
        foreach(SpinningRing r in FindObjectsOfType<SpinningRing>())
        {
            r.End();
        }
    }

    void AddBigSpinningBlade()
    {
        float speedMult = 1;

        if (halfway)
            speedMult = 1.5f;

        Vector2 speed = Vector2.zero;
        Vector2 pos = transform.position;
        SpinningRing r = Instantiate(bigRing).GetComponent<SpinningRing>();

        int rand = Random.Range(0, 4);
        switch(rand)
        {
            case 0://top left, moving down
                speed = new Vector2(0, -10 * speedMult);
                pos = transform.position + new Vector3(-4, 5, 0);
                break;
            case 1://top right moving down
                speed = new Vector2(0, -10 * speedMult);
                pos = transform.position + new Vector3(4, 5, 0);
                break;
            case 2://bottom left, moving up
                speed = new Vector2(0, 10 * speedMult);
                pos = transform.position + new Vector3(-6, -7, 0);
                break;
            case 3://bottom right, moving up
                speed = new Vector2(0, 10 * speedMult);
                pos = transform.position + new Vector3(6, -7, 0);
                break;
        }

        r.speed = speed;
        r.transform.position = pos;
        elapsedSinceLastAttack = 0;
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
    }
}
