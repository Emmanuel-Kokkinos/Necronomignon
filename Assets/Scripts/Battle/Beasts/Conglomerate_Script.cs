using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conglomerate_Script : Parent_Script, Parent_Beast
{
    [SerializeField] GameObject backPrefab;
    [SerializeField] AudioClip frontAttackSound, backAttackSound, damageSound, deathSound;

    void Start()
    {
        frontAttackSound = backAttackSound;

        base.start();
    }

    public void PlayFrontMove()
    {
        GameObject target = battleManager.getSlot(battleManager.targets[0]);

        GameObject movePrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Move"));
        movePrefab.transform.SetParent(target.transform);
        movePrefab.transform.localPosition = new Vector3(0, 100);
        movePrefab.transform.localRotation = Quaternion.identity;
        movePrefab.transform.localScale = new Vector3(2f, 2f);

        movePrefab.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/Conglomerate/Conglomerate_Move_Controller") as RuntimeAnimatorController;
        movePrefab.GetComponent<Animator>().SetTrigger("Front");
    }

    public void back_special()
    {
        ProjectileAnimation();

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
        if (battleManager.roundOrderTypes[battleManager.turn] == "Player")
        {
            attack.InitiateAttack(battleManager.currentTurn, battleManager.targets, battleManager.inFront(), Player.summoner);
        }
        else
        {
            attack.InitiateAttack(battleManager.currentTurn, battleManager.targets, battleManager.inFront(), battleManager.enemySummoner);
        }
    }

    void ProjectileAnimation()
    {
        GameObject target = battleManager.getSlot(battleManager.targets[0]);

        GameObject movePrefab = Instantiate(backPrefab);
        movePrefab.transform.SetParent(target.transform);
        movePrefab.transform.localPosition = new Vector3(0, 0);
        movePrefab.transform.localRotation = Quaternion.identity;
        movePrefab.transform.localScale = new Vector3(10, 10);
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
