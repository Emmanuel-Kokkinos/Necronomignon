using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DragonBones;

public class TutorialClicking : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouse_over = false;
    public TutorialManager tutorialManager;

    // Update is called once per frame
    void Update()
    {
        if (mouse_over)
        {
            // When mouse is clicked and cursor is over this image, set the beast to this slot
            if (Input.GetMouseButtonDown(0))
            {
                tutorialManager.wyvern.gameObject.SetActive(true);
                tutorialManager.wyvern.GetComponent<UnityArmatureComponent>().animation.Play("Idle", 0);
                gameObject.SetActive(false);
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
}
