using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetMenu : MonoBehaviour
{
    public Transform[] planets;
    public int index = 0;

    public PlayerController player;

    bool canMove = true;

    //public BossManager manager;
    public BossMenu2 manager2;

    public AudioClip move;
    public AudioClip click;

    public Text bossName;

    string[] names = { "Mercury", "Venus", "Saturn", "Uranus", "Neptune" };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        /*while (!manager.unbeaten.Contains(index))
        {
            index += (int)Mathf.Sign(Input.GetAxis("Horizontal"));

            if (index < 0)
            {
                index = planets.Length - 1;
            }

            if (index > planets.Length - 1)
            {
                index = 0;
            }

        } */

        index = 0;
        GameObject toAdd = manager2.all_planets[index];

        //if (Input.GetAxis("Horizontal") != 0 && canMove)
        {

            while (manager2.defeated.Contains(toAdd))
            {
                index += 1;

                if (index < 0)
                {
                    index = planets.Length - 1;
                }

                if (index > planets.Length - 1)
                {
                    index = 0;
                }

                toAdd = manager2.all_planets[index];


            } 
            

        }
    }

    // Update is called once per frame
    void Update()
    {
        player.canInput = false;
        bossName.text = names[index];
        GameObject toAdd = null;

        if (Input.GetAxis("Horizontal") != 0 && canMove)
        {
            SfxManager.instance.PlaySFX(move);
            do
            {
                index += (int)Mathf.Sign(Input.GetAxis("Horizontal"));

                if (index < 0)
                {
                    index = planets.Length - 1;
                }

                if (index > planets.Length - 1)
                {
                    index = 0;
                }

                toAdd = manager2.all_planets[index];
                

            } while (manager2.defeated.Contains(toAdd));

            canMove = false;

            

            
        }

        if(Input.GetAxis("Horizontal") == 0)
        {
            canMove = true;
        }

        for(int i = 0; i < planets.Length; i++)
        {
            if(index == i)
            {
                planets[i].localScale = new Vector3(1,1,1) * 1.5f;
            }

            else
            {
                planets[i].localScale = new Vector3(1, 1, 1) * 1f;
            }
        }

        if(Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1"))
        {
            // re-enable input
            player.canInput = true;

            // spawn selected planet
            manager2.SpawnBoss(index);

            SfxManager.instance.PlaySFX(click);

            // destroy this
            //Destroy(gameObject);
        }
    }
}
