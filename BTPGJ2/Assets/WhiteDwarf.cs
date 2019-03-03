using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhiteDwarf : MonoBehaviour
{
    public float glow;
    public SpriteRenderer glowSprite;
    public SpriteRenderer darkness;

    bool end = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindObjectOfType<PlayerController>().GetComponent<Damageable>().invulnerable = true;

        glow = GetComponent<Damageable>().currentHealth / GetComponent<Damageable>().maxHealth;

        glowSprite.color = new Color(1, 1, 1, glow);

        darkness.color = new Color(0, 0, 0, 1.0f - glow);

        if(GetComponent<Damageable>().currentHealth <= 0 && !end)
        {
            end = true;
            GameManager.instance.FadeToBlack();
            GameManager.instance.AddTimer(3, GoToEnding);
        }
    }

    void GoToEnding()
    {
        SceneManager.LoadScene("ending");
    }
}
