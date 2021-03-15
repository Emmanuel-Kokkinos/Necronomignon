using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dryad_Script : MonoBehaviour, Parent_Beast
{
    BattleManager battleManager;

    void Start()
    {
        GameObject g = GameObject.Find("GameManager");

        if (g != null)
        {
            battleManager = g.GetComponent<BattleManager>();
        }
    }

    public void back_special()
    {

    }

    public void front_special()
    {
        
    }

    public void Play_SoundFX()
    {
        throw new System.NotImplementedException();
    }
}
