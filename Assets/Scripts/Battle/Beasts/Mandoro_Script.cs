using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Mandoro_Script : MonoBehaviour, Parent_Beast
{
    BattleManager battleManager;
    AudioClip frontAttackSound, backAttackSound, startSound, deathSound;
    AudioSource audioSrc;
    Attack attack;

    void Start()
    {
        GameObject g = GameObject.Find("GameManager");

        if (g != null)
        {
            battleManager = g.GetComponent<BattleManager>();
            attack = g.GetComponent<Attack>();
        }

        frontAttackSound = Resources.Load<AudioClip>("Robot1");
        audioSrc = GetComponent<AudioSource>();
    }

    public void back_special()
    {
        int ran = Random.Range(1, 5);
        print("The number of attacks is " + (ran + 1));
        foreach (Beast b in battleManager.targets)
        {
            print(b.name + " before");
        }
        for (; ran > 0; ran--)
        {
            battleManager.targets.Add(battleManager.targets[0]);
        }
        
        foreach(Beast b in battleManager.targets)
        {
            print(b.name + " after");
        }

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
        audioSrc.PlayOneShot(frontAttackSound);
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

    //To modify -> add parameter to select character sound.
    public void PlaySound()
    {
        //Switch statement to select the sound played. In this case we will assume only one sound 

        audioSrc.PlayOneShot(deathSound);
    }

    public void Play_SoundFX()
    {
        throw new System.NotImplementedException();
    }
}
