using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Versioning;

/*
 *  This class handles the creation of a Beast Object from the Json File    
 *  it also links Moves and Buffs objects to the Beast Object
 */

[System.Serializable]
public class BeastManager : MonoBehaviour
{
    //to uniquly identify each beast
    static int givenId = 0;
    string path;
    string jsonString;
    public MoveManager moveManager;

    public static BeastList beastsList = new BeastList();

    // Start is called before the first frame update, Handles the parsing of Beast.json file. 
    void Start()
    {
        path = Application.streamingAssetsPath+"\\Beast.json";
        jsonString = File.ReadAllText(path);



        if (jsonString != null && beastsList.Beasts.Count <=0)
        {
            beastsList = JsonUtility.FromJson<BeastList>(jsonString);
            foreach (Beast beast in beastsList.Beasts)
            {

                print(beast.statGradients);
                // gives each beast a unique id

                beast.id = givenId;
                givenId++;
            }
        }

        // Gets the Moves and sets them in the right object. 
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
        path = Application.streamingAssetsPath+"\\Beast.json";
        jsonString = File.ReadAllText(path);



        if (jsonString != null && beastsList.Beasts.Count <= 0)
        {
            beastsList = JsonUtility.FromJson<BeastList>(jsonString);
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
        if(moveManager.movesList == null)
        {
            moveManager.movesList = new MoveList();
        }
            List<Move> ml = moveManager.movesList.Moves;

            for (int i = 0; i < ml.Count; i++)
            {
                if (ml[i].move_id == x)
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

    //Returns a beast from the List of Beasts in the game, based on its name 
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
    public static Beast getFromNameS(String str)
    {
        

        Beast b = new Beast();
        if(str == null)
        {
            return b;
        }
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
