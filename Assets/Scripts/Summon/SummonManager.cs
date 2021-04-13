using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonManager : MonoBehaviour
{

    public static new string name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        // Search for all games objects in scene with the tag "music", you can edit the GameObject tag in the inspector
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Summon");


        if (objs.Length > 1)
        {
            // This is very IMPORTANT, it keeps the GameObject value to 1, or else you'll just have multiplying GameObjects every time you re-load the scene
            Destroy(this.gameObject);
        }

        // Allows the keep the GameObject from scene to scene
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
