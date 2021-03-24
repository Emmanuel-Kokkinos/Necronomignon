using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class Zeograth_Script : MonoBehaviour, Parent_Beast
{
    BattleManager battleManager;
    Attack attack;

    void Start()
    {
        GameObject g = GameObject.Find("GameManager");

        if (g != null)
        {
            battleManager = g.GetComponent<BattleManager>();
            attack = g.GetComponent<Attack>();
        }
    }

    public void back_special()
    {
        GameObject slot = battleManager.getSlot(battleManager.targets[0]);
        print(slot.transform.GetChild(0).name + " name of child");
        slot.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Prefabs/Moves/Move_Controller") as RuntimeAnimatorController;
        //Need to replay animation or remove controller each time

        battleManager.PlayDamagedAnimation(battleManager.targets[0]);

        if (battleManager.roundOrderTypes[battleManager.turn] == "Player")
        {
            attack.InitiateAttack(battleManager.currentTurn, battleManager.targets, battleManager.inFront(), Player.summoner);
        }
        else
        {
            attack.InitiateAttack(battleManager.currentTurn, battleManager.targets, battleManager.inFront(), battleManager.enemySummoner);
        }
    }

    public void front_special() 
    {
        battleManager.PlayDamagedAnimation(battleManager.targets[0]);

        if (battleManager.roundOrderTypes[battleManager.turn] == "Player")
        {
            attack.InitiateAttack(battleManager.currentTurn, battleManager.targets, battleManager.inFront(), Player.summoner);
        }
        else
        {
            attack.InitiateAttack(battleManager.currentTurn, battleManager.targets, battleManager.inFront(), battleManager.enemySummoner);
        }
    }

    public void Play_SoundFX()
    {
        throw new System.NotImplementedException();
    }
}
