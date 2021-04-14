using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Versioning;

/*
 * Handles the Json Parsing for Beasts moves in Move.json
 */
 
[System.Serializable]
public class MoveManager : MonoBehaviour
{
    string path;
    string jsonString;

    public MoveList movesList = new MoveList();

    // Start is called before the first frame update
    void Start()
    {
        //Parse through the Json file and put the moves in a list. 
        path = Application.streamingAssetsPath + "/Move.json";
        jsonString = File.ReadAllText(path);
        movesList = JsonUtility.FromJson<MoveList>(jsonString);
    }

    public void start()
    {
        Start();
    }
}
