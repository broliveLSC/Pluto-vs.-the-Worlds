using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGiant : MonoBehaviour
{
    public TheSun sun;
    public GameObject whiteDwarf;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(7, UnParent);
    }

    void UnParent()
    {
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Halfway()
    {
        Debug.Log("HALFWAY");
        sun.halfway = true;
    }

    public void EndBattle()
    {
        FindObjectOfType<PlayerController>().GetComponent<Damageable>().invulnerable = true;
        Debug.Log("EndBattle");
        //sun.state = TheSun.TheSunState.DYING;
        sun.SwitchToDying();
        sun.redMult = 3;
    }

    public void CoolDown()
    {
        //Destroy(sun.gameObject);
        sun.gameObject.SetActive(false);
        Instantiate(whiteDwarf);
        GetComponent<Animator>().SetTrigger("decrease");
    }
}
