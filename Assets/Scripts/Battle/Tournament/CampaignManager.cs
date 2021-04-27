using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignManager : MonoBehaviour
{
    static public GameObject firstMission;
    static public string sceneInterface;
    static public bool dialogueEnd, semiDiagEnd, finDiagEnd, tourEnd = false;

    static public List<Sprite> enemyPictures = new List<Sprite>();
    /*Flag to set if win each of battles:  
     * 0 -> lose
     * 1 -> defeat john
     * 2 -> defeat jheera
     * 3 -> defeat gabriel
     */
    public int winTourBattle = 0;

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

        BuildTournament();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildTournament()
    {
        SetEnemyPictures();
    }

    public void SetBattleInterface(string mission)
    {
        sceneInterface = mission;
    }
    //Adds pictures to list of pictures
    public void SetEnemyPictures()
    {
        enemyPictures.Add(Resources.Load<Sprite>("Character_Pictures/Profile_Pictures/John_Front"));
        enemyPictures.Add(Resources.Load<Sprite>("Character_Pictures/Profile_Pictures/Jheera_Front"));
        enemyPictures.Add(Resources.Load<Sprite>("Character_Pictures/Profile_Pictures/Gabriel_Front"));
    }
}
