using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CheckTournament : MonoBehaviour
{
    public TourChecker levelChecker;
    public static int lvl;
    // Start is called before the first frame update
    void Start()
    {
        GameObject name = GameObject.Find("GameManager");

        if (name != null)
        {
            levelChecker = name.GetComponent<TourChecker>();
        }

        CheckTournamentMission();
    }
    //sends a string that represents which beasts should be sent as the enemy squad in mission list

    public void sendString(string str)
    {
        GameObject name = GameObject.Find("LevelData");

        if (name != null)
        {
            levelChecker = name.GetComponent<TourChecker>();
            levelChecker.setLastClick(str);
        }
    }

    public void CheckTournamentMission()
    {
        if (TourChecker.levels < 1)
        {
            GameObject.Find("btnTour2").SetActive(false);
        }
        if (TourChecker.levels < 2)
        {
            GameObject.Find("btnTour3").SetActive(false);
        }
        if (TourChecker.levels < 3)
        {
            GameObject.Find("btnTour4").SetActive(false);
        }
        if (TourChecker.levels < 4)
        {
            GameObject.Find("btnTour5").SetActive(false);
        }
        if (TourChecker.levels < 5)
        {
            GameObject.Find("btnTour6").SetActive(false);
        }
        if (TourChecker.levels < 6)
        {
            GameObject.Find("btnTour7").SetActive(false);
        }
        if (TourChecker.levels < 7)
        {
            GameObject.Find("btnFinal").SetActive(false);
        }
    }

    //To manage internal dialogues in battle in case it is wanted
    /*
    public void setCurrentLevel(int level)
    {
        lvl = level;
        if (ConversationManager.Instance != null)
        {
            print(lvl);
            ConversationManager.Instance.SetInt("Levels", lvl);
        }
    }
    */


}
