using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingBeast : MonoBehaviour
{
    private int total;
    public string chosenBeast;

    private void Start()
    {
        total = 0;
        chosenBeast = "Azdaha";
    }

    public void UpdateTotals(int value)
    {
        total += value;
        print(total);

        if(total <= -3)
        {
            chosenBeast = "MotherPenance";
        }
        else if (total >= 3)
        {
            chosenBeast = "Azdaha";
        }
        else
        {
            chosenBeast = "Terraos";
        }

        print(chosenBeast);
    }
}
