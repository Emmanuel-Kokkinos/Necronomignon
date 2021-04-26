using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignManager : MonoBehaviour
{
    static public GameObject firstMission;
    static public string sceneInterface;
    static public bool dialogueEnd = false; 

    // Start is called before the first frame update
    void Start()
    {
        //Makes first button dissapear so it can be active once dialogue ends
        firstMission = GameObject.Find("btnTour1");
        GameObject conversation = GameObject.Find("ConversationManager");
        GameObject speaker = GameObject.Find("chrSpeaker1");

        if (firstMission != null && dialogueEnd == false)
            firstMission.SetActive(false);

        //When Dialogue Ends once, don't repeat
        if(dialogueEnd == true)
        {
            if (conversation != null && speaker != null)
            {
                conversation.SetActive(false);
                speaker.SetActive(false);
            }
        }
            
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildTournament()
    {

    }

    public void SetBattleInterface(string mission)
    {
        sceneInterface = mission;
    }
}
