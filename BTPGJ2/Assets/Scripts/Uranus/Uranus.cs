using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uranus : MonoBehaviour
{
    public enum UranusState
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

    public UranusState state = UranusState.ENTER;

    public Animator sprite;

    public float timeBetweenShots = 2.0f;

    public float elapsedSinceLastAttack = 0.0f;

    // attacks
    public GameObject gasFollower;
    public GameObject gasSpawner1;
    public GameObject gasSpawner2;

    // stuff for all bosses
    public GameObject soundBlast;
    public GameObject splosion;

    bool halfway = false;
    bool fall = false;
    private float splosionTimer = 0;

    public float initHealth = 60;
    public int bossIndex = 0;

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
            case UranusState.IDLE:
                UpdateIdle();
                break;

            case UranusState.ANGRY:
                UpdateAngry();
                break;

            case UranusState.ENTER:
                UpdateEnter();
                break;

            case UranusState.DYING:
                UpdateDying();
                break;

            case UranusState.ATTACK1:
                break;

            case UranusState.ATTACK2:
                break;

            case UranusState.ATTACK3:
                break;
        }
    }

    void UpdateIdle()
    {
        elapsedSinceLastAttack += Time.deltaTime * GameManager.instance.speed;

        if (elapsedSinceLastAttack > timeBetweenShots)
        {
            int r = Random.Range(0, 2);

            switch (r)
            {
                case 0:
                    CreateFollower();
                    break;
                case 1:
                    CreateSpawner(1);
                    break;
                case 2:
                    CreateSpawner(2);
                    break;
            }

            state = UranusState.ATTACK1;
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

    void CreateFollower()
    {
        GasFollower gs = Instantiate(gasFollower).GetComponent<GasFollower>();
        gs.spacing = halfway ? 0.35f : 0.7f;
        GameManager.instance.AddTimer(4.2f, ResetAttackTime);
    }

    void CreateSpawner(int index)
    {
        switch(index)
        {
            case 1:
                Instantiate(gasSpawner1);
                break;
            case 2:
                Instantiate(gasSpawner2);
                break;
        }
        GameManager.instance.AddTimer(2f, ResetAttackTime);
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
        state = UranusState.IDLE;

        if(halfway)
        {

        }
    }

    void SwitchToAngry()
    {
        sprite.SetTrigger("angry");
        state = UranusState.ANGRY;
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
        state = UranusState.DYING;
        foreach (SpinningRing r in FindObjectsOfType<SpinningRing>())
        {
            r.End();
        }
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
        state = UranusState.IDLE;
    }
}
