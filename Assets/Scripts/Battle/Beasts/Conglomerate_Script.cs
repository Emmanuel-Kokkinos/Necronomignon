using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conglomerate_Script : Parent_Script, Parent_Beast
{
    [SerializeField] GameObject backPrefab;
    [SerializeField] AudioClip frontAttackSound, backAttackSound, startSound, deathSound;
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
        //Audio Effect example
        audioSrc.PlayOneShot(backAttackSound);

        ProjectileAnimation();
        battleManager.PlayDamagedAnimation(battleManager.targets[0]);

        if (battleManager.roundOrderTypes[battleManager.turn] == "Player")
        {
            attack.InitiateAttack(battleManager.currentTurn, battleManager.targets, battleManager.inFront(), Player.summoner);
        }
        else
        {
            attack.InitiateAttack(battleManager.currentTurn, battleManager.targets, battleManager.inFront(), battleManager.enemySummoner);
        }
        endOfAttack();
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
        endOfAttack();
    }

    void ProjectileAnimation()
    {
        GameObject target = battleManager.getSlot(battleManager.targets[0]);

        GameObject movePrefab = Instantiate(backPrefab);
        movePrefab.transform.SetParent(target.transform);
        movePrefab.transform.localPosition = new Vector3(0, 0);
        movePrefab.transform.localRotation = Quaternion.identity;
        movePrefab.transform.localScale = new Vector3(30, 30);
    }

    public void Play_SoundFX()
    {
        throw new System.NotImplementedException();
    }
}
