using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Kitsune_Script : Parent_Script, Parent_Beast
{
    [SerializeField] GameObject backPrefab;
    [SerializeField] AudioClip frontAttackSound, backAttackSound, damageSound, deathSound;

    private GameObject statusEffect;
    private string statusName;
    private int statusCounter = -1;
    public void back_special()
    {
        ProjectileAnimation();
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
        GameObject player = battleManager.getSlot(battleManager.currentTurn);
        GameObject target = battleManager.getSlot(battleManager.targets[0]);

        GameObject movePrefab = Instantiate(backPrefab);
        movePrefab.transform.SetParent(player.transform);
        movePrefab.transform.localPosition = new Vector3(0, 0);
        movePrefab.transform.localRotation = Quaternion.identity;
        movePrefab.transform.localScale = new Vector3(30, 30);

        Vector3 shootDir = ((target.transform.localPosition) - (player.transform.localPosition)).normalized;

        movePrefab.GetComponent<Projectile>().Setup(shootDir);
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

    public void checkStatusEffect() {
    //status effect is done
        if (statusCounter == 3) {
            statusCounter = -1;
            //destroy prefab
            return;
        }

        statusCounter++;
    }

    public void applyStatusEffect(string type) {
        if (statusCounter != -1) return;

        statusCounter = 0;

        Debug.Log(type + " Status effect applied ");

        statusEffect = (GameObject)Instantiate(Resources.Load("Prefabs/Moves/Move"));
        statusEffect.transform.SetParent(this.transform);


        statusEffect.transform.localPosition = new Vector2(73, 0);
        statusEffect.transform.localScale = new Vector2(1f, 1f);
        statusEffect.transform.SetAsFirstSibling();

        statusEffect.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Beasts_Moves/Mandoro/Mandoro_Move_Controller") as RuntimeAnimatorController;
        statusEffect.GetComponent<Animator>().SetTrigger("Back");
    }

    public string Beast_Name() {
        return "Kitsune";
    }
}
