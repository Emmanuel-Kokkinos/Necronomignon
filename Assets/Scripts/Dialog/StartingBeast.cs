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
        chosenBeast = "Conglomerate";
    }

    public void UpdateTotals(int value)
    {
        total += value;
        print(total);

        if(total <= 18)
        {
            chosenBeast = "Conglomerate";
        }
        if (total <= 25)
        {
            chosenBeast = "Kitsune";
        }
        if (total <= 32)
        {
            chosenBeast = "Wyvern";
        }
        if (total <= 40)
        {
            chosenBeast = "Dryad";
        }
    }
}
