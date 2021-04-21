using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Mandoro_Script : MonoBehaviour, Parent_Beast
{
    BattleManager battleManager;
    AudioClip frontAttackSound, backAttackSound, startSound, deathSound;
    AudioSource audioSrc;
    Attack attack;
    GameObject emptyObj;

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

    public void PlayBackMove()
    {
        GameObject player = this.gameObject;
        GameObject target = battleManager.getSlot(battleManager.targets[0]);
        emptyObj = new GameObject("Empty");

        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        float targetX = target.transform.position.x ;
        float targetY = target.transform.position.y;
        targetY += targetY > playerY? - 10 : 10;

        float deltaX = playerX - targetX;
        float deltaY = playerY - targetY;

        float delta = deltaY / deltaX;
        double tan = Mathf.Atan(delta);

        double angle = tan * (180 / Math.PI);
        //double angle = Math.Tan(delta);

        Debug.Log("playerX + playerY: " + playerX + " " + playerY);
        Debug.Log("targetX + targetY : " + targetX + " " + targetY);
        Debug.Log("deltaX + deltaY : " + deltaX + " " + deltaY);
        Debug.Log("delta : " + delta);
        Debug.Log("Angle : " + angle);

        Component[] allComp = emptyObj.GetComponents<Component>();
        if (allComp.Length == 1){
            Debug.Log("fuck");
            emptyObj = new GameObject("Empty");
            emptyObj.transform.SetParent(player.transform);
            emptyObj.transform.localPosition = new Vector3(-12 , 12);
            emptyObj.transform.localScale = new Vector2(0.2f, 0.2f);
        }
        
        GameObject movePrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Move"));
        movePrefab.transform.SetParent(emptyObj.transform);
        

        movePrefab.transform.localPosition = new Vector2(550, 0);
        emptyObj.transform.rotation = Quaternion.Euler(0, 0, (float)angle);
        movePrefab.transform.localScale = new Vector2(playerX > targetX ? -11f : 11f, 0.4f);
        movePrefab.transform.SetAsFirstSibling();

        movePrefab.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/Mandoro/Mandoro_Move_Controller") as RuntimeAnimatorController;
        movePrefab.GetComponent<Animator>().SetTrigger("Back");
    }

    public void back_special()
    {
        int ran = UnityEngine.Random.Range(1, 5);
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
