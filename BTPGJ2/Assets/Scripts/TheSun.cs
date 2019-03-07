using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheSun : MonoBehaviour
{
    public enum TheSunState
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

    public TheSunState state = TheSunState.ENTER;

    public Animator sprite;

    public float timeBetweenShots = 2.0f;

    public float elapsedSinceLastAttack = 0.0f;

    // attacks
    public GameObject smallSplosion;
    public GameObject leftWave;
    public GameObject downWave;
    public GameObject bigWave;

    // stuff for all bosses
    public GameObject soundBlast;
    public GameObject splosion;

    public bool halfway = false;
    bool fall = false;
    private float splosionTimer = 0;

    public float initHealth = 60;
    public int bossIndex = 0;

    public Animator redGiant;
    public float redMult = 1;

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
        redGiant.SetFloat("game_speed", GameManager.instance.speed * redMult);
        switch (state)
        {
            case TheSunState.IDLE:
                UpdateIdle();
                break;

            case TheSunState.ANGRY:
                UpdateAngry();
                break;

            case TheSunState.ENTER:
                UpdateEnter();
                break;

            case TheSunState.DYING:
                UpdateDying();
                break;

            case TheSunState.ATTACK1:
                break;

            case TheSunState.ATTACK2:
                break;

            case TheSunState.ATTACK3:
                break;
        }
    }

    void UpdateIdle()
    {
        elapsedSinceLastAttack += Time.deltaTime * GameManager.instance.speed;

        if (elapsedSinceLastAttack > timeBetweenShots)
        {
            int r = Random.Range(0, 3);

            switch (r)
            {
                case 0:
                    CreateLeftWave();
                    break;
                case 1:
                    CreateDownWave();
                    break;
                case 2:
                    CreateSplosionCharge();
                    break;
                case 3:
                    CreateBigWave();
                    break;
            }

            state = TheSunState.ATTACK1;
            //sprite.SetTrigger("attack");
        }

        if (!halfway)
        {
            float diff = GetComponent<Damageable>().maxHealth - GetComponent<Damageable>().currentHealth;
            if (diff >= GetComponent<Damageable>().maxHealth * 0.5f)
            {
                halfway = true;
                timeBetweenShots = timeBetweenShots * 0.8f;
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

        transform.position += 2.0f * -Vector3.right * Time.deltaTime * GameManager.instance.speed;

        if (elapsedSinceLastAttack > 5f)
        {
            Instantiate(myTitle);
            SwitchToIdle();
            elapsedSinceLastAttack = 0;
        }
    }

    void UpdateAngry()
    {

    }

    void UpdateAttack()
    {

    }

    void CreateLeftWave()
    {
        leftWave.GetComponent<LeftWaveSpawner>().Activate();
        GameManager.instance.AddTimer(4, leftWave.GetComponent<LeftWaveSpawner>().Deactivate);
        GameManager.instance.AddTimer(3, ResetAttackTime);
    }

    void CreateDownWave()
    {
        downWave.GetComponent<LeftWaveSpawner>().Activate();
        GameManager.instance.AddTimer(4, downWave.GetComponent<LeftWaveSpawner>().Deactivate);
        GameManager.instance.AddTimer(3, ResetAttackTime);
    }

    void CreateSplosionCharge()
    {
        smallSplosion.GetComponent<LeftWaveSpawner>().Activate();
        GameManager.instance.AddTimer(4, smallSplosion.GetComponent<LeftWaveSpawner>().Deactivate);
        GameManager.instance.AddTimer(3, ResetAttackTime);
    }

    void CreateBigWave()
    {

    }

    void StartFalling()
    {
        fall = true;
    }

    void UpdateDying()
    {
        /*GameObject.Find("Player").GetComponent<Damageable>().invulnerable = true;
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
        }*/
    }

    void SwitchToIdle()
    {
        //sprite.SetTrigger("idle");
        state = TheSunState.IDLE;

        if (halfway)
        {

        }
    }

    void SwitchToAngry()
    {
        sprite.SetTrigger("angry");
        state = TheSunState.ANGRY;
        GameManager.instance.AddTimer(1, MakeCameraShake);
        GameManager.instance.AddTimer(2, SwitchToIdle);

        GameManager.instance.AddTimer(1f, AddSoundBlast);
        GameManager.instance.AddTimer(1.25f, AddSoundBlast);
        GameManager.instance.AddTimer(1.4f, AddSoundBlast);
        GameManager.instance.AddTimer(1.65f, AddSoundBlast);
        GameManager.instance.AddTimer(1.8f, AddSoundBlast);
        GameManager.instance.AddTimer(1.95f, AddSoundBlast);
    }

    public void SwitchToDying()
    {
        state = TheSunState.DYING;
        /*foreach (SpinningRing r in FindObjectsOfType<SpinningRing>())
        {
            r.End();
        }*/
        leftWave.GetComponent<LeftWaveSpawner>().Deactivate();
        downWave.GetComponent<LeftWaveSpawner>().Deactivate();
        smallSplosion.GetComponent<LeftWaveSpawner>().Deactivate();
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
        state = TheSunState.IDLE;
    }
}
