using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;


public class DreamSlime_Script : Parent_Script, Parent_Beast
{
    LoadMission loadMission;
    HealthManager healthManager;

    [SerializeField] GameObject backPrefab;
    [SerializeField] AudioClip frontAttackSound, backAttackSound, damageSound, deathSound;

    void Start()
    {
        GameObject g = GameObject.Find("GameManager");

        if (g != null)
        {
            loadMission = g.GetComponent<LoadMission>();
            healthManager = g.GetComponent<HealthManager>();
        }
        
        base.start();
    }

    public void PlayBackMove()
    {
        GameObject target = battleManager.getSlot(battleManager.targets[0]);

        GameObject movePrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Move"));
        movePrefab.transform.SetParent(target.transform);
        movePrefab.transform.localPosition = new Vector3(0, 100);
        movePrefab.transform.localRotation = Quaternion.identity;
        movePrefab.transform.localScale = new Vector3(2f, 2f);

        movePrefab.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/DreamSlime/DreamSlime_Move_Controller") as RuntimeAnimatorController;
        movePrefab.GetComponent<Animator>().SetTrigger("Back");
    }

    public void PlayFrontMove()
    {
        GameObject target = battleManager.getSlot(battleManager.targets[0]);

        GameObject movePrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Move"));
        movePrefab.transform.SetParent(target.transform);
        movePrefab.transform.localPosition = new Vector3(0, 100);
        movePrefab.transform.localRotation = Quaternion.identity;
        movePrefab.transform.localScale = new Vector3(2f, 2f);

        movePrefab.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/DreamSlime/DreamSlime_Move_Controller") as RuntimeAnimatorController;
        movePrefab.GetComponent<Animator>().SetTrigger("Front");
    }

    public void back_special()
    {
        int slot = -1;
        int ran = Random.Range(0, 10);
        if (ran < 3 && battleManager.roundOrderTypes[battleManager.turn] == "Player" && !battleManager.isSquadFull("Player"))
        {
            while (slot == -1)
            {
                ran = Random.Range(0, Values.SMALLSLOT);
                if ((battleManager.slots[8] == null || (ran != 0 && ran != 1 && ran != 4 && ran != 5)) &&
                    (battleManager.slots[9] == null || (ran != 1 && ran != 2 && ran != 5 && ran != 6)) &&
                    (battleManager.slots[10] == null || (ran != 2 && ran != 3 && ran != 6 && ran != 7)))
                {

                    if ((battleManager.slots[ran] == null || battleManager.slots[ran].speed == 0 || battleManager.slots[ran].hitPoints <= 0)
                        )
                    {
                        battleManager.playerPadSlots[ran].transform.gameObject.SetActive(true);
                        GameObject beastPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Beasts/" + "DreamSlime"));
                        beastPrefab.transform.SetParent(battleManager.playerPadSlots[ran].transform);
                        beastPrefab.transform.localPosition = new Vector3(0, 0);
                        beastPrefab.transform.localRotation = Quaternion.identity;
                        beastPrefab.transform.localScale = new Vector3(20f, 20f);
                        beastPrefab.GetComponent<UnityArmatureComponent>().animation.Play("Idle", 0);
                        slot = ran;
                    }
                }
            }
            battleManager.slots[slot] = BeastManager.getFromNameS("DreamDummy");
            battleManager.slots[slot].hitPoints = battleManager.slots[slot].maxHP;
            battleManager.attackPool.Add(battleManager.slots[slot]);
            loadMission.playerSlot[slot] = (battleManager.slots[slot]);
            //healthManager.playersLeft++;
            battleManager.slots[slot].nonCombatant = true;

            //health display
            loadMission.playerDisplaySlots[slot].gameObject.SetActive(true);
            loadMission.playerImgs[slot].sprite = Resources.Load<Sprite>("Static_Images/DreamSlime_Idle_00");
            healthManager.playerHealthBars.Add(loadMission.playerHealthBars[slot]);
            healthManager.playerHealthBars[healthManager.playerHealthBars.Count - 1].SetMaxHealth(battleManager.slots[slot].maxHP);
            healthManager.playerHealths.Add(healthManager.playerHealthsSaved[slot]);
            healthManager.playerHealths[healthManager.playerHealths.Count - 1].gameObject.SetActive(true);
            healthManager.playerHealths[healthManager.playerHealths.Count - 1].text = battleManager.slots[slot].maxHP.ToString();

            for (int x = 0; x < battleManager.players.Count; x++)
            {
                if (battleManager.players[x] == null || battleManager.players[x].speed == 0 || battleManager.players[x].hitPoints <= 0)
                {
                    battleManager.players[x] = battleManager.slots[slot];
                    battleManager.playersActive[x] = true;
                    break;
                }
            }
        }
        else if (ran < 3 && battleManager.roundOrderTypes[battleManager.turn] == "Enemy" && !battleManager.isSquadFull("Enemy"))
        {
            while (slot == -1)
            {
                ran = Random.Range(0, Values.SMALLSLOT);
                if ((battleManager.enemySlots[8] == null || (ran != 0 && ran != 1 && ran != 4 && ran != 5)) &&
                    (battleManager.enemySlots[9] == null || (ran != 1 && ran != 2 && ran != 5 && ran != 6)) &&
                    (battleManager.enemySlots[10] == null || (ran != 2 && ran != 3 && ran != 6 && ran != 7)))
                {
                    if (battleManager.enemySlots[ran] == null || battleManager.enemySlots[ran].speed == 0 || battleManager.enemySlots[ran].hitPoints <= 0)
                    {
                        battleManager.enemyPadSlots[ran].transform.gameObject.SetActive(true);
                        GameObject beastPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Beasts/" + "DreamSlime"));
                        beastPrefab.transform.SetParent(battleManager.enemyPadSlots[ran].transform);
                        beastPrefab.transform.localPosition = new Vector3(0, 0);
                        beastPrefab.transform.localRotation = Quaternion.identity;
                        beastPrefab.transform.localScale = new Vector3(20f, 20f);
                        slot = ran;
                    }
                }
            }
            battleManager.enemySlots[slot] = BeastManager.getFromNameS("DreamDummy");
            battleManager.enemySlots[slot].hitPoints = battleManager.enemySlots[slot].maxHP;
            battleManager.enemyAttackPool.Add(battleManager.enemySlots[slot]);
            loadMission.enemySlot[slot] = (battleManager.enemySlots[slot]);
            //healthManager.enemiesLeft++;

            //health display
            loadMission.enemyDisplaySlots[slot].gameObject.SetActive(true);
            loadMission.enemyImgs[slot].sprite = Resources.Load<Sprite>("Static_Images/DreamSlime_Idle_00");
            healthManager.enemyHealthBars.Add(loadMission.enemyHealthBars[slot]);
            healthManager.enemyHealthBars[healthManager.enemyHealthBars.Count - 1].SetMaxHealth(battleManager.enemySlots[slot].maxHP);
            healthManager.enemyHealths.Add(healthManager.enemyHealthsSaved[slot]);
            healthManager.enemyHealths[healthManager.enemyHealths.Count - 1].gameObject.SetActive(true);
            healthManager.enemyHealths[healthManager.enemyHealths.Count - 1].text = battleManager.enemySlots[slot].maxHP.ToString();
            for (int x = 0; x < battleManager.enemies.Count; x++)
            {
                if (battleManager.enemies[x] == null || battleManager.enemies[x].speed == 0 || battleManager.enemies[x].hitPoints <= 0)
                {
                    battleManager.enemies[x] = battleManager.enemySlots[slot];
                    battleManager.enemiesActive[x] = true;
                    break;
                }
            }
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
