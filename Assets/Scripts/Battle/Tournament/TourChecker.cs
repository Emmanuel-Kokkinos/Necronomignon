
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TourChecker : MonoBehaviour
{
    static public int levels = 0;

    static public string lastClick;

    void Start()
    {

    }
    //sets the lastclick string to whichever value was saved in the button clicked
    public void setLastClick(string levelName)
    {
        lastClick = levelName;
    }
    //unlocks the following level
    public void Progess(string levelName)
    {
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
        else if (lastClick == "fifth" && levels <= 4)
        {
            levels++;
            unlock("LuzuriaAngelicus");
        }
        else if (lastClick == "sixth" && levels <= 5)
        {
            levels++;
            unlock("ClockworkQueen");
        }
        else if (lastClick == "seventh" && levels <= 6)
        {
            levels++;
            unlock("Zeograth");
        }
        else if (lastClick == "final" && levels <= 7)
        {
            levels++;
            unlock("Mandoro");
        }
    }

    public static void unlock(string name)
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
