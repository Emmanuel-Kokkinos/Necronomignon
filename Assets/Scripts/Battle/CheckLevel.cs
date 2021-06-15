using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//checks which levels are availible for the player
public class CheckLevel : MonoBehaviour
{
    public LevelChecker levelChecker;
    public static int lvl;
    // Start is called before the first frame update
    void Start()
    {
        GameObject name = GameObject.Find("LevelData");

        if(name != null)
        {
            levelChecker = name.GetComponent<LevelChecker>();
        }

        //Interface of either the map or the tournament
        if (SceneManager.GetActiveScene().name.Equals("Map"))
            ActiveCampaingInterface();
        else if (SceneManager.GetActiveScene().name.Equals("Tournament"))
            ActiveTourInterface();
       
    }
    //sends a string that represents which beasts should be sent as the enemy squad in mission list
    public void sendString(string str)
    {
        GameObject name = GameObject.Find("LevelData");

        if (name != null)
        {
            levelChecker = name.GetComponent<LevelChecker>();
            levelChecker.setLastClick(str);
        }
    }
    public void setCurrentLevel(int level)
    {
        lvl = level;
        if (ConversationManager.Instance != null)
        {
            print(lvl);
            ConversationManager.Instance.SetInt("Levels", lvl);
        }
    }

    public void ActiveCampaingInterface()
    {
        if (LevelChecker.levels < 1)
        {
            GameObject.Find("btnLevel2").SetActive(false);
        }
        if (LevelChecker.levels < 2)
        {
            GameObject.Find("btnLevel3").SetActive(false);
        }
        if (LevelChecker.levels < 3)
        {
            GameObject.Find("btnLevel4").SetActive(false);
        }
        if (LevelChecker.levels < 4)
        {
            GameObject.Find("btnLevel5").SetActive(false);
        }
        if (LevelChecker.levels < 5)
        {
            GameObject.Find("btnLevel6").SetActive(false);
        }
        if (LevelChecker.levels < 6)
        {
            GameObject.Find("btnLevel7").SetActive(false);
        }
        if (LevelChecker.levels < 7)
        {
            GameObject.Find("btnLevel8").SetActive(false);
        }
        if (LevelChecker.levels < 8)
        {
            GameObject.Find("btnLevel9").SetActive(false);
        }
        if (LevelChecker.levels < 9)
        {
            GameObject.Find("btnLevel10").SetActive(false);
        }
    }

    public void ActiveTourInterface()
    {
        if (LevelChecker.tourLevels < 1)
        {
            GameObject.Find("btnTour2").SetActive(false);
        }
        if (LevelChecker.tourLevels < 2)
        {
            GameObject.Find("btnTour3").SetActive(false);
        }
        if (LevelChecker.tourLevels < 3)
        {
            GameObject.Find("btnBoss").SetActive(false);
        }
    }
}
