using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Loads the demo battle scene with the correct squad
 */
public class LoadDemoBattle : MonoBehaviour
{
    public SquadData squadData;
    public LoadScenes loadScenes;

    public static int squadNo;

    // Loads the demo battle scene with the correct squad
    public void SetSquadNumber(int number)
    {
        squadNo = number;

        if(squadNo == 1)
        {
            if (squadData.GetSquad1Status())
            {
                loadScenes.LoadSelect("DemoBattle");
            }
        }
        else
        {
            if (squadData.GetSquad2Status())
            {
                loadScenes.LoadSelect("DemoBattle");
            }
        }
    }

    // Returns the squad number of the current squad
    public int GetSquadNumber()
    {
        return squadNo;
    }
}
