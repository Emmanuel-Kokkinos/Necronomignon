﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BattleManager battleManager;
//    public BeastManager beastManager;

   public static bool mouse_over;
    
    void Start() 
    {
        GetName();
    }

    void Update()
    {
//        OnPointerEnter(null);
//        print(mouse_over);
        if (mouse_over)
        {
            print("a");
            if (Input.GetMouseButtonDown(0))
            {
                print("b");
                print(battleManager.turn);
                //print("THIS IS WHATS BUGGING: " + battleManager.turn);
                if (battleManager.roundOrderTypes[battleManager.turn] == "Player")
                {
                    print("c");
                    //Attack this beast
                    battleManager.Attack(GetName());
                }
            }
        }
    }
 

    //When the cursor is over this image, make mouse_over true
    public void PointerEnter(PointerEventData eventData)
    {
        print("d");
        mouse_over = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("d");
        mouse_over = true;
    }

    //When cursor leaves this image, make mouse_over false
    public void OnPointerExit(PointerEventData eventData)
    {
        print("e");
        mouse_over = false;
    }

    //Get the name of the beast that this slot is holding
    Beast GetName()
    {
        if (gameObject.name == "Slot1") return battleManager.enemySlot1;
        else if (gameObject.name == "Slot2") return battleManager.enemySlot2;
        else if (gameObject.name == "Slot3") return battleManager.enemySlot3;
        else if (gameObject.name == "Slot4") return battleManager.enemySlot4;
        else if (gameObject.name == "Slot5") return battleManager.enemySlot5;
        else if (gameObject.name == "Slot6") return battleManager.enemySlot6;
        else return null;

    }
}
