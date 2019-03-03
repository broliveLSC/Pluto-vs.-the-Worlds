using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public int totalBosses = 5;
    public List<int> unbeaten = new List<int>();
    public List<GameObject> bosses = new List<GameObject>();

    public List<GameObject> bosses_all = new List<GameObject>();
    public List<Sprite> icons = new List<Sprite>();

    bool started = false;

    int boss_num = 0;

    public EnemyHealthDisplay enemyHealth;
    public GameObject enemyIcon;
    public GameObject iconBg;

    public GameObject currentBoss;
    public PlanetMenu pm;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < totalBosses; i++)
        {
            // add index of each boss; 1= mercury, 2 = venus, etc.
            unbeaten.Add(i);
        }

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
        if(!started)
        {
            //GameManager.instance.AddTimer(2, ChooseRandom);
            started = true;
        }
    }

    public void ChooseRandom()
    {
        //int r = Random.Range(0, unbeaten.Count);
        int r = Random.Range(0, bosses.Count);
        //int choice = unbeaten[r];
        Debug.Log("R is " + r);
        boss_num = r;
        //SpawnBoss(choice);
        SpawnBoss(r);
    }

    public void SpawnBoss(int index)
    {
        pm.gameObject.SetActive(false);
        // qqq
        //currentBoss = Instantiate(bosses[index]);

        int d_index = -1;

        foreach (GameObject g in bosses)
        {
            if (g.GetComponent<Mercury>() != null)
            {
                if (index == g.GetComponent<Mercury>().bossIndex)
                {
                    d_index = index;
                    //icons.RemoveAt(index);
                    //bosses.RemoveAt(index);
                }
            }

            if (g.GetComponent<Venus>() != null)
            {
                if (index == g.GetComponent<Venus>().bossIndex)
                {
                    d_index = index;
                    //icons.RemoveAt(index);
                    //bosses.RemoveAt(index);
                }
            }

            if (g.GetComponent<Saturn>() != null)
            {
                if (index == g.GetComponent<Saturn>().bossIndex)
                {
                    d_index = index;
                    //icons.RemoveAt(index);
                    //bosses.RemoveAt(index);
                }
            }

            if (g.GetComponent<Uranus>() != null)
            {
                if (index == g.GetComponent<Uranus>().bossIndex)
                {
                    d_index = index;
                    //icons.RemoveAt(index);
                    //bosses.RemoveAt(index);
                }
            }
            if (g.GetComponent<Neptune>() != null)
            {
                if (index == g.GetComponent<Neptune>().bossIndex)
                {
                    d_index = index;
                    //icons.RemoveAt(index);
                    //bosses.RemoveAt(index);
                }
            }

        }

        Debug.Log("Spawn boss with index " + d_index);
        currentBoss = Instantiate(bosses_all[d_index]); //qqq

        // set health bar

        enemyHealth.gameObject.SetActive(true);
        iconBg.SetActive(true);
        enemyIcon.SetActive(true);
        enemyHealth.enemyHealth = currentBoss.GetComponent<Damageable>();
        enemyHealth.icon.sprite = icons[d_index]; //qqq

        if(RunSettings.instance != null && RunSettings.instance.healAfterFight)
            FindObjectOfType<PlayerController>().GetComponent<Damageable>().currentHealth = FindObjectOfType<PlayerController>().GetComponent<Damageable>().maxHealth;
    }

    public void BeatBoss(int index)
    {
        enemyHealth.gameObject.SetActive(false);
        iconBg.SetActive(false);
        enemyIcon.SetActive(false);

        unbeaten.Remove(index);
        //bosses.RemoveAt(index);

        //GameObject toDestroy = null;
        int d_index = -1;

        foreach(GameObject g in bosses)
        {
            if(g.GetComponent<Mercury>() != null)
            {
                if(index == g.GetComponent<Mercury>().bossIndex)
                {
                    d_index = index;
                    //icons.RemoveAt(index);
                    //bosses.RemoveAt(index);
                }
            }

            if (g.GetComponent<Venus>() != null)
            {
                if (index == g.GetComponent<Venus>().bossIndex)
                {
                    d_index = index;
                    //icons.RemoveAt(index);
                    //bosses.RemoveAt(index);
                }
            }

            if (g.GetComponent<Saturn>() != null)
            {
                if (index == g.GetComponent<Saturn>().bossIndex)
                {
                    d_index = index;
                    //icons.RemoveAt(index);
                    //bosses.RemoveAt(index);
                }
            }

            if (g.GetComponent<Uranus>() != null)
            {
                if (index == g.GetComponent<Uranus>().bossIndex)
                {
                    d_index = index;
                    //icons.RemoveAt(index);
                    //bosses.RemoveAt(index);
                }
            }
            if (g.GetComponent<Neptune>() != null)
            {
                if (index == g.GetComponent<Neptune>().bossIndex)
                {
                    d_index = index;
                    //icons.RemoveAt(index);
                    //bosses.RemoveAt(index);
                }
            }

        }

        bosses.RemoveAt(d_index);
        icons.RemoveAt(d_index);

        if (RunSettings.instance != null && RunSettings.instance.chooseEnemies)
            pm.gameObject.SetActive(true);

        else//if (RunSettings.instance != null && !RunSettings.instance.chooseEnemies)
            GameManager.instance.AddTimer(2.0f, ChooseRandom);
    }
    
}
