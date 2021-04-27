using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
 
public class Mandoro_Script : Parent_Script, Parent_Beast
{
    [SerializeField] AudioClip frontAttackSound, backAttackSound, damageSound, deathSound;
    private GameObject emptyObj;
    public void back_special()
    {

        int ran = UnityEngine.Random.Range(1, 5);
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

        PlayBackMove();
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

    public async void PlayBackMove()
    {
        await Task.Delay(500);

        GameObject player = this.gameObject;
        GameObject target = battleManager.getSlot(battleManager.targets[0]);
        emptyObj = new GameObject("Empty");
        GameObject arm = player.transform.Find("Left Arm").gameObject;

        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        float targetX = target.transform.position.x;
        float targetY = target.transform.position.y;
        targetY += targetY > playerY ? -10 : 10;

        float deltaX = playerX - targetX;
        float deltaY = playerY - targetY;

        float delta = deltaY / deltaX;
        double tan = Mathf.Atan(delta);

        double angle = tan * (180 / Math.PI);

        //Debug.Log("playerX + playerY: " + playerX + " " + playerY);
        //Debug.Log("targetX + targetY : " + targetX + " " + targetY);
        //Debug.Log("deltaX + deltaY : " + deltaX + " " + deltaY);
        //Debug.Log("delta : " + delta);
        //Debug.Log("Angle : " + angle);

        Component[] allComp = emptyObj.GetComponents<Component>();
        if (allComp.Length == 1)
        {
            Debug.Log("fuck");
            emptyObj = new GameObject("Empty");
            emptyObj.transform.SetParent(arm.transform);
            emptyObj.transform.localPosition = new Vector2(-1.7f, 2.5f);
            emptyObj.transform.localScale = new Vector2(0.2f, 0.2f);
        }

        GameObject movePrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Moves/Move"));
        movePrefab.transform.SetParent(emptyObj.transform);


        movePrefab.transform.localPosition = new Vector2(73, 0);
        movePrefab.transform.localScale = new Vector2(playerX > targetX ? 0.8f : -0.8f, 0.1f);
        emptyObj.transform.rotation = Quaternion.Euler(0, 0, (float)angle);
        movePrefab.transform.SetAsFirstSibling();

        await Task.Delay(70);

        //movePrefab.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/Mandoro/Mandoro_Move_Controller") as RuntimeAnimatorController;
        movePrefab.GetComponent<Animator>().SetTrigger("Back");
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
