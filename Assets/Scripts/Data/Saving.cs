using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Saving : MonoBehaviour
{

    public static GameObject loadSaveDialog;
    public static GameObject loadSaveText;
    public static Text txtLoadSave;

    // Start is called before the first frame update
    void Start()
    {
        //Variables for dialog box when game is loaded or saved
        loadSaveDialog = GameObject.Find("loadSaveDialog");
        loadSaveText = GameObject.Find("loadSaveText");

        if (loadSaveText != null)
        {
            txtLoadSave = (Text)loadSaveText.GetComponent(typeof(Text));
        }
        
        if(loadSaveDialog != null)
            loadSaveDialog.SetActive(false);

        //loadSaveDialog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void saveAll()
    {
        EasySave.Save<BeastList>("playerBL",BeastManager.beastsList);
        EasySave.Save<int>("level", LevelChecker.levels);
        EasySave.Save<int>("winTour", CampaignManager.winTourBattle);
        List<Beast> failsafe = new List<Beast>();
      /*  while (failsafe.Count < 11)
        {
            failsafe.Add(null);
        }*/
        if (SquadData.squad1Saved)
        {
            EasySave.Save<List<Beast>>("squad1",SquadData.squad1);
        }
        else
        {
            EasySave.Save<List<Beast>>("squad1", failsafe);
        }
        if (SquadData.squad2Saved)
        {
            EasySave.Save<List<Beast>>("squad2", SquadData.squad2);
        }
        else
        {
            EasySave.Save<List<Beast>>("squad2", failsafe);
        }
        EasySave.Save<int>("playerXP", Player.summoner.xp);

        if (txtLoadSave != null && loadSaveDialog != null)
        {
            //Dialog box when game is saved
            txtLoadSave.text = "The Game Has Been Saved!";
            loadSaveDialog.SetActive(true);
        }
            
            
    }
    public static void loadAll()
    {
        BeastManager.beastsList = EasySave.Load<BeastList>("playerBL");
        LevelChecker.levels = EasySave.Load<int>("level");
        if (EasySave.Load<List<Beast>>("squad1") != null && EasySave.Load<List<Beast>>("squad1").Count > 0)
        {
            SquadData.squad1 = EasySave.Load<List<Beast>>("squad1");
            SquadData.squad1Saved = true;
        }
        else
        {
            SquadData.squad1Saved = false;
        }
        if (EasySave.Load<List<Beast>>("squad2") != null && EasySave.Load<List<Beast>>("squad2").Count > 0)
        {
            SquadData.squad2 = EasySave.Load<List<Beast>>("squad2");
            SquadData.squad2Saved = true;
        }
        else
        {
            SquadData.squad2Saved = false;
        }
        Player.summoner.xp = EasySave.Load<int>("playerXP");

        //Displays dialog box when game is loaded
        if(txtLoadSave != null && loadSaveDialog != null)
        {
            txtLoadSave.text = "Your Game Has Been Loaded!";
            loadSaveDialog.SetActive(true);
        }
        
        changeSettingsBtnStatus(false);
    }

    public void closeDialog()
    {
        if (loadSaveDialog != null){
            loadSaveDialog.SetActive(false);

            changeSettingsBtnStatus(true);
        }
    }

    public static void changeSettingsBtnStatus(bool isEnabled)
    {
        foreach (var child in GameObject.FindGameObjectsWithTag("settings")){
            if (child.GetComponent<Button>() != null)
                child.GetComponent<Button>().interactable = isEnabled;

            else if (child.GetComponent<Dropdown>() != null)
                child.GetComponent<Dropdown>().interactable = isEnabled;

            else if (child.GetComponent<Slider>()) 
                child.GetComponent<Slider>().interactable = isEnabled;
        }
    }

}
