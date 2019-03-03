using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunSettings : MonoBehaviour
{
    public static RunSettings instance;

    public bool chooseEnemies = true; // choose or random
    public bool healAfterFight = true; // heal or no heal
    public bool canRetry = true; // retry or game over

    public float playerHealth;
    public float gameSpeed;
    public float enemyHealthMult;

    public Slider playerHealthSlider;
    public Slider enemyHealthSlider;
    public Slider gameSpeedSlider;

    public Text playerHealthText;
    public Text enemyHealthText;
    public Text gameSpeedText;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGameSpeedText();
        UpdatePlayerHealthText();
        UpdateEnemyHealthText();
    }

    void UpdatePlayerHealthText()
    {
        if (playerHealthSlider == null)
            return;

        playerHealth = playerHealthSlider.value;
        playerHealthText.text = playerHealthSlider.value.ToString();
    }

    void UpdateEnemyHealthText()
    {
        if (enemyHealthSlider == null)
            return;

        switch (enemyHealthSlider.value)
        {
            case 1:
                enemyHealthMult = 0.25f;
                break;
            case 2:
                enemyHealthMult = 0.5f;
                break;
            case 3:
                enemyHealthMult = 1;
                break;
            case 4:
                enemyHealthMult = 1.5f;
                break;
            case 5:
                enemyHealthMult = 2f;
                break;
        }
        enemyHealthText.text = "X " + enemyHealthMult;
    }

    void UpdateGameSpeedText()
    {
        if (gameSpeedSlider == null)
            return;

        switch (gameSpeedSlider.value)
        {
            case 1:
                gameSpeed = 0.5f;
                break;
            case 2:
                gameSpeed = 0.75f;
                break;
            case 3:
                gameSpeed = 1;
                break;
            case 4:
                gameSpeed = 1.25f;
                break;
            case 5:
                gameSpeed = 1.5f;
                break;
        }
        gameSpeedText.text = "X " + gameSpeed;
    }
}
