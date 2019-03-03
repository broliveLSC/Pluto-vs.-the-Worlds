using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Jump") || Input.GetButton("Fire1"))
        {
            Confirm();
        }
    }

    public void Confirm()
    {
        ReturnToMenu();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("title");
    }
}
