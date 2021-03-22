using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent_Script : MonoBehaviour
{
    protected BattleManager battleManager;
    protected Attack attack;

    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.Find("GameManager");

        if (g != null)
        {
            battleManager = g.GetComponent<BattleManager>();
            attack = g.GetComponent<Attack>();
        }
    }
    protected void start()
    {
        Start();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    protected void endOfAttack()
    {
        //battleManager.PlayAttackAnimation(battleManager.inFront());
        battleManager.TakeTurn();
    }
}
