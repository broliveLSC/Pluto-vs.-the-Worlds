using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JupiterMoon : MonoBehaviour
{
    public Transform orbitTarget;

    public enum MoonState
    {
        idle,
        prepare,
        shoot,
        ricochet,
        go_back
    }

    public MoonState state = MoonState.idle;

    public float shootSpeed = 10.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent = null;

        switch(state)
        {
            case MoonState.idle:
                transform.position = orbitTarget.position;
                break;
            case MoonState.prepare:
                /*var direction = (FindObjectOfType<PlayerController>().transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);*/
                Vector2 dir =  FindObjectOfType<PlayerController>().transform.position - transform.position;
                transform.right = dir;
                break;
            case MoonState.shoot:
                transform.parent = null;
                transform.position += transform.right * shootSpeed * 1.5f * Time.deltaTime * GameManager.instance.speed;
                break;
            case MoonState.ricochet:
                transform.parent = null;
                transform.position -= transform.right * shootSpeed * 2f * Time.deltaTime * GameManager.instance.speed;
                break;
            case MoonState.go_back:
                //transform.parent = orbit;
                transform.position = Vector3.Lerp(transform.position, orbitTarget.position, 3f * Time.deltaTime);
                if(transform.position == orbitTarget.position)
                {
                    state = MoonState.idle;
                }
                break;
        }
    }

    
    public void SwitchToShoot()
    {
        SetState(MoonState.shoot);
    }

    public void SwitchToPrepare()
    {
        SetState(MoonState.prepare);
        GameManager.instance.AddTimer(5.0f, GoBack);
        GameManager.instance.AddTimer(1.0f, SwitchToShoot);
    }

    public void GoBack()
    {
        SetState(MoonState.go_back);
    }

    public void SetState(MoonState newState)
    {
        state = newState;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("AAAAAAA");
        if(collision.gameObject.name == "Platform")
        {
            Debug.Log("HIT PLATFORM");
            if(state == MoonState.shoot)
            {
                SetState(MoonState.ricochet);
                GameManager.instance.AddTimer(3.0f, GoBack);
            }
        }
    }
}
