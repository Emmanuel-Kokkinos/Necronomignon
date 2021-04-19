using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conglomerate_Script : MonoBehaviour, Parent_Beast
{
    BattleManager battleManager;
    Attack attack;

    [SerializeField] GameObject backPrefab;
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

    void ProjectileAnimation()
    {
        GameObject target = battleManager.getSlot(battleManager.targets[0]);

        GameObject movePrefab = Instantiate(backPrefab);
        movePrefab.transform.SetParent(target.transform);
        movePrefab.transform.localPosition = new Vector3(0, 0);
        movePrefab.transform.localRotation = Quaternion.identity;
        movePrefab.transform.localScale = new Vector3(10, 10);
    }

    public void Play_SoundFX()
    {
        throw new System.NotImplementedException();
    }
}
