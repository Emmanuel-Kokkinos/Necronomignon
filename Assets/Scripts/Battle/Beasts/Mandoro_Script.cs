using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Mandoro_Script : Parent_Script, Parent_Beast
{
    [SerializeField] AudioClip frontAttackSound, backAttackSound, damageSound, deathSound;

    public void back_special()
    {
        int ran = Random.Range(1, 5);

        for (; ran > 0; ran--)
        {
            battleManager.targets.Add(battleManager.targets[0]);
        }
        

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


    public void Play_SoundFX(string sound)
    {
        
        switch (sound)
        {
            case "front": audioSrc.PlayOneShot(frontAttackSound); break;
            case "back": audioSrc.PlayOneShot(backAttackSound); break;
            case "damage": audioSrc.PlayOneShot(damageSound); break;
            case "death": audioSrc.PlayOneShot(deathSound); break;
        }
        
    }
}
