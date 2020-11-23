using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SummonBookLoader : MonoBehaviour
{
    static public string beastName;

    public BeastManager beastManager;

    public List<GameObject> beastTexts;
    public List<Image> slots;

    public List<string> summonedNames = new List<string>();
    public List<string> summonedImages = new List<string>();
    public List<Beast> summoned = new List<Beast>();

    public List<GameObject> poolSlots;
    public GameObject back;
    public GameObject forward;

    public int counter = 0;

    void Start()
    {
        if (!beastManager.isLoaded())
        {
            beastManager.Awake();
        }
        BeastList bl = BeastManager.beastsList;
        for (int x = 0; x < bl.Beasts.Count; x++)
        {
            summonedNames.Add(bl.Beasts[x].name);
            summonedImages.Add(bl.Beasts[x].static_img);
            summoned.Add(bl.Beasts[x]);
        }

        SetImages();
    }

    //Fill up the image slots with your summoned beasts
    void SetImages()
    {
        for (int x = 0 + (counter * 6); x < slots.Count + (counter * 6); x++)
        {
            if (summoned.Count >= x + 1)
            {
                slots[x % 6].gameObject.SetActive(true);
                slots[x % 6].sprite = Resources.Load<Sprite>(summonedImages[x]);
                beastTexts[x % 6].GetComponent<Text>().text = summonedNames[x];

                //Make not summoned beasts transparent
                if (summoned[x].tier == 0)
                {
                    var tempColor = slots[x % 6].color;
                    tempColor.a = .5f;
                    slots[x % 6].color = tempColor;
                }
            }
            else
            {
                slots[x % 6].gameObject.SetActive(false);
                beastTexts[x % 6].GetComponent<Text>().text = "";
            }
        }

        if (counter == 0)
        {
            back.SetActive(false);
        }
        else if (counter > 0)
        {
            back.SetActive(true);
        }
        if ((counter * 6) + 6 >= summoned.Count)
        {
            forward.SetActive(false);
        }
        else if ((counter * 6) + 6 < summoned.Count)
        {
            forward.SetActive(true);
        }
    }

    public void Forward()
    {
        changeImages("Forward");
    }

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

        SetImages();
    }

    //Loads animation scene if beast is summoned and summon page if it isn't
    public void OnClick()
    {
        for(int x = 1; x <= poolSlots.Count; x++)
        {
            if (EventSystem.current.currentSelectedGameObject.name == "Slot" + x)
            {
                beastName = summoned[(counter * 6) + x - 1].name;
                if (summoned[(counter * 6) + x - 1].tier == 0)
                {
                    SceneManager.LoadScene(beastName + "Page1");
                }
                else
                {
                    SceneManager.LoadScene("BeastView");
                }
            }
        }
    }
}
