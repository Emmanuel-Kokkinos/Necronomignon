using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class AudioManager : MonoBehaviour
{
    //THIS ARRAY HOLDS ALL THE SOUNDS IN THE GAME
    public Sound[] sounds;

    // Holds the Mp3 Clip For the Battle Theme (Liberation Of Honor) -> Gets instantiated in Unity inspector
    public AudioClip battleTheme;
    // Holds the Mp3 Clip For the Main menu Theme (Neutral Theme) -> Gets instantiated in Unity inspector
    public AudioClip mainTheme;

    // This string will hold the name of the previous scene so the game can decide which theme song to play appropriately
    public String tempName = "";

    void Awake()
    {
        // Search for all games objects in scene with the tag "music", you can edit the GameObject tag in the inspector
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            // This is very IMPORTANT, it keeps the GameObject value to 1, or else you'll just have multiplying GameObjects every time you re-load the scene
            Destroy(this.gameObject);
        }

        // Allows the keep the GameObject from scene to scene
        DontDestroyOnLoad(this.gameObject);


        // Sets the Sound Object Values
        foreach (Sound s in sounds)
        {
           s.source = gameObject.AddComponent<AudioSource>();
           s.source.clip = s.clip;

           s.source.volume = s.volume;
           s.source.pitch = s.pitch;
        }
        
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject go = GameObject.Find("VolumeSlider");
        if (go != null)
        {
            Slider slide = go.GetComponent<Slider>();
            go = GameObject.Find("Music");
            AudioSource[] auso = go.GetComponents<AudioSource>();
            //slide.value = auso[auso.Length - 1].volume;
            auso[auso.Length - 1].volume = slide.value;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject go = GameObject.Find("VolumeSlider");
        if (go != null)
        {
            Slider slide = go.GetComponent<Slider>();
            go = GameObject.Find("Music");
            AudioSource[] auso = go.GetComponents<AudioSource>();
            foreach (AudioSource sour in auso)
            {

                sour.volume = slide.value;
            }
        }
    }
    
    public void Play(string name)
    {
        // Find all object with tag "music and sets them to a GameObject Array
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        // Parse through the Sound array to find the correspondant clip where the name matches the search. 
        Sound s = Array.Find(sounds, sound => sound.name == name);

        //Sets the value to the Audio Source Component from the GameObject
        AudioSource audio = objs[0].GetComponent<AudioSource>();

        //sets the played clip to the main theme
        audio.clip = mainTheme;


        // In case the scene is a battle scene, this will load the battle music
        if (name.Equals("Manoli") || name.Equals("RandomFight") || name.Equals("RandomerFight"))
        {
            audio.clip = battleTheme;
            audio.Play();
            tempName = name;      
        }
        
        // If exiting from a battle , this sets the music back to the Main theme 
        else if ((name.Equals("Map") && tempName.Equals("Manoli")) || (name.Equals("Map") && tempName.Equals("RandomFight")) || (name.Equals("Map") && tempName.Equals("RandomerFight")))
        {
            tempName = name;
            audio.Play();
        }

        else
        {
            if (s != null)
            {
                tempName = name;

                s.source.Play();               
            }
        }
    }
    
}
