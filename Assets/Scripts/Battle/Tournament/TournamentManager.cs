using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentManager : MonoBehaviour
{
    static public GameObject firstMission;
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
}
