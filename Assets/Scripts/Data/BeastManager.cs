﻿using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Versioning;



[System.Serializable]
public class BeastManager : MonoBehaviour
{
    //to uniquly identify each beast
    static int givenId = 0;
    string path;
    string jsonString;
    public MoveManager moveManager;

    public static BeastList beastsList = new BeastList();

    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath + "/Scripts/Data/Beast.json";
        jsonString = File.ReadAllText(path);

       beastsList = JsonUtility.FromJson<BeastList>(jsonString);
     // print(JsonConvert.DeserializeObject(jsonString)); 

        if (jsonString != null && beastsList.Beasts.Count <=0)
        {          
            foreach (Beast beast in beastsList.Beasts)
            {
                // gives each beast a unique id
                beast.id = givenId;
                givenId++;
            }
        }

        foreach(Beast beast in beastsList.Beasts)
        {
            beast.Move_A = getMove(beast.moveA);
            beast.Move_B = getMove(beast.moveB);
        }
    }

    public bool isLoaded()
    {
        if (beastsList.Beasts.Count == 0)
        {
            return false;
        }
        
         return true;
    }

    public void Awake()
    {
        path = Application.dataPath + "/Scripts/Data/Beast.json";
        jsonString = File.ReadAllText(path);

        beastsList = JsonUtility.FromJson<BeastList>(jsonString);

        if (jsonString != null)
        {
            foreach (Beast beast in beastsList.Beasts)
            {
                beast.id = givenId;
                givenId++;
            }
        }

        foreach (Beast beast in beastsList.Beasts)
        {
            beast.Move_A = getMove(beast.moveA);
            beast.Move_B = getMove(beast.moveB);
        }
    }

    public Move getMove(int x)
    {
        List<Move> ml = moveManager.movesList.Moves;
        for(int i = 0; i < ml.Count; i++)
        {
            if(ml[i].move_id == x)
            {
                return ml[i];
            }

        }
        Move mavi = new Move();
        mavi.move_id = 0;
        mavi.name = "struggle";
        mavi.power = 50;
        mavi.condition_chance = .000003f;
        return mavi;
    }

    public Beast getFromName(String str)
    {
        if(beastsList.Beasts.Count <= 0)
        {
            Start();
        }

        Beast b = new Beast();
        for (int x = 0; x < beastsList.Beasts.Count; x++)
        {
            if (str.Equals(beastsList.Beasts[x].name))
            {
                b = new Beast(beastsList.Beasts[x]);
                b.id = givenId;
                givenId++;
                break;
            }
        }
        return b;
    }
}
