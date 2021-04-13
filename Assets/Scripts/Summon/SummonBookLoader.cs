using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

/*
 * This class handles most of the logic behind the SummonMain scene
 * it outputs the list of beasts in the Necronomignon
 */

public class SummonBookLoader : MonoBehaviour
{
    //Hold the values for the generating the beasts list
    static public string beastName;
    static public int counter = 0;
    static public int sortedBy = 0;
    
    //BeastManager Script object
    public BeastManager beastManager;

    //Holds the name and Image of the beast that will be listed on a specific slot
    public List<GameObject> beastTexts;
    public List<Image> slots;

    //Hold values for unlocked beasts 
    List<Beast> sorted = new List<Beast>();
    public List<string> summonedNames = new List<string>();
    public List<string> summonedImages = new List<string>();
    public List<Beast> summoned = new List<Beast>();

    public List<GameObject> poolSlots;
    public GameObject back;
    public GameObject forward;
    public Dropdown dropdown;

    /*
     * Will be called everytime the script is loaded, instantiate values for holding unlocked beasts 
     */
    void Start()
    {
        if (!beastManager.isLoaded())
        {
            beastManager.Awake();
        }
        BeastList bl = BeastManager.beastsList;

        for (int x = 0; x < bl.Beasts.Count; x++)
        {
            if (bl.Beasts[x].tier >= 0)
            {
                summoned.Add(bl.Beasts[x]);
                summonedImages.Add(bl.Beasts[x].static_img);
                summonedNames.Add(bl.Beasts[x].name);
                sorted.Add(bl.Beasts[x]);
            }
        }
        SortImagesDropdown(sortedBy);
        dropdown.value = sortedBy;

        SetBeasts();
    }

    //Fill up the image slots with your summoned beasts
    void SetBeasts()
    {
        //Destroy old prefabs
        foreach (Image slot in slots)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }

        
        //Sets prefabs in available spaces 
        for (int x = 0 + (counter * 6); x < slots.Count + (counter * 6); x++)
        {
            slots[x % 6].gameObject.SetActive(true);
            if (sorted.Count >= x + 1)
            {
                slots[x % 6].sprite = Resources.Load<Sprite>("Static_Images/EmptyRectangle");
                
                //Prefab setting
                GameObject beastPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Beasts/" + summonedNames[x]));
                beastPrefab.transform.SetParent(GameObject.Find("Slot" +((x % 6) + 1)).transform);
                beastPrefab.transform.localPosition = new Vector3(0, -50);
                beastPrefab.transform.localRotation = Quaternion.identity;
                beastPrefab.transform.localScale = new Vector3(30f, 30f);

                beastTexts[x % 6].GetComponent<Text>().text = summonedNames[x];

                //Make not summoned beasts transparent
                if (sorted[x].tier <= 0)
                {
                    var tempColor = slots[x % 6].color;
                    tempColor.a = .5f;
                    slots[x % 6].color = tempColor;
                }
                else
                {
                    var tempColor = slots[x % 6].color;
                    tempColor.a = 1f;
                    slots[x % 6].color = tempColor;
                }

            }
            else
            {
                slots[x % 6].gameObject.SetActive(false);
                beastTexts[x % 6].GetComponent<Text>().text = "";
            }
        }


        // Enables pagination for available beasts in summon 
        if (counter == 0)
        {
            back.SetActive(false);
        }
        else if (counter > 0)
        {
            back.SetActive(true);
        }
        if ((counter * 6) + 6 >= sorted.Count)
        {
            forward.SetActive(false);
        }
        else if ((counter * 6) + 6 < sorted.Count)
        {
            forward.SetActive(true);
        }

        /* OLD METHOD TO POPULATE SUMMON KEPT FOR REFERENCE
         * 
         *
        for (int x = 0 + (counter * 6); x < slots.Count + (counter * 6); x++)
        {
            if (sorted.Count >= x + 1)
            {
                slots[x % 6].gameObject.SetActive(true);
                slots[x % 6].sprite = Resources.Load<Sprite>("Static_Images/"+summonedImages[x]);
                beastTexts[x % 6].GetComponent<Text>().text = summonedNames[x];

                //Make not summoned beasts transparent
                if (sorted[x].tier <= 0)
                {
                    var tempColor = slots[x % 6].color;
                    tempColor.a = .5f;
                    slots[x % 6].color = tempColor;
                }
                else
                {
                    var tempColor = slots[x % 6].color;
                    tempColor.a = 1f;
                    slots[x % 6].color = tempColor;
                }
                
            }
            else
            {
                slots[x % 6].gameObject.SetActive(false);
                beastTexts[x % 6].GetComponent<Text>().text = "";
            }
        }
        *
        */


    }


    //Changes Image set & arrow from the Summon Book
    public void Forward()
    {
        changeImages("Forward");
    }

    //Changes Image set & arrow from the Summon Book
    public void Back()
    {
        changeImages("Back");
    }

    
    public void changeImages(string str)
    {
        if (str == "Forward")
        {
            counter++;
        }
        else if (str == "Back")
        {
            counter--;
        }

        SetBeasts();
    }

    //Loads animation scene if beast is summoned and summon page if it isn't
    public void OnClick()
    {
        for(int x = 1; x <= poolSlots.Count; x++)
        {
            if (EventSystem.current.currentSelectedGameObject.name == "Slot" + x)
            {
                beastName = summonedNames[(counter * 6) + x - 1];
                if (sorted[(counter * 6) + x - 1].tier >= 0 && hasAStory(beastName))
                {
                    SummonManager.name = beastName;
                    SceneManager.LoadScene("BeastStory");
                }
                else
                {
                    SceneManager.LoadScene("BeastView");
                }
            }
        }
    }

    public static bool hasAStory(string name)
    {
        for(int x = 0; x< StoryManager.storyList.Stories.Count;x++)
        {
            if(name == StoryManager.storyList.Stories[x].beast_name)
            {
                return true;
            }
        }
        return false;
    }
    public bool hasAStory(Beast b)
    {
        return hasAStory(b.name);
    }

    //Depending on the selected choice in the dropdown menu, will call the appropriate method
    public void SortImagesDropdown(int value)
    {
        if(value == 0)
        {
            sortedBy = 0;
            SortByTier();
        }
        else if (value == 1)
        {
            sortedBy = 1;
            SortByType();
        }
        else if (value == 2)
        {
            sortedBy = 2;
            SortByName();
        }
    }

    //Sorts the beasts by tier
    void SortByTier()
    {
        sorted.Clear();
        summonedImages.Clear();
        summonedNames.Clear();

        for (int x = 5; x >= -1; x--)
        {
            for (int y = 0; y < summoned.Count; y++)
            {
                if (summoned[y].tier == x)
                {
                    sorted.Add(summoned[y]);
                    summonedImages.Add(summoned[y].static_img);
                    summonedNames.Add(summoned[y].name);
                }
            }
        }
        
        SetBeasts();
    }

    //Sorts the beasts based on their type
    void SortByType()
    {
        sorted.Clear();
        summonedImages.Clear();
        summonedNames.Clear();

        for (int x = 0; x <= Enum.GetNames(typeof(Beast.types)).Length; x++)
        {
            for (int y = 0; y < summoned.Count; y++)
            {
                if ((int)summoned[y].type == x && summoned[y].tier >= 0) 
                {
                    sorted.Add(summoned[y]);
                    summonedImages.Add(summoned[y].static_img);
                    summonedNames.Add(summoned[y].name);
                }
            }
        }

        SetBeasts();
    }

    //Sorts the beasts in alphabetical order
    void SortByName()
    {
        sorted.Clear();

        List<string> names = new List<string>();

        foreach(Beast b in summoned)
        {
            names.Add(b.name);
        }
        names.Sort();

        for(int x = 0; x < names.Count; x++)
        {
            for(int y = 0; y < summoned.Count; y++)
            {
                if(names[x] == summoned[y].name && summoned[y].tier >= 0)
                {
                    sorted.Add(summoned[y]);
                }
            }
        }

        summonedImages.Sort();
        summonedNames.Sort();

        SetBeasts();
    }

}
