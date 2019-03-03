using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venus : MonoBehaviour
{
    public enum VenusState
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

    public VenusState state = VenusState.ENTER;

    public Animator sprite;

    public float timeBetweenShots = 2.0f;

    public float elapsedSinceLastAttack = 0.0f;

    // attacks
    /*public Animator eyeTargetL;
    public Animator eyeTargetR;
    public LineRenderer eyeL;
    public LineRenderer eyeR;*/

    public Animator beams;
    public float initAnimSpeed = 0.5f;
    public float secondAnimSpeed = 1f;

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
    public AudioClip pew;

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
        /*if(eyeL.enabled)
            eyeL.SetPosition(1, eyeTargetL.transform.position + new Vector3(0, -2));
        if (eyeR.enabled)
            eyeR.SetPosition(1, eyeTargetR.transform.position + new Vector3(0, -2));

        var lDir = eyeTargetL.transform.position - eyeL.transform.position;
        var lDist = Vector3.Distance(eyeL.transform.position, eyeTargetL.transform.position);
        Debug.DrawLine(eyeL.transform.position, eyeL.transform.position + lDir * lDist, Color.red);

        eyeL.transform.LookAt(eyeTargetL.transform.position, -transform.forward);
        //eyeTargetL.transform.LookAt(eyeL.transform, transform.forward);*/

        float animSpeedMult = (halfway ? secondAnimSpeed : initAnimSpeed) * GameManager.instance.speed;
        beams.SetFloat("speed", animSpeedMult);



        switch (state)
        {
            case VenusState.IDLE:
                UpdateIdle();
                break;

            case VenusState.ANGRY:
                UpdateAngry();
                break;

            case VenusState.ENTER:
                UpdateEnter();
                break;

            case VenusState.DYING:
                UpdateDying();
                break;

            case VenusState.ATTACK1:
                break;

            case VenusState.ATTACK2:
                break;

            case VenusState.ATTACK3:
                break;
        }
    }

    void UpdateIdle()
    {
        elapsedSinceLastAttack += Time.deltaTime * GameManager.instance.speed;

        int range = 2;
        if (halfway)
            range = 3;

        if (elapsedSinceLastAttack > timeBetweenShots)
        {
            int r = Random.Range(0, range);

            switch (r)
            {
                case 0:
                    DoIdleBlasts();
                    break;
                case 1:
                    DoSweep();
                    break;
                case 2:
                    DoCross();
                    break;
            }

            SfxManager.instance.PlaySFX(attack);
            state = VenusState.ATTACK1;
            //sprite.SetTrigger("attack");
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

        transform.position -= 2.0f * Vector3.up * Time.deltaTime * GameManager.instance.speed;

        if (elapsedSinceLastAttack > 3f)
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
        state = VenusState.IDLE;
    }

    void SwitchToAngry()
    {
        SfxManager.instance.PlaySFX(angry, true);
        sprite.SetTrigger("angry");
        state = VenusState.ANGRY;
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
        state = VenusState.DYING;
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


    void DoIdleBlasts()
    {
        beams.SetTrigger("short");
        GameManager.instance.AddTimer(0.25f, PewPew);
        GameManager.instance.AddTimer(0.3f, PewPew);
        GameManager.instance.AddTimer(0.35f, PewPew);
        GameManager.instance.AddTimer(0.4f, PewPew);
        GameManager.instance.AddTimer(0.45f, PewPew);
        GameManager.instance.AddTimer(0.5f, PewPew);
        GameManager.instance.AddTimer(2, ResetAttackTime);
    }

    void DoSweep()
    {
        beams.SetTrigger("sweep");
        GameManager.instance.AddTimer(0.25f, PewPew);
        GameManager.instance.AddTimer(0.3f, PewPew);
        GameManager.instance.AddTimer(0.35f, PewPew);
        GameManager.instance.AddTimer(0.4f, PewPew);
        GameManager.instance.AddTimer(0.45f, PewPew);
        GameManager.instance.AddTimer(0.5f, PewPew);
        GameManager.instance.AddTimer(2, ResetAttackTime);
    }

    void DoCross()
    {
        beams.SetTrigger("cross");
        GameManager.instance.AddTimer(0.25f, PewPew);
        GameManager.instance.AddTimer(0.3f, PewPew);
        GameManager.instance.AddTimer(0.35f, PewPew);
        GameManager.instance.AddTimer(0.4f, PewPew);
        GameManager.instance.AddTimer(0.45f, PewPew);
        GameManager.instance.AddTimer(0.5f, PewPew);
        GameManager.instance.AddTimer(2, ResetAttackTime);
        
    }

    private void ResetAttackTime()
    {
        elapsedSinceLastAttack = 0;
        state = VenusState.IDLE;
    }

    void PewPew()
    {
        SfxManager.instance.PlaySFX(pew, true);
    }
}
