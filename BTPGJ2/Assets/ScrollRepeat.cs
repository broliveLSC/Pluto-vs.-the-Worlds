using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollRepeat : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    public Transform[] pieces;

    public Transform boundary;
    public float offset = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < pieces.Length; i++)
        {
            pieces[i].position -= new Vector3(scrollSpeed * Time.deltaTime, 0, 0);

            if(pieces[i].position.x < boundary.position.x)
            {
                ResetPiece(i);
            }
        }
    }

    void ResetPiece(int index)
    {
        int prev = index - 1;
        if (prev < 0)
            prev = pieces.Length - 1;

        pieces[index].position = pieces[prev].position + new Vector3(offset, 0, 0);
    }
}
