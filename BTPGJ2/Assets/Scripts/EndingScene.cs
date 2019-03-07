using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingScene : MonoBehaviour
{
    public GameObject dark;
    public GameObject alone;
    public GameObject was;
    public Text content;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(1, TurnOnDark);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnDark()
    {
        dark.SetActive(true);
        GameManager.instance.AddTimer(2, TurnOnAlone);
    }

    public void TurnOnAlone()
    {
        alone.SetActive(true);
        GameManager.instance.AddTimer(2, TurnOnWas);
    }

    public void TurnOnWas()
    {
        was.SetActive(true);
        GameManager.instance.AddTimer(2, SetContent);
    }

    public void SetContent()
    {
        content.text = "He was... Content.";
        GameManager.instance.AddTimer(1.5f, GameManager.instance.FadeToBlack);
        GameManager.instance.AddTimer(8, ReturnToTitle);
    }

    public void ReturnToTitle()
    {
        // destroy managers that may have made it back to the beginning
        Destroy(FindObjectOfType<GameSettings>().gameObject);
        Destroy(FindObjectOfType<RunSettings>().gameObject);

        GameManager.instance.ReturnToTitle();
    }


}
