using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dryad_Script : MonoBehaviour, Parent_Beast
{
    BattleManager battleManager;
    Attack attack;

    [SerializeField] AudioClip frontAttackSound, backAttackSound, startSound, deathSound;
    AudioSource audioSrc;

    void Start()
    {
        GameObject g = GameObject.Find("GameManager");
        GameObject au = GameObject.Find("Music");

        if (g != null)
        {
            battleManager = g.GetComponent<BattleManager>();
            attack = g.GetComponent<Attack>();
        }

        if (au != null)
            audioSrc = au.GetComponent<AudioSource>();
    }

    public void back_special()
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

    public void Play_SoundFX(string sound)
    {
        throw new System.NotImplementedException();
    }
}
