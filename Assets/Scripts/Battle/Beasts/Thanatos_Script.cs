using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Thanatos_Script : Parent_Script, Parent_Beast
{
    [SerializeField] AudioClip frontAttackSound, backAttackSound, damageSound, deathSound;
    AudioSource audioSrc;

    void Start()
    {
        base.start();

        GameObject au = GameObject.Find("Music");

        if (au != null)
            audioSrc = au.GetComponent<AudioSource>();
    }

    public void back_special()
    {
        battleManager.PlayDamagedAnimation(battleManager.targets[0]);
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

        switch (sound)
        {
            case "front": audioSrc.PlayOneShot(frontAttackSound); break;
            case "back": audioSrc.PlayOneShot(backAttackSound); break;
            case "damage": audioSrc.PlayOneShot(damageSound); break;
            case "death": audioSrc.PlayOneShot(deathSound); break;
        }
        
        
    }
}
