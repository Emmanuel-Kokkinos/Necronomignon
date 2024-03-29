﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DragonBones;

/**
 * Handles the turn order and attacking in the demo battle scene
 */
public class DemoBattleManager : MonoBehaviour
{
    public LoadDemoBattle loadDemoBattle;
    public SquadData squadData;
    public Attack attack;
    public HealthManager healthManager;

    public List<Image> slotImgs; // Slots in the squad
    public List<Beast> slots; // Beast in each slot including nulls
    public List<Image> orderImgs; // Static images for the battle order
    List<Beast> thisSquad = new List<Beast>(); // Squad being used in battle not including nulls
    List<Beast> roundOrder = new List<Beast>(); // Order in which beasts attack
    List<Beast> enemies = new List<Beast>(); // The target
    List<int> moves = new List<int>(); // Number of moves per beast
    List<int> speeds = new List<int>(); // Speeds of each beast

    public int totalDamage;
    public Text totalDamageText;
    int turn = 0;
    int totalMoves;
    int squadNumber;
    public bool attackingAnim = false;
    public Beast currentTurn;
    Beast b;
    UnityArmatureComponent armature;

    // Start is called before the first frame update
    void Start()
    {
        b = new Beast();
        b.name = "Target";
        b.hitPoints = 1000000;
        b.defence = 10;
        b.power = 10;
        b.speed = 10;
        b.dexterity = 10;
        b.number_MOVES = 0;
        enemies.Add(b);
        enemies.Add(null);
        enemies.Add(null);
        enemies.Add(null);
        squadNumber = loadDemoBattle.GetSquadNumber();
        LoadSquadImages();
        LoadOrder();
    }

    // Update is called once per frame
    private void Update()
    {
        GameObject go = GameObject.Find("DefenceSlider");

        // Slider used to modify targets defence
        if(go != null)
        {
            Slider slide = go.GetComponent<Slider>();
            enemies[0].defence = (int)slide.value;
        }
    }

    // Loads the animations of the players squad
    void LoadSquadImages()
    {
        List<Beast> toLoad = new List<Beast>(); // Squad in use
        toLoad = squadData.GetSquadList(squadNumber);

        for(int x = 0; x < toLoad.Count; x++)
        {
            if(toLoad[x] != null)
            {
                slots.Add(toLoad[x]);

                slotImgs[x].sprite = Resources.Load<Sprite>("Static_Images/EmptyRectangle");

                GameObject beastPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Beasts/" + toLoad[x].name));
                beastPrefab.transform.SetParent(slotImgs[x].transform);
                beastPrefab.transform.localPosition = beastPrefab.transform.position;
                beastPrefab.transform.localRotation = Quaternion.identity;
                beastPrefab.transform.localScale = beastPrefab.transform.localScale * .25f;

                beastPrefab.GetComponent<UnityArmatureComponent>().animation.Play("Idle", 0);

                thisSquad.Add(toLoad[x]);
            }
            else
            {
                slots.Add(null);
                slotImgs[x].gameObject.SetActive(false);
            }
        }
    }

    // Returns the static image of the beast
    string GetImage(Beast beast)
    {
        return beast.static_img;
    }

    // Loads the order of attacks for a round
    void LoadOrder()
    {
        for (int x = 0; x < thisSquad.Count; x++)
        {
            if( thisSquad[x] != null)
            {
                moves.Add(thisSquad[x].number_MOVES);
                speeds.Add(thisSquad[x].speed);
            }
        }

        foreach(int x in moves)
        {
            totalMoves += x;
        }

        speeds.Sort();
        speeds.Reverse();

        int i = 0;

        // Adds beasts to the order
        // When everyone has attacked once, repeat until nobody has any moves left
        while (i < totalMoves)
        {
            for (int x = 0; x < thisSquad.Count; x++)
            {
                for(int y = 0; y < thisSquad.Count; y++)
                {
                    if(speeds[x] == thisSquad[y].speed && moves[y] > 0)
                    {
                        roundOrder.Add(thisSquad[y]);
                        moves[y]--;
                        i++;
                    }
                }
            }
        }

        UpdateOrderBar();
    }

    // Updates the order bar after each attack to show who is attacking next
    void UpdateOrderBar()
    {
        currentTurn = roundOrder[turn];

        foreach (Image beast in orderImgs)
        {
            beast.gameObject.SetActive(true);
        }

        for (int x = 0; x < orderImgs.Count; x++)
        {
            if (orderImgs[x].transform.childCount > 0)
            {
                foreach (UnityEngine.Transform child in orderImgs[x].transform)
                {
                    Destroy(child.gameObject);
                }
            }
            try
            {
                GameObject beastPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Beasts/" + roundOrder[x + turn].name));
                beastPrefab.transform.SetParent(orderImgs[x].transform);
                var pos = beastPrefab.transform.position;
                pos.y += 15;
                beastPrefab.transform.localPosition = pos;
                beastPrefab.transform.localRotation = Quaternion.identity;
                beastPrefab.transform.localScale = beastPrefab.transform.localScale * .15f;
                beastPrefab.GetComponent<UnityArmatureComponent>().animation.Play("Idle", 1);
                beastPrefab.GetComponent<UnityArmatureComponent>().animation.Stop();
            }
            catch
            {
                orderImgs[x].gameObject.SetActive(false);
            }
        }
    }

    // Attacks the target with whoevers turn it is
    public void Attack()
    {
        bool inFront = this.inFront();

        PlayAttackAnimation(inFront);

        List<Beast> beThe = new List<Beast>();
        beThe.Add(b);

        attack.InitiateAttack(currentTurn, beThe, inFront, Player.summoner);
        totalDamageText.text = totalDamage.ToString();

        TakeTurn();
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
                //beast.front_special();
            }
        }
        else
        {
            armature.animation.Play("Back", 1);
            StartCoroutine(AnimationWaitTime(armature));

            if (beast != null)
            {
                beast.Play_SoundFX("back");
                //beast.back_special();
            }
        }
    }

    // Uses a beasts turn when they attack and ends the round if they are the last beast in the round order
    void TakeTurn()
    {
        if (turn == totalMoves - 1)
        {
            Debug.Log("Round Ended");
            turn = 0;
            UpdateOrderBar();
        }
        else
        {
            turn++;
            UpdateOrderBar();
            Debug.Log(currentTurn + "'s turn");
        }
    }
     
    // Returns true if the beast is in the front row
    bool inFront()
    {
        for (int x = 0; x < slots.Count; x++)
        {
            if (x < 3 && (currentTurn.Equals(slots[x])))
            {
                return true;
            }
        }

        return false;
    }

    // Returns the slot that the current attacker is in
    GameObject getSlot()
    {
        for (int x = 0; x < slots.Count; x++)
        {
            if (slots[x] != null && currentTurn.name == slots[x].name)
            {
                return slotImgs[x].gameObject;
            }
        }

        return null;
    }

    // Wait for the animation to finish then go back to Idle animation
    public IEnumerator AnimationWaitTime(UnityArmatureComponent beast)
    {
        attackingAnim = true;
        yield return new WaitUntil(() => beast.animation.isCompleted);
        attackingAnim = false;
        beast.animation.Play("Idle", 0);
    }
}
