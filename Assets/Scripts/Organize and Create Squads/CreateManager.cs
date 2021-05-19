using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Handles all of the processes when creating a squad
 */
public class CreateManager : MonoBehaviour
{
    public CreatePoolLoader createPoolLoader;

    public List<GameObject> normalSlots; // Squad gameobjects for normal characters
    public List<GameObject> bigSlots; // Squad gameobjects for big characters
    public List<GameObject> totalSlots;
    public List<Beast> slots; // Beasts that are in the squad slots

    public GameObject cancelButton;
    public GameObject removeButton;
    public GameObject moveDescription;
    public Text description;
    [SerializeField] Text totalBeastCostText;
    public Beast selected;
    public int selectedIndex;
    public bool canBePlaced = true;
    public bool placing = false;
    public bool moving = false;
    public int totalCost = 0;
    public int selectedSlotID;
    public bool saveMode = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject go in normalSlots)
        {
            go.SetActive(false);
        }

        foreach(GameObject go in bigSlots)
        {
            go.SetActive(false);
        }

        cancelButton.SetActive(false);
        removeButton.SetActive(false);
    }

    // Sets all slot lights and cancel button active for when a beast is being selected
    public void LightUpSlots()
    {
        TurnOffSlots();
        moveDescription.SetActive(true);
        
        selected.setAttacks();
        
        if (selected.size == 0)
        {
            foreach (GameObject go in normalSlots)
            {
                go.SetActive(true);
            }

            if (slots[8].speed != 0)
            {
                normalSlots[0].SetActive(false);
                normalSlots[1].SetActive(false);
                normalSlots[4].SetActive(false);
                normalSlots[5].SetActive(false);
            }
            if (slots[9].speed != 0)
            {
                normalSlots[1].SetActive(false);
                normalSlots[2].SetActive(false);
                normalSlots[5].SetActive(false);
                normalSlots[6].SetActive(false);
            }
            if (slots[10].speed != 0)
            {
                normalSlots[2].SetActive(false);
                normalSlots[3].SetActive(false);
                normalSlots[6].SetActive(false);
                normalSlots[7].SetActive(false);
            }
            
        }
        else if (selected.size == 1)
        {
            foreach (GameObject go in bigSlots)
            {
                go.SetActive(true);
            }

            if (slots[0].speed != 0 || slots[1].speed != 0 || slots[4].speed != 0 || slots[5].speed != 0 || slots[9].speed != 0)
            {
                bigSlots[0].SetActive(false);
            }
            if (slots[1].speed != 0 || slots[2].speed != 0 || slots[5].speed != 0 || slots[6].speed != 0 || slots[8].speed != 0 || slots[10].speed != 0)
            {
                bigSlots[1].SetActive(false);
            }
            if (slots[2].speed != 0 || slots[3].speed != 0 || slots[6].speed != 0 || slots[7].speed != 0 || slots[9].speed != 0)
            {
                bigSlots[2].SetActive(false);
            }
        }

        cancelButton.SetActive(true);
    }

    public void ShowMoveDescription()
    {
        // Sets the description of selected beast
        for (int x = 0 + (createPoolLoader.counter * 9); x < createPoolLoader.summoned.Count; x++)
        {
            if (selected.name.Equals(createPoolLoader.summoned[x].name))
            {
                description.GetComponent<Text>().text = "\nIn the front row \n" + selected.Move_A.description +
                    "\n\nIn the back row \n" + selected.Move_B.description + "\n\nCost = " + selected.cost;
            }
        }
    }

    public void UpdateTotalBeastCost()
    {
        totalBeastCostText.text = "Team cost = " + totalCost + " / " + Values.TOTAL_BEAST_COST;
    }

    // Sets all slots that do not have a beast placed in them to inactive
    public void TurnOffSlots()
    {
        moveDescription.SetActive(false);

        for (int x = 0; x < slots.Count; x++)
        {
            if (x < normalSlots.Count)
            {
                if (slots[x] == null || slots[x].speed == 0)
                {
                    normalSlots[x].SetActive(false);
                }
            }
            else
            {
                if (slots[x] == null || slots[x].speed == 0)
                {
                    bigSlots[x - normalSlots.Count].SetActive(false);
                }
            }
        }

        cancelButton.SetActive(false);
    }

    // Cancels all selections when cancel button is pressed
    public void Cancel()
    {
        TurnOffSlots();
        selected = null;
        selectedIndex = -1;
        if (removeButton.activeInHierarchy) removeButton.SetActive(false);
        if (moving) moving = false;
        if (placing) placing = false;
    }

    // Removes selected beast when remove button is pressed
    public void Remove()
    {
        RemoveImage();
        createPoolLoader.PutImageBack();
        RemoveSlotImage();
    }

    // Removes image from a slot
    public void RemoveImage()
    {
        if (selected.size == 0)
        {
            GameObject.Find("Slot" + selectedSlotID).GetComponent<SlotSelect>().RemoveImage();
        }
        else if(selected.size == 1)
        {
            GameObject.Find("BigSlot" + (selectedSlotID - normalSlots.Count).ToString()).GetComponent<SlotSelect>().RemoveImage();
        }
        slots[selectedSlotID - 1] = null;
    }

    // Removes the image in a slot and remove it from selected variables
    public void RemoveSlotImage()
    {
        if (selectedSlotID <= 8)
        {
            GameObject.Find("Slot" + selectedSlotID).GetComponent<SlotSelect>().RemoveImage();
        }
        else if(selectedSlotID <= 11)
        {
            GameObject.Find("BigSlot" + (selectedSlotID - normalSlots.Count).ToString()).GetComponent<SlotSelect>().RemoveImage();
        }
        slots[selectedSlotID - 1] = new Beast();

        totalCost -= selected.cost;
        UpdateTotalBeastCost();
        selected = null;
        selectedIndex = -1;
        CheckPlaceable();
        moving = false;
        TurnOffSlots();
        removeButton.SetActive(false);
    }

    // Checks to see if any more beasts can be placed
    public void CheckPlaceable()
    {
        if(totalCost < Values.TOTAL_BEAST_COST)
        {
            canBePlaced = true;
        }
        else
        {
            canBePlaced = false;
        }
    }
}
