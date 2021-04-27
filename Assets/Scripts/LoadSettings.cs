using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LoadSettings : MonoBehaviour
{
    public GameObject settingsPrefab;
    public GameObject blurBackground;
    public Dropdown resolutionDropdown;
    public Dropdown fullscreenDropdown;
    public static Button redRoach;
    public Resolution[] resolutions;
    //stores all possible screen resolutions with a 16:9 aspect ratio
    public static List<Resolution> resolutionsList;
    public bool fullscreen = true;
    public bool borderless = true;

    float value = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Loads assets asynchroniously
        setRedRoachAsset();

        if (blurBackground != null) blurBackground.SetActive(false);

        if (resolutionsList == null){
            resolutionsList = new List<Resolution>();
            getScreenResolutions();
        }
    }

    // Update is called once per frame
    void Update(){}

    //Load settings assets into screen
    public void loadSettings() {
        //if settings screen is instantiated stop function
        GameObject go = GameObject.Find("Music");
        
        if (go != null)
        {
            AudioSource[] auso = go.GetComponents<AudioSource>();
            value = auso[0].volume;
        }
        if (GameObject.Find("SettingsScreen") != null) return;

        blurBackground.SetActive(true);
        
        GameObject parent = GameObject.Find("SettingsHolder"); 
        GameObject child = Instantiate(settingsPrefab, new Vector3(0, 0, 0), Quaternion.identity);
      

        child.name = "SettingsScreen";
        child.transform.SetParent(parent.transform, false);

        resolutionDropdown = GameObject.Find("resolutionDropdown").GetComponent<Dropdown>();
        fullscreenDropdown = GameObject.Find("fullscreenDropdown").GetComponent<Dropdown>();

        setOnClickFunctions();
        setVolumeSlider();
        setFullscreenDropdown();
        getFullscreenOption();
        setResolutionsDropdown();
        setRedRoachAsset();
    }
    
    //Gives buttons onclick listeners
    public void setOnClickFunctions()
    {
        Button CloseSettings = (Button) GameObject.Find("closeSettingsBtn").GetComponent<Button>();
        Button saveBtn = (Button) GameObject.Find("SaveBtn").GetComponent<Button>();
        Button loadBtn = (Button) GameObject.Find("LoadBtn").GetComponent<Button>();
        redRoach = (Button) GameObject.Find("RedRoach").GetComponent<Button>();
        Button RedRoach = (Button) GameObject.Find("Red Roach").GetComponent<Button>();

        CloseSettings.onClick.AddListener(closeSettings);
        saveBtn.onClick.AddListener(Saving.saveAll);
        loadBtn.onClick.AddListener(Saving.loadAll);
        resolutionDropdown.onValueChanged.AddListener(delegate { onResolutionChanged(); });
        fullscreenDropdown.onValueChanged.AddListener(delegate { onFullscreenChanged(); });
        redRoach.onClick.AddListener(Player.activeRedRoach);
        RedRoach.onClick.AddListener(Player.activeRedRoach);
    }

    //Asset to initiate redroach mode
    public static void setRedRoachAsset() {

        //Loads assets asynchroniously so they can be used when red roach is active
        Sprite clicked = Resources.Load<Sprite>("Static_Images/clicked_button");
        Sprite unclicked = Resources.Load<Sprite>("Static_Images/unclicked_button");

        Color colorTemp = (Player.RedRoach ? Color.red : new Color(0.9137255f, 0.7098039f, 0.1254902f, 1f));
        //Solves missing reference exception
        if(redRoach != null)
        {
            redRoach.image.sprite = Player.RedRoach ? clicked : unclicked;
            redRoach.GetComponentInChildren<Text>().color = colorTemp;
        }
        
    }

    public void setVolumeSlider() {
        GameObject go = GameObject.Find("VolumeSlider");
        
        if (go != null)
        {
            Slider slide = go.GetComponent<Slider>();
            slide.value = value;
            go = GameObject.Find("Music");
            AudioSource[] auso = go.GetComponents<AudioSource>();
            auso[auso.Length - 1].volume = slide.value;
        }
    }

    /*
        Gets all supported screen resolutions with a 16:9 aspect ratio and stores them
    */
    public void getScreenResolutions() {
        //16:9 ratio
        double defaultRatio = Math.Truncate(((double)9 / 16)*1000);
        resolutions = Screen.resolutions;
        
        foreach (Resolution res in 
            resolutions){
            double currRatio = Math.Truncate(((double)res.height / res.width)*1000);

            if (currRatio == defaultRatio)
                resolutionsList.Add(res);
        }
    }

    public void setResolutionsDropdown() {
        int tempVal = -1;
        if (fullscreen) resolutionDropdown.interactable = false;

        resolutionDropdown.options.Clear();
        
        for(int i = 0; i < resolutionsList.Count; i++){
            String temp = resolutionsList[i].width + " x " + resolutionsList[i].height;

            resolutionDropdown.options.Add(new Dropdown.OptionData() { text = temp });

            Debug.Log("curr " + Screen.currentResolution.ToString()) ;
            Debug.Log(resolutionsList[i].ToString()) ;

            if (resolutionsList[i].Equals(Screen.currentResolution))
                tempVal = i;
        }
        resolutionDropdown.value = tempVal;

    }

    public void setFullscreenDropdown() {

        fullscreenDropdown.options.Clear();

        fullscreenDropdown.options.Add(new Dropdown.OptionData() { text = "Fullscreen"});
        fullscreenDropdown.options.Add(new Dropdown.OptionData() { text = "Windowed"});
        fullscreenDropdown.options.Add(new Dropdown.OptionData() { text = "Borderless"});

        fullscreenDropdown.value = 0;
            
    }
    
    public void getFullscreenOption() {
        switch (fullscreenDropdown.value) {
                //fullscreen
            case 0: fullscreen = true;
                break;
                //windowed
            case 1: fullscreen = false; borderless = false;
                break;
                //windowed borderless
            case 2: fullscreen = false; borderless = true;
                break;
        }
    }

    public void closeSettings()
    {
        GameObject settings = GameObject.Find("SettingsScreen");

        //if settings screen is not instantiated stop function
        if (settings == null) return;

        blurBackground.SetActive(false);

        Destroy(settings);
    }


    public void onFullscreenChanged() {
        getFullscreenOption();

        //toggles fullscreen mode not sure if it works tho
        Screen.fullScreen = (fullscreen) ? Screen.fullScreen : !Screen.fullScreen;

        resolutionDropdown.interactable = fullscreen? false : true;    
    }

    public void onResolutionChanged() {
        Resolution temp = resolutionsList[resolutionDropdown.value];

        Screen.SetResolution(temp.width, temp.height, borderless ? FullScreenMode.MaximizedWindow : FullScreenMode.Windowed);
    }
}
