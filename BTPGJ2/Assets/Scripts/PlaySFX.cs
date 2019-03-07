using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public AudioClip clip;
    public bool interrupt = false;
    public bool loop = false;
    // Start is called before the first frame update
    void Start()
    {
        SfxManager.instance.PlaySFX(clip, interrupt, loop);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
