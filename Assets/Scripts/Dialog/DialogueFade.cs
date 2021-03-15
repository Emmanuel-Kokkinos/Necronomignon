using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fade()
    {
        Color colour = Color.black;

        Initiate.Fade("DialogScene", colour, 1.5f);
    }
}
