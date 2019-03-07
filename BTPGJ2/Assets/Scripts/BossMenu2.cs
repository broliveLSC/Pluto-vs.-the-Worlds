using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BossMenu2 : MonoBehaviour
{
    public PlanetMenu pm;

    public GameObject[] all_planets;
    public List<GameObject> defeated;

    // hud
    public List<Sprite> icons = new List<Sprite>();
    public EnemyHealthDisplay enemyHealth;
    public GameObject enemyIcon;
    public GameObject iconBg;

    GameObject currentBoss;
    int currentBossIndex = 0;

    public GameObject theSun;
    bool fightTheSun = false;

    // Start is called before the first frame update
    void Start()
    {
        if (RunSettings.instance != null && RunSettings.instance.chooseEnemies)
            GameManager.instance.AddTimer(2.0f, AddPlanetMenu);

        else//if (RunSettings.instance != null && !RunSettings.instance.chooseEnemies)
            GameManager.instance.AddTimer(2.0f, ChooseRandom);
    }

    void AddPlanetMenu()
    {
        pm.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChooseRandom()
    {
        //int r = Random.Range(0, unbeaten.Count);
        int r = -1;

        // keep choosing a random boss until we find one that hasn't been defeated
        do
        {
            r = Random.Range(0, all_planets.Length);
        } while (defeated.Contains(all_planets[r]));

        
        //SpawnBoss(choice);
        SpawnBoss(r);
    }

    public void BeatBoss(int index)
    {
        defeated.Add(all_planets[index]);
        GameManager.instance.HideEnemyHealthBar();

        if(defeated.Count == 5)
        {
            // beat everyone, so now fight the sun
            FindObjectOfType<PlayerController>().GetComponent<Damageable>().invulnerable = false;
            Instantiate(theSun);
            //EditorApplication.isPaused = true;
            currentBoss = theSun;
            //EditorApplication.isPaused = true;
            fightTheSun = true;
            //EditorApplication.isPaused = true;
            return;
        }

        if (RunSettings.instance != null && RunSettings.instance.chooseEnemies)
            pm.gameObject.SetActive(true);

        else//if (RunSettings.instance != null && !RunSettings.instance.chooseEnemies)
            GameManager.instance.AddTimer(2.0f, ChooseRandom);
    }

    public void SpawnBoss(int index)
    {
        FindObjectOfType<PlayerController>().GetComponent<Damageable>().invulnerable = false;

        GameManager.instance.ShowEnemyHealthBar();

        pm.gameObject.SetActive(false);
        currentBoss = Instantiate(all_planets[index]);
        currentBossIndex = index;

        enemyHealth.gameObject.SetActive(true);
        iconBg.SetActive(true);
        enemyIcon.SetActive(true);
        enemyHealth.enemyHealth = currentBoss.GetComponent<Damageable>();
        enemyHealth.icon.sprite = icons[index]; //qqq

        if (RunSettings.instance != null && RunSettings.instance.healAfterFight)
            FindObjectOfType<PlayerController>().GetComponent<Damageable>().currentHealth = FindObjectOfType<PlayerController>().GetComponent<Damageable>().maxHealth;

    }

    public void DestroyCurrentBoss()
    {
        if (currentBoss == null)
            return;

        GameManager.instance.HideEnemyHealthBar();

        //Destroy(currentBoss);
        currentBoss.SetActive(false);
        if(fightTheSun)
        {
            //Destroy(FindObjectOfType<RedGiant>().gameObject);
            //FindObjectOfType<RedGiant>().gameObject.SetActive(false);
        }
    }

    public void RespawnBoss()
    {
        if(!fightTheSun)
            SpawnBoss(currentBossIndex);

        else
        {
            Instantiate(theSun);
            currentBoss = theSun;
            fightTheSun = true;
            GameManager.instance.HideEnemyHealthBar();
        }
    }
}
