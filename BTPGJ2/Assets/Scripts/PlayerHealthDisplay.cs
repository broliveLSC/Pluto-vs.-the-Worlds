using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthDisplay : MonoBehaviour
{
    public Image[] hearts;
    public Damageable playerHealth;

    bool init = false;
    // Start is called before the first frame update
    void Start()
    {
        //Init();
    }

    public void Init()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i > playerHealth.maxHealth - 1)
            {
                hearts[i].gameObject.SetActive(false);
            }
        }

        init = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
            return;

        for (int i = 0; i < playerHealth.maxHealth; i++)
        {
            if (i > playerHealth.currentHealth - 1)
            {
                hearts[i].enabled = false;
                hearts[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                hearts[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
            }

            else
            {
                hearts[i].enabled = true;
                hearts[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hearts[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
            }
        }
    }
}
