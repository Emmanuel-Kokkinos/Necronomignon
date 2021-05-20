using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//checks which levels are availible and sends the string to mission list
public class LevelChecker : MonoBehaviour
{
    static public int levels = 0;
    static public int tourLevels = 0;

    static public string lastClick;

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Level");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    //sets the lastclick string to whichever value was saved in the button clicked
    public void setLastClick(string levelName)
    {
        lastClick = levelName;
    }
    //unlocks the following level
    public void Progess(string levelName)
    {
        //Tournament
        if (lastClick == "John" && tourLevels <= 0)
        {
            tourLevels++;
            CampaignManager.winTourBattle = tourLevels;
        }
        if (lastClick == "DemonChick" && tourLevels <= 1)
        {
            tourLevels++;
            CampaignManager.winTourBattle = tourLevels;
        }
        if (lastClick == "Gabriel" && levels <= 2)
        {
            tourLevels++;
            CampaignManager.winTourBattle = tourLevels;
        }
        if (lastClick == "Boss" && tourLevels <= 3)
        {
            tourLevels = 0;
            levels = 0;
        }

        //Missions of adventure
        if (lastClick == "first" && levels <= 0)
        {
            levels++;
            unlock("Kitsune");
        }
        else if (lastClick == "second" && levels <= 1)
        {
            levels++;
            unlock("Dryad");
        }
        else if (lastClick == "third" && levels <= 2)
        {
            levels++;
            unlock("Wyvern");
        }
        else if (lastClick == "fourth" && levels <= 3)
        {
            levels++;
            unlock("DreamSlime");
        }
        /*else if (lastClick == "sample" && levels <= 4)
        {
            levels++;
        }*/
        else if (lastClick == "randomer" && levels <= 4)
        {
            levels++;
            unlock("LuzuriaAngelicus");
        }
        else if (lastClick == "random" && levels <= 5)
        {
            levels++;
            unlock("ClockworkQueen");
        }
        else if (lastClick == "bottom" && levels <= 6)
        {
            levels++;
            unlock("Zeograth");
        }
        else if (lastClick == "top" && levels <= 7)
        {
            levels++;
            unlock("Mandoro");
        }
        else if(lastClick == "boss" && levels <= 8)
        {
            levels++;
            unlock("Nage");
        }
        else if (lastClick == "even" && levels <= 9)
        {
            levels++;
            unlock("Cthulhu");
        }
        //These will define the tournament levels

    }


    public static void unlock(String name)
    {
        Beast b = new Beast();
        int x = 0;
        for (; x < BeastManager.beastsList.Beasts.Count; x++)
        {
            if (BeastManager.beastsList.Beasts[x].name == name)
            {
                b = BeastManager.beastsList.Beasts[x];
                break;
            }
        }
        if (b.tier == -1)
        {
            BeastManager.beastsList.Beasts[x].tier = 0;
        }
    }
}
