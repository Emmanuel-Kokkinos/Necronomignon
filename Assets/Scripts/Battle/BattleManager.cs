//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DragonBones;

//manages the main battle mechanics
public class BattleManager : MonoBehaviour
{
    public bool eRunning = false;
    public bool pRunning = false;

    //These are the slot objects that hold the images for the Beast
    public List<GameObject> playerPadSlots = new List<GameObject>(Values.SLOTMAX);
    public List<GameObject> enemyPadSlots = new List<GameObject>(Values.SLOTMAX);

    //These are Scripts given in unity 
    //NOTE: this script will crash if these scripts are not added before runtime
    public HealthManager healthManager;
    public Attack attack;
    public LoadMission loadMission;

    public Text txtTurn;
    Animator anim;
    UnityArmatureComponent armature;

    public List<Beast> players = new List<Beast>();
    public List<Beast> enemies = new List<Beast>();
    public List<Beast> roundOrder = new List<Beast>();
    public List<string> roundOrderTypes = new List<string>();
    public List<Beast> attackPool = new List<Beast>();
    public List<Beast> enemyAttackPool = new List<Beast>();

    public List<Beast> targets;
    public bool cancelGuard = false;


    public int turn = 0;
    int totalMoves;
    //int totalBeasts = Values.SQUADMAX * 2;

    public Beast currentTurn;
    public Beast selectedEnemy;
    public Beast selectedFriend;

    public List<Image> orderBar = new List<Image>();
    public List<Image> orderBarOutlines = new List<Image>();

    public List<Beast> slots;
    public List<Beast> enemySlots;

    public List<bool> playersActive = new List<bool>();
    public List<bool> enemiesActive= new List<bool>();

    public Summoner playerSummoner;
    public Summoner enemySummoner;

    List<int> playersTurnsTaken = new List<int>();
    List<int> enemiesTurnsTaken = new List<int>();
    int FPSTarget = 30;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FPSTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.targetFrameRate != FPSTarget)
        {
            Application.targetFrameRate = FPSTarget;
        }
    }

    //Get lists from LoadMission and add the players to the attack pool
    public void SendLists(List<Beast> thisSquad, List<Beast> enemySquad, List<HealthBar> activePlayersHealth, List<HealthBar> activeEnemiesHealth, List<DamageOutput> activePlayerDamage, List<DamageOutput> activeEnemyDamage, Summoner enemySummoner)
    {
        selectedEnemy = enemySquad[0];
        for (int x = 0; x < enemySquad.Count; x++)
        {
            if (enemySquad[x].hitPoints < selectedEnemy.hitPoints)
            {
                selectedEnemy = enemySquad[x];
            }
        }
        playerSummoner = Player.summoner;
        this.enemySummoner = enemySummoner;
        enemies = enemySquad;
        while (enemies.Count < Values.SQUADMAX) { 
            enemies.Add(null);
        }
        while (enemiesActive.Count < Values.SQUADMAX)
        {
            enemiesActive.Add(true);
        }

        for (int x =0; x < Values.SQUADMAX; x++)
        {
            if (enemies[x] != null)
            {
                enemyAttackPool.Add(enemies[x]);
            }
            else
            {
                enemiesActive[x] = false;
                //totalBeasts--;
            }
        }

        players = thisSquad;
        while (players.Count < Values.SQUADMAX)
        {
            players.Add(null);
        }
        while (playersActive.Count < Values.SQUADMAX)
        {
            playersActive.Add(true);
        }

        for (int x = 0; x < Values.SQUADMAX; x++)
        {
            if (players[x] != null)
            {

                attackPool.Add(players[x]);
            }
            else
            {
                playersActive[x] = false;
                //totalBeasts--;
            }
        }

        for(int x = 0; x < Values.SQUADMAX; x++)
        {
            if(players[x] != null)
            {
                players[x].hitPoints = players[x].maxHP;
            }
            if (enemies[x] != null)
            {
                enemies[x].hitPoints = enemies[x].maxHP;
            }
        }

        healthManager.GetHealth(players, enemies, activePlayersHealth, activeEnemiesHealth);
        LoadOrder();
    }

    //Get the slot info for each beast from LoadMission
    public void GetSlots(List<Beast> s, List<Beast> e)
    {
        slots.Clear();
        enemySlots.Clear();
        for (int x = 0; x < Values.SLOTMAX; x++)
        {
            slots.Add(s[x]);
            enemySlots.Add(e[x]);
        }
    }

    //Create attack order
    //This seems to be a big source of issues as it creats what has been dubbed "the Ultimate Crash"
    //The cause is unknown and this method is entegeral so either the issue must be found or the method must be re created from scratch
    void LoadOrder()
    {
        int[] moves = new int[Values.SQUADMAX * 2];
        //this loop takes the default amount of moves for the beast as well as the move the beast will be using
        for(int x = 0; x < Values.SQUADMAX; x++)
        {
            if (playersActive[x] && players[x] != null)
            {
                moves[x] = players[x].number_MOVES;
                if (inFront(players[x]))
                {
                    moves[x] += players[x].Move_A.number_of_moves;
                }
                else
                {
                    moves[x] += players[x].Move_B.number_of_moves;
                }
            }
            if (enemiesActive[x] && enemies[x] != null)
            {
                moves[x+ Values.SQUADMAX] = enemies[x].number_MOVES;
                enemies[x].setAttacks();
                if (inFront(enemies[x]))
                {
                    moves[x + Values.SQUADMAX] += enemies[x].Move_A.number_of_moves;
                }
                else
                {
                    moves[x + Values.SQUADMAX] += enemies[x].Move_B.number_of_moves;
                }

            }
        }

        totalMoves = moves.Sum();

        List<int> Speed = new List<int>();
        //this loop sorts by speed both freindly and enemy beasts with ties being broken by the player
        for (int x = 0; x < Values.SQUADMAX*2; x++)
        {
            
            if (x < Values.SQUADMAX && players[x] != null)
            {
                Speed.Add((int)Mathf.Floor((float)players[x].speed));
            }
            else if (x >= Values.SQUADMAX && enemies[x % Values.SQUADMAX] != null)

            {
                Speed.Add((int)Mathf.Floor((float)enemies[x % Values.SQUADMAX].speed ));
            }
            else
            {
                Speed.Add(0);
            }
        }

       // bool[] beastActive = { playersActive[0], playersActive[1], playersActive[2], playersActive[3], enemiesActive[0], enemiesActive[1], enemiesActive[2], enemiesActive[3] };
        List<bool> beastActive = new List<bool>();
        for(int x = 0; x < Values.SQUADMAX*2; x++)
        {
            if (x < Values.SQUADMAX)
            {
                beastActive.Add(playersActive[x]);
            }
            else
            {
                beastActive.Add(enemiesActive[x % Values.SQUADMAX]);
            }
        }
        int i = 0;

        List<string> wave = new List<string>();

        //Create an array with each speed and sort it highest to lowest
        Speed.Sort();
        Speed.Reverse();

        //Clear the previous round order
        roundOrder.Clear();
        roundOrderTypes.Clear();
        //Loop through the speed array and find the corresponding beast and add them to the round order
        //may also be the root of 'ultiamte crash'
        while (i < totalMoves)
        {
            for(int y = 0; y < Values.SQUADMAX*2; y++)
            {
                for(int x = 0; x < Values.SQUADMAX * 2; x++)
                {
                    if (x< Values.SQUADMAX && beastActive[x] && Speed[y] == players[x].speed && moves[x] > 0 && !InWave("Player " + players[x].name, wave))
                    {
                        roundOrder.Add(players[x]);
                        roundOrderTypes.Add("Player");
                        wave.Add("Player " + players[x].name);
                        moves[x]--;
                        i++;
                        break;
                    }
                    else if(x>= Values.SQUADMAX && beastActive[x] && Speed[y] == enemies[x% Values.SQUADMAX].speed && moves[x] > 0 && !InWave("Enemy " + enemies[x % Values.SQUADMAX].name, wave))
                    {
                        roundOrder.Add(enemies[x % Values.SQUADMAX]);
                        roundOrderTypes.Add("Enemy");
                        wave.Add("Enemy " + enemies[x % Values.SQUADMAX].name);
                        moves[x]--;
                        i++;
                        break;
                    }
                }
            }
            wave.Clear();
        }
        if (turn < roundOrder.Count)
        {
            currentTurn = roundOrder[turn];
            txtTurn.text = roundOrderTypes[0] + " " + currentTurn.name + "'s turn \n HP left: " + currentTurn.hitPoints;
        }
        while((1+turn) > roundOrderTypes.Count)
        {
            turn--;
        }
        if (roundOrderTypes[turn] == "Enemy" && attackPool.Count > 0)
        {
            if (!eRunning && !pRunning)
            {
                StartCoroutine(EnemyAttack());
            }
        }
        else if (roundOrderTypes[turn] == "Player" && enemyAttackPool.Count > 0)
        {
            if (!eRunning && !pRunning)
            {
                StartCoroutine(PlayerAttack());
            }
        }
        UpdateOrderBar();
    }

    //Check to see of beast is used in the loop yet
    bool InWave(string beast, List<string> wave)
    {
        for (int i = 0; i < wave.Count; i++)
        {
            if (beast == wave[i])
            {
                return true;
            }
        }
        return false;
    }
    //keeps the order bar showing the correct order of attack
    void UpdateOrderBar()
    {
        currentTurn = roundOrder[turn];

        foreach(Image outline in orderBarOutlines)
        {
            outline.gameObject.SetActive(true);
        }

        foreach(Image beast in orderBar)
        {
            beast.gameObject.SetActive(true);
        }

        for (int x = 0; x < orderBar.Count; x++)
        {
            if (orderBar[x].transform.childCount > 0)
            {
                foreach (UnityEngine.Transform child in orderBar[x].transform)
                {
                    Destroy(child.gameObject);
                }
            }
            try
            {
                GameObject beastPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Beasts/" + roundOrder[x + turn].name));
                beastPrefab.transform.SetParent(orderBar[x].transform);
                var pos = beastPrefab.transform.position;
                pos.y += 20;
                beastPrefab.transform.localPosition = pos;
                beastPrefab.transform.localRotation = Quaternion.identity;
                beastPrefab.transform.localScale = beastPrefab.transform.localScale * .0175f;
                beastPrefab.GetComponent<UnityArmatureComponent>().animation.Play("Idle", 1);
                beastPrefab.GetComponent<UnityArmatureComponent>().animation.Stop();

                if (roundOrderTypes[x + turn].Equals("Player"))
                {
                    orderBarOutlines[x].GetComponent<Outline>().effectColor = Color.blue;
                }
                else if (roundOrderTypes[x + turn].Equals("Enemy"))
                {
                    orderBarOutlines[x].GetComponent<Outline>().effectColor = Color.red;
                }
            }
            catch
            {
                orderBar[x].gameObject.SetActive(false);
                orderBarOutlines[x].gameObject.SetActive(false);
            }
        }
    }
    //starts the current players, or enemies attack. 
    void TakeTurn()
    {
        turn++;
        currentTurn = roundOrder[turn]; 
        txtTurn.text = roundOrderTypes[turn] + " " + currentTurn + " 's turn";
        if (pRunning) pRunning = false;
        if (eRunning) eRunning = false;
        //If it is enemy turn, start their attack
        if (roundOrderTypes[turn] == "Enemy")
        {
            if (!eRunning && !pRunning)
            {
                StartCoroutine(EnemyAttack());
            }
            UpdateOrderBar();
        }
        else if (roundOrderTypes[turn] == "Player")
        {
            if (!eRunning && !pRunning)
            {
                StartCoroutine(PlayerAttack());
            }
            UpdateOrderBar();
        }
        decroStatAndBuff();
    }

    //This method is responsible for decrementing counters for status effects (excluding Doom) and Buffs & debuffs
    void decroStatAndBuff()
    {
        //this is checking if a status effect is still active and decrimenting its counter if it is
        for (int x = 0; x < currentTurn.statusTurns.Length; x++)
        {
            if (currentTurn.statusTurns[x] > 0 && (int)Move.types.Corrupt != x)
            {
                currentTurn.statusTurns[x]--;

                // Stops status effect animation when no longer affected
                if(currentTurn.statusTurns[x] == 0)
                {
                    try
                    {
                        Destroy(getSlot(currentTurn).gameObject.transform.Find("Move(Clone)").gameObject);
                    }
                    catch
                    {

                    }
                }
            }
            if (currentTurn.statusTurns[x] > 0 && x == (int)Move.types.Burn)
            {
                //healthManager.DisplayDamageOutput(currentTurn, "5", new Color(209f / 255f, 0f / 255f, 0f / 255f));
                healthManager.UpdateHealth(currentTurn, 5);
            }
            if (currentTurn.statusTurns[x] > 0 && x == (int)Move.types.Poison)
            {
                //healthManager.DisplayDamageOutput(currentTurn, Mathf.Ceil((float)currentTurn.hitPoints * .05f).ToString(), new Color(31f / 255f, 107f / 255f, 27f / 255f));
                healthManager.UpdateHealth(currentTurn, (int)Mathf.Ceil((float)currentTurn.hitPoints * .05f));
            }
        }
        //this checks for buffs and decriments them all by 1. if the buff counter hits 0 it is removed
        for (int x = 0; x < currentTurn.buffs.Count; x++)
        {

            if (currentTurn.buffs[x].turnsLeft <= 0)
            {
                currentTurn.buffs.RemoveAt(x);
                x--;
            }
            else
            {
                currentTurn.buffs[x].turnsLeft--;
            }
        }
    }

    //this method goes through both slots untill it finds a mathching beast to the current turn and returning it's slot, and if it finds nothing it returns -1
    public int getCurrentBeastSlot()
    {
        int slot = -1;
        //loops through the slots for player and enemy beasts untill it finds thematching one
        for (int x = 0; x < slots.Count; x++)
        {
            if (roundOrderTypes[turn] == "Player" && currentTurn.Equals(slots[x]))
            {
                slot = x;
                break;
            }
            else if (roundOrderTypes[turn] == "Enemy" && currentTurn.Equals(enemySlots[x]))
            {
                slot = x;
                break;
            }
        }
        return slot;
    }
    //this method goes through both slots untill it finds a mathching beast the given beast and returning it's slot, and if it finds nothing it returns -1
    int getCurrentBeastSlot(Beast b)
    {
        int slot = -1;
        for (int x = 0; x < slots.Count; x++)
        {
            if (b.Equals(enemySlots[x]) || b.Equals(slots[x]))
            {
                slot = x;
                break;
            }
        }
        return slot;
    }
    
    //this method determines whether the attacker is in the front or back row and adjusts targets as neccisary 
    //it also initiates attacks
    public void Attack(Beast target)
    {
        bool inFront = this.inFront();
        bool guarded = this.guarded(target);
        cancelGuard = false;

        targets.Clear();
        targets.Add(target);

        //this checks if there is a beast blocking the current target as well making sure there's nothing that would need to cancel the block 
        //like healing for example
        if (guarded && !cancelGuard)
        {
            int slot = getCurrentBeastSlot(targets[targets.Count-1]);
            targets.Clear();
            Beast b = new Beast();
            if (roundOrderTypes[turn] == "Player")
            {
                //this takes the slot of the targeted beast and finds the beast directly in front of it
                for (int x = (Values.SLOTMAX/2); x < enemySlots.Count; x++)
                {
                    if (x == slot && enemySlots[x % (Values.SMALLSLOT / 2)] != null && enemySlots[x % (Values.SMALLSLOT / 2)].hitPoints > 0)
                    {
                        b = enemySlots[x % (Values.SMALLSLOT / 2)];
                    }
                }
            }
            else
            {
                for (int x = (Values.SMALLSLOT/2); x < slots.Count; x++)
                {
                    if (x == slot && slots[x % (Values.SMALLSLOT / 2)] != null && slots[x % (Values.SMALLSLOT / 2)].hitPoints > 0)
                    {
                        b = slots[x % (Values.SMALLSLOT / 2)];
                    }
                }
            }
            targets.Clear();
            targets.Add(b);
        }
        //this method currently needs improvment
        //this method checks if the current turn beast is confused and will make them target a friendly beast
        if (currentTurn.statusTurns[(int)Move.types.Confusion] > 0)
        {
            targets.Clear();
            if (roundOrderTypes[turn] == "Player")
            {
                targets.Add(GetEnemyTarget());
            }
            else
            {
                targets.Add(GetPlayerTarget());
            }
        }
        //this checks if the current beast has a doom target
        //this is to ensure that whomever has been targeted is still alive as to not kill them twice
        if (currentTurn.cursed != null)
        {
            if (currentTurn.cursed.hitPoints <= 0)
            {
                currentTurn.cursed = null;
                currentTurn.curseCharge = 0;

            }
            else
            {
                targets.Clear();
                targets.Add(currentTurn.cursed);
            }
        }

        if (targets[0] == null || targets[0].speed == 0)
        {
            print(targets[0]);
            print(roundOrderTypes[turn]);
            if (roundOrderTypes[turn] == "Player")
            {

                Attack(GetPlayerTarget());
                return;
            }
            else
            {
                Attack(GetEnemyTarget());
                return;
            }
        }

        ChangeSquadLayers(targets);

        //Check to see if the round is still going and then run an attack
        if (turn >= totalMoves - 1)
        {
            PlayAttackAnimation(inFront);

            GameObject slot = getSlot();
            if (!slot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Front") &&
                !slot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Back"))
            {
                if (pRunning) pRunning = false;
                if (eRunning) eRunning = false;
            }

            Debug.Log("Round Ended");
            ClearTurns();
            currentTurn = roundOrder[0];
            txtTurn.text = roundOrderTypes[0] + " " + currentTurn + "'s turn";
            turn = 0;

            if (healthManager.playersLeft > 0 && healthManager.enemiesLeft > 0 && roundOrderTypes[turn] == "Enemy")
            {
                StartCoroutine(EnemyAttack());
            }
            else if (healthManager.enemiesLeft > 0 && healthManager.playersLeft > 0 && roundOrderTypes[turn] == "Player")
            {
                StartCoroutine(PlayerAttack());
            }
        }
        else
        {
            PlayAttackAnimation(inFront);

            GameObject slot = getSlot();
            if (!slot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Front") &&
                !slot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Back"))
            {
                if (pRunning) pRunning = false;
                if (eRunning) eRunning = false;
            }
            AddTurn();
            Beast b = new Beast();
            if(turn+1 >= roundOrder.Count)
            {
                while (!b.Equals(currentTurn))
                {
                    b = roundOrder[turn];
                    turn--;
                }
            }
            TakeTurn();
        }
    }

    //Returns slot GameObject of currentturn.
    GameObject getSlot()
    {
        if (roundOrderTypes[turn] == "Player")
        {
            //loops untill it finds the matching beast
            for (int x = 0; x < slots.Count; x++)
            {
                if (slots[x] != null && currentTurn.name == slots[x].name)
                {
                    return playerPadSlots[x];
                }
            }
        }
        else if (roundOrderTypes[turn] == "Enemy")
        {
            for (int x = 0; x < enemySlots.Count; x++)
            {
                if (enemySlots[x] != null && currentTurn.name == enemySlots[x].name)
                {
                    return enemyPadSlots[x];
                }
            }
        }

        return null;
    }

    public GameObject getSlot(Beast b)
    {
        for(int x = 0; x < slots.Count; x++)
        {
            if (slots[x] != null && b.Equals(slots[x]))
            {
                return playerPadSlots[x];
            }
            else if (enemySlots[x] != null && b.Equals(enemySlots[x]))
            {
                return enemyPadSlots[x];
            }
        }
        return null;
    }

    //plays the attacking animation for either front or back row attack depending on the bool
    public void PlayAttackAnimation(bool inFront)
    {
        GameObject slot = getSlot();
        slot = slot.transform.GetChild(0).gameObject;
        Parent_Beast beast = slot.GetComponent<Parent_Beast>();
        armature = slot.GetComponent<UnityArmatureComponent>();

        if (inFront)
        {
            armature.animation.Play("Front", 1);
            StartCoroutine(AnimationWaitTime(armature));

            if (beast != null)
            {
                beast.Play_SoundFX("front");
                beast.front_special();
            }
        }
        else
        {
            armature.animation.Play("Back", 1);
            StartCoroutine(AnimationWaitTime(armature));

            if (beast != null)
            {
                beast.Play_SoundFX("back");
                beast.back_special();
            }
        }
    }

    //this plays the damage animation for one or many beasts
    public void PlayDamagedAnimation(List<Beast> targets)
    {
        foreach(Beast b in targets)
        {
            if(b != null)
            {
                PlayDamagedAnimation(b);
            }
        }
    }
    public void PlayDamagedAnimation(Beast target)
    {
        if (roundOrderTypes[turn] == "Player")
        {
            //loops through the slots until it finds a matching beast to the target
            for (int x = 0; x < enemySlots.Count; x++)
            {
                if (enemySlots[x] != null && enemySlots[x].Equals(target))
                {
                    Parent_Beast beast = enemyPadSlots[x].transform.GetChild(0).GetComponent<Parent_Beast>();
                    beast.Play_SoundFX("damage");

                    StartCoroutine(ChangeBattleColor(enemyPadSlots[x].transform.GetChild(0).gameObject));
                    armature = enemyPadSlots[x].transform.GetChild(0).gameObject.GetComponent<UnityArmatureComponent>();
                    armature.animation.Play("Damage", 1);
                    StartCoroutine(AnimationWaitTime(armature));
                    break;
                }
            }
        }
        else if (roundOrderTypes[turn] == "Enemy")
        {
            for (int x = 0; x < slots.Count; x++)
            {
                if (slots[x] != null && slots[x].Equals(target))
                {
                    Parent_Beast beast = playerPadSlots[x].transform.GetChild(0).GetComponent<Parent_Beast>();
                    beast.Play_SoundFX("damage");

                    StartCoroutine(ChangeBattleColor(playerPadSlots[x].transform.GetChild(0).gameObject));
                    armature = playerPadSlots[x].transform.GetChild(0).gameObject.GetComponent<UnityArmatureComponent>();
                    armature.animation.Play("Damage", 1);
                    StartCoroutine(AnimationWaitTime(armature));
                    break;
                }
            }
        }
    }

    //Change the color of the target for a split second to show the damage type
    //                               ****Need to fix for DragonBones****
    IEnumerator ChangeBattleColor(GameObject beast)
    {
        //beast.gameObject.transform.GetChild(0).GetComponent<Image>().color = attack.GetTypeColor(currentTurn);
        yield return new WaitForSeconds(.1f);
        //beast.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.white;
    }

    //starts the attacking cycle for the enemy
    IEnumerator EnemyAttack()
    {
        eRunning = true;
        yield return new WaitForSeconds(2f);
        if (attackPool.Count > 0)
        {
            Attack(GetEnemyTarget());
        }
    }

    //Enemy targets a random player from a pool of active player beasts
    Beast GetEnemyTarget()
    {
        int rand = Random.Range(0, attackPool.Count);

        Beast b = attackPool[rand];
        if (Player.RedRoach)
        {
            b = RedRoachEnemyTarget();
        }

        targets.Clear();
        targets.Add(b);
        return b;
    }
    Beast RedRoachEnemyTarget()
    {
        Beast b = attackPool[0];
        List<Beast> beasts = new List<Beast>();
        foreach (Beast be in attackPool)
        {
            print(calculateType(currentTurn, be));
            print(calculateType(currentTurn, b));
            if (calculateType(currentTurn, be) > calculateType(currentTurn, b))
            {
                b = be;
                beasts.Add(be);
            }
        }
        print(beasts.Count);
        print(b);
        if(beasts.Count > 1)
        {
            for(int x = 0; x < beasts.Count; x++)
            {
                if (beasts[x].hitPoints < b.hitPoints)
                {
                    b = beasts[x];
                }
            }
        }
        else if(beasts.Count < 1)
        {
            foreach (Beast be in attackPool)
            {
                if (be.hitPoints < b.hitPoints)
                {
                    b = be;
                }
            }
        }

        return b;
    }
    float calculateType(Beast attacker, Beast target)
    {
        if (attack == null || target == null)
        {
            return 0;
        }
        int[] attackertype = new int[2];
        attackertype[0] = (int)attacker.type;
        attackertype[1] = (int)attacker.secondType;
        int[] defendertype = new int[2];
        defendertype[0] = (int)target.type;
        defendertype[1] = (int)target.secondType;
        float modifier = 1;
        
        for (int x = 0; x < attackertype.Length; x++)
        {
            for (int y = 0; y < defendertype.Length; y++)
            {
                //checks to make sure that neither type is normal, which as no strength or weakness
                if ((attackertype[x] != (int)Beast.types.Normal) && (defendertype[y] != (int)Beast.types.Normal))
                {

                    switch (attackertype[x])
                    {
                        case (int)Beast.types.Water:
                            if (defendertype[y] == (int)Beast.types.Fire || defendertype[y] == (int)Beast.types.Cosmic)
                            {
                                print("super");
                                modifier *= 1.5f;
                            }
                            if (defendertype[y] == (int)Beast.types.Air || defendertype[y] == (int)Beast.types.Horror)
                            {
                                print("not so good");
                                modifier *= 0.75f;
                            }
                            break;

                        case (int)Beast.types.Fire:
                            if (defendertype[y] == (int)Beast.types.Earth || defendertype[y] == (int)Beast.types.Horror)
                            {
                                print("super");
                                modifier *= 1.5f;
                            }
                            if (defendertype[y] == (int)Beast.types.Water || defendertype[y] == (int)Beast.types.Cosmic)
                            {
                                print("not so good");
                                modifier *= 0.75f;
                            }
                            break;

                        case (int)Beast.types.Earth:
                            if (defendertype[y] == (int)Beast.types.Air || defendertype[y] == (int)Beast.types.Cosmic)
                            {
                                print("super");
                                modifier *= 1.5f;
                            }
                            if (defendertype[y] == (int)Beast.types.Fire || defendertype[y] == (int)Beast.types.Horror)
                            {
                                print("not so good");
                                modifier *= 0.75f;
                            }
                            break;

                        case (int)Beast.types.Air:
                            if (defendertype[y] == (int)Beast.types.Water || defendertype[y] == (int)Beast.types.Horror)
                            {
                                print("super");
                                modifier *= 1.5f;
                            }
                            if (defendertype[y] == (int)Beast.types.Earth || defendertype[y] == (int)Beast.types.Cosmic)
                            {
                                print("not so good");
                                modifier *= 0.75f;
                            }
                            break;
                        case (int)Beast.types.Dark:
                            if (defendertype[y] == (int)Beast.types.Light || defendertype[y] == (int)Beast.types.Horror)
                            {
                                print("super");
                                modifier *= 1.5f;
                            }
                            if (defendertype[y] == (int)Beast.types.Cosmic)
                            {
                                print("not so good");
                                modifier *= 0.75f;
                            }
                            break;
                        case (int)Beast.types.Light:
                            if (defendertype[y] == (int)Beast.types.Dark || defendertype[y] == (int)Beast.types.Cosmic)
                            {
                                print("super");
                                modifier *= 1.5f;
                            }
                            if (defendertype[y] == (int)Beast.types.Horror)
                            {
                                print("not so good");
                                modifier *= 0.75f;
                            }
                            break;
                        case (int)Beast.types.Horror:
                            if (defendertype[y] == (int)Beast.types.Light || defendertype[y] == (int)Beast.types.Earth || defendertype[y] == (int)Beast.types.Water)
                            {
                                print("super");
                                modifier *= 1.5f;
                            }
                            if (defendertype[y] == (int)Beast.types.Dark || defendertype[y] == (int)Beast.types.Fire || defendertype[y] == (int)Beast.types.Air)
                            {
                                print("not so good");
                                modifier *= 0.75f;
                            }
                            break;
                        case (int)Beast.types.Cosmic:
                            if (defendertype[y] == (int)Beast.types.Fire || defendertype[y] == (int)Beast.types.Air || defendertype[y] == (int)Beast.types.Dark)
                            {
                                print("super");
                                modifier *= 1.5f;
                            }
                            if (defendertype[y] == (int)Beast.types.Earth || defendertype[y] == (int)Beast.types.Water || defendertype[y] == (int)Beast.types.Light)
                            {
                                print("not so good");
                                modifier *= 0.75f;
                            }
                            break;
                    }

                }
            }
        }
        return modifier;

    }
    //gets a random target from the enemy team
    public Beast GetPlayerTarget()
    {
        int rand = Random.Range(0, enemyAttackPool.Count);

        Beast b = enemyAttackPool[rand];        
        return b;
    }
    //starts the attacking cycle for the player
    IEnumerator PlayerAttack()
    {
        pRunning = true;
        yield return new WaitForSeconds(2f);
        //pRunning = false;
        if (enemyAttackPool.Count > 0)
        {
            Attack(selectedEnemy);
        }
    }

    // Changes the layers of the attacker and target so the projectiles only hit the target
    void ChangeSquadLayers(List<Beast> targets)
    {
        // Resets all of the prefabs to the Player layer
        foreach (Beast b in attackPool)
        {
            GameObject slot = getSlot(b);
            slot.transform.GetChild(0).gameObject.layer = 8;
        }

        foreach (Beast b in enemyAttackPool)
        {
            GameObject slot = getSlot(b);
            slot.transform.GetChild(0).gameObject.layer = 8;
        }

        // Sets the attacker to the Player layer
        GameObject player = getSlot(currentTurn);
        player.transform.GetChild(0).gameObject.layer = 8;

        // Sets the target to the Enemy layer
        foreach (Beast target in targets)
        {
            GameObject enemy = getSlot(target);
            enemy.transform.GetChild(0).gameObject.layer = 9;
        }
    }

    //Get the row to determine whether the current turn beast is using an A move or a B move
    public bool inFront()
    {
        if(currentTurn.size == 1)
        {
            return true;
        }
        for(int x = 0; x< slots.Count; x++)
        {
            if(x < (Values.SMALLSLOT / 2) && (currentTurn.Equals(slots[x]) || currentTurn.Equals(enemySlots[x])))
            {
                return true;
            }
        }
        return false;
    }

    //Determines if the target has a given beast in front of them 
    bool guarded(Beast b)
    {
        if (inFront(b))
        {
            return false;
        }

        int slot = -1;
        for (int x = 0; x < slots.Count; x++)
        {
            if (b.Equals(slots[x]))
            {
                slot = x;
                break;
            }
            else if (b.Equals(enemySlots[x]))
            {
                slot = x;
                break;
            }
        }

        if (roundOrderTypes[turn] == "Player")
        {
            for (int x = (Values.SMALLSLOT / 2); x < enemySlots.Count; x++)
            {
                if(x == slot && enemySlots[x % (Values.SMALLSLOT / 2)] != null && enemySlots[x % (Values.SMALLSLOT / 2)].hitPoints > 0)
                {
                    return true;
                }
            }
        }
        else
        {
            for (int x = (Values.SMALLSLOT / 2); x < slots.Count; x++)
            {
                if (x == slot && slots[x % (Values.SMALLSLOT / 2)] != null && slots[x % (Values.SMALLSLOT / 2)].hitPoints > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //Get the row to determine whether a given beast is in the front or back row
    bool inFront(Beast b)
    {
        if(b.size == 1)
        {
            return true;
        }

        for (int x = 0; x < slots.Count; x++)
        {
            if (x < (Values.SMALLSLOT / 2) && (b.Equals(slots[x]) || b.Equals(enemySlots[x])))
            {
                return true;
            }
        }
        return false;
    }

    //Remove the desired beast by setting its active variable to false and removing image
    public void RemoveBeast(Beast target)
    {
        //totalBeasts --;

        if(!target.isEnemy)
        {
            if (playersTurnsTaken.Count < Values.SQUADMAX)
            {
                while (playersTurnsTaken.Count < Values.SQUADMAX)
                {
                    playersTurnsTaken.Add(0);
                }
            }
            for (int x = 0; x < players.Count; x++)
            {
                if (target.Equals(players[x]))
                {
                    playersActive[x] = false;
                    loadMission.RemoveImage(players[x], "Player");
                    attackPool.Remove(players[x]);
                    turn -= playersTurnsTaken[x];
                }
            }
        }
        else if (target.isEnemy)
        {
            if(enemiesTurnsTaken.Count() <= 0)
            {
                while (enemiesTurnsTaken.Count < Values.SQUADMAX)
                {
                    enemiesTurnsTaken.Add(0);
                }
            }
            for (int x = 0; x < enemies.Count; x++)
            {
                if (target.Equals(enemies[x]))
                {
                    enemiesActive[x] = false;
                    enemyAttackPool.Remove(enemies[x]);
                    loadMission.RemoveImage(enemies[x], "Enemy");
                    turn -= enemiesTurnsTaken[x];
                }
            }
        }

        //Set selected enemy to lowest hp enemy by default if the previously selected enemy was killed.
        if (target.Equals(selectedEnemy))
        {
            if (enemyAttackPool.Count > 0)
            {
                selectedEnemy = enemyAttackPool[0];
                for (int x = 0; x < enemyAttackPool.Count; x++)
                {
                    if (enemyAttackPool[x].hitPoints < selectedEnemy.hitPoints)
                    {
                        selectedEnemy = enemyAttackPool[x];
                    }
                }
            }
        }

        LoadOrder(); //Re Create the round order
    }

    //Add turn to keep track of how many times a beast has went in the round
    //Used to keep track of how much the turn variable has to be edited by when a beast gets knocked out
    void AddTurn()
    {
        if (playersTurnsTaken.Count < Values.SQUADMAX)
        {
            playersTurnsTaken.Clear();
            while (playersTurnsTaken.Count < Values.SQUADMAX)
            {
                playersTurnsTaken.Add(0);
            }
        }
        if (enemiesTurnsTaken.Count < Values.SQUADMAX)
        {
            enemiesTurnsTaken.Clear();
            while (enemiesTurnsTaken.Count < Values.SQUADMAX)
            {
                enemiesTurnsTaken.Add(0);
            }
            
        }

        if (roundOrderTypes[turn] == "Player")
        {
            for(int x= 0; x < players.Count; x++)
            {
                if (currentTurn == players[x]) playersTurnsTaken[x] += 1;
            }
        }
        else
        {
            for (int x = 0; x < enemies.Count; x++)
            {
                if (currentTurn == enemies[x]) enemiesTurnsTaken[x] += 1;
            }
        }
    }
    //gets the name of the static image for any given beast
    string GetImage(Beast beast)
    {
        return beast.static_img;
    }
    //sets everyones turns to 0
    void ClearTurns()
    {   
            playersTurnsTaken.Clear();
            playersTurnsTaken.Add(0);
            playersTurnsTaken.Add(0);
            playersTurnsTaken.Add(0);
            playersTurnsTaken.Add(0);
            enemiesTurnsTaken.Clear();
            enemiesTurnsTaken.Add(0);
            enemiesTurnsTaken.Add(0);
            enemiesTurnsTaken.Add(0);
            enemiesTurnsTaken.Add(0);  
    }  
    
    public bool isSquadFull(string squad)
    {
        int y = 0;

        if (squad == "Player")
        {
            for (int x = 0; x < Values.SLOTMAX; x++)
            {
                if ((slots[x] != null && slots[x].speed != 0 && slots[x].hitPoints > 0) && x < Values.SMALLSLOT)
                {
                    y++;
                }
                else if ((slots[x] != null && slots[x].speed != 0 && slots[x].hitPoints > 0) && x >= Values.SMALLSLOT)
                {
                    y += 4;
                }
            }
        }
        else
        {
            for (int x = 0; x < Values.SLOTMAX; x++)
            {
                if ((enemySlots[x] != null && enemySlots[x].speed != 0 && enemySlots[x].hitPoints > 0) && x < Values.SMALLSLOT)
                {
                    y++;
                }
                else if((enemySlots[x] != null && enemySlots[x].speed != 0 && enemySlots[x].hitPoints > 0) && x >= Values.SMALLSLOT)
                {
                    y += 4;
                }
            }
        }
        return y == 8;
    }

    // Wait for the animation to finish then go back to Idle animation
    public IEnumerator AnimationWaitTime(UnityArmatureComponent beast)
    {
        yield return new WaitUntil(() => beast.animation.isCompleted);
        beast.animation.Play("Idle", 0);
    }

    IEnumerator PauseWhile(System.Func<bool> condition)
    {
        if (condition == null)
            yield break;
        while (!condition())
            yield return null;
    }

}
