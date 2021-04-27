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
    }
}
