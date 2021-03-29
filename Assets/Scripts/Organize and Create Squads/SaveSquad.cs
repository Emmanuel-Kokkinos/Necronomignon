using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Handles the action of saving a squad including which buttons to set active
 */
public class SaveSquad : MonoBehaviour
{
    public CreateManager createManager;
    public SquadData squadData;

    public GameObject cancelButton;
    public GameObject removeButton;
    public GameObject saveButton;
    public GameObject squad1Button;
    public GameObject squad2Button;

    // Start is called before the first frame update
    private void Start()
    {
        squad1Button.SetActive(false);
        squad2Button.SetActive(false);
    }

    // Handles the buttons being activated or deactivated when saving a squad
    public void SaveButton()
    {
        if (createManager.totalCost >= 0)
        {
            squad1Button.SetActive(true);
            squad2Button.SetActive(true);
            saveButton.SetActive(false);

            if (cancelButton.activeInHierarchy) cancelButton.SetActive(false);
            if (removeButton.activeInHierarchy) removeButton.SetActive(false);

            createManager.saveMode = true;
        }
        else
        {
            Debug.Log("Problem saving the squad");
        }
    }

    // Saves the beasts that are loaded into the squad to the chosen squad number
    public void SaveThisSquad(int squadNumber)
    {
        if (squadNumber == 1)
        {
            squadData.ClearList(1);
            Beast be = new Beast();
            foreach(Beast b in createManager.slots)
            {
                squadData.AddToList(1, b);
            }
            squadData.ChangeSquadSavedStatus(1);
        }
        else if (squadNumber == 2)
        {
            squadData.ClearList(2);
            foreach (Beast b in createManager.slots)
            {
                squadData.AddToList(2, b);
            }
            squadData.ChangeSquadSavedStatus(2);
        }
        else
        {
            Debug.Log("Error saving squad.");
        }

        SceneManager.LoadScene("OrganizeMain");
    }
}
