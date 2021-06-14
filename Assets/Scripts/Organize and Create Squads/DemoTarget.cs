using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Handles the mouse movement for attacks in the demo battle scene
 */
public class DemoTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DemoBattleManager demoBattleManager;

    private bool mouse_over = false;

    // Update is called once per frame
    void Update()
    {
        // Attacks every time the target is clicked
        if (mouse_over && demoBattleManager.attackingAnim == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                demoBattleManager.Attack();
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
