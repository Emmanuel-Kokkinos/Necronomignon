using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadCampaing : MonoBehaviour
{

    public GameObject campaignPrefab;
    public GameObject blurBackground;

    // Start is called before the first frame update
    void Start()
    {
        if (blurBackground != null) blurBackground.SetActive(false);
    }

    public void loadCampaign()
    {
        if (GameObject.Find("CampaignPick") != null) return;

        blurBackground.SetActive(true);

        GameObject parent = GameObject.Find("CampaignPickHold");
        GameObject child = Instantiate(campaignPrefab, new Vector3(0, 0, 0), Quaternion.identity);


        child.name = "CampaignPick";
        child.transform.SetParent(parent.transform, false);

        Button closePick = (Button)GameObject.Find("closePickBtn").GetComponent<Button>();
        closePick.onClick.AddListener(ClosePick);
        Button battle = (Button)GameObject.Find("battleBtn").GetComponent<Button>();
        battle.onClick.AddListener( delegate { SceneManager.LoadScene("Map"); });
        Button tournament = (Button)GameObject.Find("tournamentBtn").GetComponent<Button>();
        tournament.onClick.AddListener(delegate { SceneManager.LoadScene("DialogScene"); });
    }

    // Update is called once per frame
    void Update(){    }

    public void ClosePick()
    {
        GameObject campaign = GameObject.Find("CampaignPick");

        //if settings screen is not instantiated stop function
        if (campaign == null) return;

        blurBackground.SetActive(false);

        Destroy(campaign);
    }
}
