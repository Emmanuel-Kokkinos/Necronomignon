using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene : MonoBehaviour
{

    public string sceneInterface;
    [SerializeField] DialogueManager dialogueManager;
    // Start is called before the first frame update
    void Start()
    {
        sceneInterface = CampaignManager.sceneInterface;

        Debug.Log("Setting Battle Interface");

        Debug.Log(sceneInterface);
        SetBattleInterface();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBattleInterface()
    {
        GameObject canv = GameObject.Find("Canvas");
        if (sceneInterface == "Tournament")
        {
            canv.GetComponent<Image>().sprite = Resources.Load<Sprite>("Background_Pics/Tournament_Combat");
            //Set the images and components based on battle
            switch (LevelChecker.lastClick)
            {
                case "John": break;
                case "DemonChick": break;
                case "Gabriel": break;
            }
        }
        else if(sceneInterface == "Campaing")
        {
            canv.GetComponent<Image>().sprite = Resources.Load<Sprite>("Background_Pics/battle-background-sunny");
        }
    }
}
