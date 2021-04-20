using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DragonBones;

/**
 * Handles the movement of beasts when creating a squad
 */
public class SlotSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CreateManager createManager;
    public CreatePoolLoader createPoolLoader;
    public BeastManager beastManager;

    private bool mouse_over = false;

    public int slotID; //Set in inspector

    Beast thisBeast;
    int thisBeastIndex;

    // Update is called once per frame
    void Update()
    {
        if (mouse_over)
        {
            // When mouse is clicked and cursor is over this image, set the beast to this slot
            if (Input.GetMouseButtonDown(0) && !createManager.saveMode)
            {
                if (createManager.canBePlaced && createManager.placing ) SetImage();
                else if (createManager.moving && !createManager.placing) MoveImage();
                else EditPlace();
            }
        }
    }

    // When the cursor is over this image, make mouse_over true
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
    }

    // When cursor leaves this image, make mouse_over false
    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
    }

    // Set the selected beasts animation to the chosen slot
    void SetImage()
    {
        // Make sure no beast is already in that slot
        if (createManager.slots[slotID - 1] == null || createManager.slots[slotID - 1].speed == 0)
        {
            for (int x = 0; x < createPoolLoader.summonedNames.Count; x++)
            {
                if (createPoolLoader.summonedNames[x].Equals(createManager.selected.name))
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Static_Images/EmptyRectangle");

                    GameObject beastPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Beasts/" + createManager.selected.name));
                    if(createManager.selected.size == 0)
                    {
                        beastPrefab.transform.SetParent(GameObject.Find("Slot" + slotID).transform);
                    }
                    else if(createManager.selected.size == 1)
                    {
                        beastPrefab.transform.SetParent(GameObject.Find("BigSlot" + (slotID - 8)).transform);
                    }
                    beastPrefab.transform.localPosition = new Vector3(-10, -35);
                    beastPrefab.transform.localRotation = Quaternion.identity;
                    beastPrefab.transform.localScale = new Vector3(18f, 18f);
                    beastPrefab.transform.SetParent(GameObject.Find("SlotBeasts").transform);

                    beastPrefab.GetComponent<UnityArmatureComponent>().animation.Play("Idle", 0);
                }
            }

            ChangePoolImage();
        }  
    }

    // Remove the beast's animation from this slot
    public void RemoveImage()
    {
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Static_Images/Ellipse 1");

        foreach(UnityEngine.Transform child in GameObject.Find("SlotBeasts").transform)
        {
            if(child.name.Contains(createManager.selected.name))
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    // Remove the beast from the pool when it is loaded in the squad
    void ChangePoolImage()
    {
        int index = createManager.selectedIndex;

        createPoolLoader.slots[index].enabled = false;
        GameObject.Find("PoolBeasts").transform.GetChild(index).gameObject.SetActive(false);
        SetSlot();
    }

    // Set the CreateManager's variables to reflect the selected beast
    void SetSlot()
    {
        char chr = (gameObject.name).ToCharArray()[gameObject.name.Length - 1];
        int num = int.Parse(chr.ToString());

        if (createManager.selected.size == 0)
        {
            createManager.slots[num - 1] = createManager.selected;
        }
        else if(createManager.selected.size == 1)
        {
            createManager.slots[num + createManager.normalSlots.Count - 1] = createManager.selected;
        }

        thisBeast = createManager.selected;
        thisBeastIndex = createManager.selectedIndex;

        createManager.totalCost += thisBeast.cost;
        createManager.UpdateTotalBeastCost();
        createManager.placing = false;
        createManager.CheckPlaceable();
        createManager.selectedIndex = -1;
        createManager.TurnOffSlots(); 
    }

    // Select this beast and give options to move to another slot or remove from the grid
    void EditPlace()
    {
        print(createPoolLoader.summoned[slotID + (createPoolLoader.counter * 9)]);
        createManager.selected = thisBeast;
        createManager.selectedIndex = thisBeastIndex;
        createManager.selectedSlotID = slotID;
        createManager.LightUpSlots();
        createManager.ShowMoveDescription();
        createManager.removeButton.SetActive(true);
        createManager.moving = true;
    }

    // Move this beast to another slot
    void MoveImage()
    {
        // Make sure the spot to move to is empty before allowing to move
        if (slotID != createManager.selectedSlotID && (createManager.slots[slotID - 1] == null || createManager.slots[slotID - 1].speed == 0))
        {
            for (int x = 0; x < createPoolLoader.summonedNames.Count; x++)
            {
                if (createPoolLoader.summonedNames[x].Equals(createManager.selected.name))
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Static_Images/EmptyRectangle");

                    GameObject beastPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Beasts/" + createManager.selected.name));
                    if (createManager.selected.size == 0)
                    {
                        beastPrefab.transform.SetParent(GameObject.Find("Slot" + slotID).transform);
                    }
                    else if (createManager.selected.size == 1)
                    {
                        beastPrefab.transform.SetParent(GameObject.Find("BigSlot" + (slotID - 8)).transform);
                    }
                    beastPrefab.transform.localPosition = new Vector3(-10, -35);
                    beastPrefab.transform.localRotation = Quaternion.identity;
                    beastPrefab.transform.localScale = new Vector3(18f, 18f);
                    beastPrefab.transform.SetParent(GameObject.Find("SlotBeasts").transform);

                    beastPrefab.GetComponent<UnityArmatureComponent>().animation.Play("Idle", 0);
                }
            }

            SetSlot();
            createManager.RemoveSlotImage();
        }
    }
}
