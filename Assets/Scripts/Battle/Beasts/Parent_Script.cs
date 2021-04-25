using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class Parent_Script : MonoBehaviour
{
    protected BattleManager battleManager;
    protected Attack attack;
    protected AudioSource audioSrc;

    void Start()
    {

        GameObject g = GameObject.Find("GameManager");
        GameObject au = GameObject.Find("Music");

        if (g != null)
        {
            battleManager = g.GetComponent<BattleManager>();
            attack = g.GetComponent<Attack>();
        }
        audioSrc = GetComponent<AudioSource>();

        if(au != null && audioSrc != null)
        {
            audioSrc.volume = au.GetComponent<AudioSource>().volume;
        }
        this.GetComponent<UnityArmatureComponent>().unityData = new UnityDragonBonesData();


    }

    protected void start()
    {
        Start();
    }
}
