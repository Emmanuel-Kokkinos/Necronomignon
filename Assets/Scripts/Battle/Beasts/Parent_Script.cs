using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class Parent_Script : MonoBehaviour
{
    protected BattleManager battleManager;
    protected Attack attack;

    void Start()
    {

        GameObject g = GameObject.Find("GameManager");

        if (g != null)
        {
            battleManager = g.GetComponent<BattleManager>();
            attack = g.GetComponent<Attack>();
        }
        this.GetComponent<UnityArmatureComponent>().unityData = new UnityDragonBonesData();


    }

    protected void start()
    {
        Start();
    }
}
