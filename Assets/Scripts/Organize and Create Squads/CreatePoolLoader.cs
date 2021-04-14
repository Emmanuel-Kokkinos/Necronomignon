using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Handles the beasts in the pool that can be added to a squad
 */
public class CreatePoolLoader : MonoBehaviour
{
    public BeastManager beastManager;
    public CreateManager createManager;

    public List<Image> slots; // Slots in the pool
    public List<string> summonedImages = new List<string>(); // Static image of each beast in the pool
    public List<Beast> summoned = new List<Beast>(); // Beasts that are summoned (not tier 0)
    public List<string> summonedNames = new List<string>(); // Names of beasts in pool

    public GameObject back;
    public GameObject forward;
    public Dropdown dropdown;

    public int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!beastManager.isLoaded())
        {
            beastManager.Awake();
        }

        BeastList bl = BeastManager.beastsList;

        for (int x = 0; x < bl.Beasts.Count; x++)
        {
            bl.Beasts[x].setAttacks();
            if(bl.Beasts[x].tier > 0)
            {
                summonedImages.Add(bl.Beasts[x].static_img);
                summoned.Add(bl.Beasts[x]);
                summonedNames.Add(bl.Beasts[x].name);
            }
        }

        SetImages();
    }

    // Fill up the image slots with your summoned beasts
    void SetImages()
    {
        // Destroy old prefabs
        foreach (Transform child in GameObject.Find("PoolBeasts").transform)
        {
            Destroy(child.gameObject);
        }

        // Populate pool with new prefabs
        for (int x =  (counter * 9); x < slots.Count + (counter * 9); x++)
        {
            slots[x % 9].sprite = Resources.Load<Sprite>("Static_Images/EmptyRectangle");

            if (summoned.Count >= x + 1 && NotSummoned(x))
            {
                GameObject beastPrefab = (GameObject) Instantiate(LoadAssetBundles.getObj(summoned[x].name));
                beastPrefab.transform.SetParent(GameObject.Find("Pool" + (x % 9 + 1)).transform);
                beastPrefab.transform.localPosition = new Vector3(0, -75);
                beastPrefab.transform.localRotation = Quaternion.identity;
                beastPrefab.transform.localScale = new Vector3(5f, 5f);
                beastPrefab.transform.SetParent(GameObject.Find("PoolBeasts").transform);

                Animator animator = beastPrefab.GetComponent<Animator>();
                animator.enabled = false;
            }
        }

        // Shows or hides the front and back page buttons depending on if there are beasts on previous pages
        if (counter == 0)
        {
            back.SetActive(false);
        }
        else if (counter > 0)
        {
            back.SetActive(true);
        }
        if ((counter * 9) + 9 >= summoned.Count)
        {
            forward.SetActive(false);
        }
        else if ((counter * 9) + 9 < summoned.Count)
        {
            forward.SetActive(true);
        }
    }

    // Sets the page forward by 1 when the front button is pressed
    public void Forward()
    {
        changeImages("Forward");
    }
    
    // Sets the page back by 1 when the back button is pressed
    public void Back()
    {
        changeImages("Back");
    }

    // Returns true if the beast exists but isn't summoned yet
    bool NotSummoned(int y)
    {
        if (y >= summoned.Count)
        {
            return true;
        }

        Beast beast;


        beast = y >= 0 && y < summoned.Count ? summoned[y] : null;

        if (beast == null)
        {
            return false;
        }

        for (int x = 0; x < createManager.slots.Count; x++)
        {
            if (beast.Equals(createManager.slots[x]))
            {
                return false;
            }
        }

        return true;
    }

    // Changes the counter to show which page we are viewing beasts on
    public void changeImages(string str)
    {
        if(str == "Forward")
        {
            counter++;
        }
        else if(str == "Back")
        {
            counter--;
        }

        SetImages();
    }

    // When a beast is removed from the grid, place the image back into the pool
    public void PutImageBack()
    {
        int index = createManager.selectedIndex;

        slots[index].enabled = true;
        GameObject.Find("PoolBeasts").transform.GetChild(index).gameObject.SetActive(true);
    }
}
