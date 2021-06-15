using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingBeast : MonoBehaviour
{
    private int total = 0;
    private string chosenBeast = "Azdaha";

    public void UpdateTotals(int value)
    {
        total += value;

        if(total <= -2)
        {
            chosenBeast = "MotherPenance";
        }
        else if (total >= 2)
        {
            chosenBeast = "Azdaha";
        }
        else
        {
            chosenBeast = "Terraos";
        }
        unlock(chosenBeast);
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
