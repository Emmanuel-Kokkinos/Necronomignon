using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TournamentManager : MonoBehaviour
{
    static public GameObject firstMission;
    static public string sceneInterface;

    // Start is called before the first frame update
    void Start()
    {
        //Makes first button dissapear so it can be active once dialogue ends
        firstMission = GameObject.Find("btnTour1");

        if(firstMission != null)
            firstMission.SetActive(false);

        
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
