using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner 
{
    public int xp = 1;
    public int level = 1;
    public int xpNeeded;

    public int getLevel()
    {
        updateLevel();
        return level;
    }

    public void addXP(int newxp)
    {
        if(newxp < 1)
        {
            xp++;
            return;
        }
        xp += newxp;
        updateLevel();
    }

    public void updateLevel()
    {
        xpNeeded = (int)Math.Round((4 * (Math.Pow((double)level , (double)3))) / 5);
        while(this.xp > xpNeeded)
        {
            if (level < 100)
            {
                level++;
                xpNeeded = (int)Math.Round((4 * (Math.Pow((double)level, (double)3))) / 5);
            }
            else
            {
                break;
            }
        }
    }

    public int xpForLevel(int x)
    {
        return (int)Math.Round((4 * (Math.Pow((double)x, (double)3))) / 5);
    }
}
