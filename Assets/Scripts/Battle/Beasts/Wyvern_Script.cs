using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wyvern_Script : MonoBehaviour, Parent_Beast
{
    BattleManager battleManager;
    Attack attack;
    [SerializeField] GameObject backPrefab;
    [SerializeField] AudioClip frontAttackSound, backAttackSound, damageSound, deathSound;
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

        audioSrc = GetComponent<AudioSource>();
    }

    public void back_special()
    {
        
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

        Play_SoundFX("back");
    }

    public void front_special()
    {

        battleManager.targets.Clear();
        battleManager.targets = FindColumnTargets();
        battleManager.cancelGuard = true;
        battleManager.PlayDamagedAnimation(battleManager.targets);

        if (battleManager.roundOrderTypes[battleManager.turn] == "Player")
        {
            attack.InitiateAttack(battleManager.currentTurn, battleManager.targets, battleManager.inFront(), Player.summoner);
        }
        else
        {
            attack.InitiateAttack(battleManager.currentTurn, battleManager.targets, battleManager.inFront(), battleManager.enemySummoner);
        }

        Play_SoundFX("front");
    }

    void ProjectileAnimation()
    {
        GameObject target = battleManager.getSlot(battleManager.targets[0]);

        GameObject movePrefab = Instantiate(backPrefab);
        movePrefab.transform.SetParent(target.transform);
        movePrefab.transform.localPosition = new Vector3(0, 0);
        movePrefab.transform.localRotation = Quaternion.identity;
        movePrefab.transform.localScale = new Vector3(50, 50);
    }

    //this method finds which targets are availible to a beast using a stab attack
    //it takes into consideration the position of the attacker and the availible targets 
    public List<Beast> FindColumnTargets()
    {
        int slot = battleManager.getCurrentBeastSlot();
        List<Beast> slots = battleManager.slots;
        List<Beast> enemySlots = battleManager.enemySlots;
        List<Beast> targets = battleManager.targets;

        if (battleManager.roundOrderTypes[battleManager.turn] == "Enemy")
        {
            slot += Values.SMALLSLOT / 2;
            //this switch finds the column the attacker is in and find the most sutible target column determined by distance
            //the aligned cloumn is always prioritised 
            switch (slot % (Values.SMALLSLOT / 2))
            {
                case 0:
                    do
                    {
                        if (slots[slot % (Values.SMALLSLOT / 2)] != null && slots[slot % (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(slots[slot % (Values.SMALLSLOT / 2)]);
                            print(slots[slot % (Values.SMALLSLOT / 2)]);
                        }
                        if (slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)] != null && slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)]);
                            print(slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)]);
                        }
                        if (targets.Count < 1 && slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))] != null && slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].speed > 0 && slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].hitPoints > 0)
                        {
                            targets.Add(slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))]);
                        }
                        slot++;
                    } while (targets.Count < 1);
                    break;
                case 1:
                    do
                    {
                        if (slots[slot % (Values.SMALLSLOT / 2)] != null && slots[slot % (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(slots[slot % (Values.SMALLSLOT / 2)]);
                            print(slots[slot % (Values.SMALLSLOT / 2)]);
                        }
                        if (slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)] != null && slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)]);
                            print(slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)]);
                        }
                        if (targets.Count < 1 && slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))] != null && slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].speed > 0 && slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].hitPoints > 0)
                        {
                            targets.Add(slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))]);
                        }
                        slot++;
                    } while (targets.Count < 1);
                    break;
                case 2:
                    do
                    {
                        if (slots[slot % (Values.SMALLSLOT / 2)] != null && slots[slot % (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(slots[slot % (Values.SMALLSLOT / 2)]);
                            print(slots[slot % (Values.SMALLSLOT / 2)]);
                        }
                        if (slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)] != null && slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)]);
                            print(slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)]);
                        }
                        if (targets.Count < 1 && slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))] != null && slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].speed > 0 && slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].hitPoints > 0)
                        {
                            targets.Add(slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))]);
                        }
                        slot++;
                    } while (targets.Count < 1);
                    break;
                case 3:
                    do
                    {
                        if (slots[slot % (Values.SMALLSLOT / 2)] != null && slots[slot % (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(slots[slot % (Values.SMALLSLOT / 2)]);
                            print(slots[slot % (Values.SMALLSLOT / 2)]);
                        }
                        if (slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)] != null && slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)]);
                            print(slots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)]);
                        }
                        if (targets.Count < 1 && slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))] != null && slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].speed > 0 && slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].hitPoints > 0)
                        {
                            targets.Add(slots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))]);
                        }
                        slot--;
                    } while (targets.Count < 1);
                    break;
            }
            while (targets.Count < 1)
            {
                slot++;
                if (slots[Values.SMALLSLOT + (slot % Values.SMALLSLOT / 2)] != null && slots[Values.SMALLSLOT + (slot % Values.SMALLSLOT / 2)].speed > 0 && slots[Values.SMALLSLOT + (slot % Values.SMALLSLOT / 2)].hitPoints > 0)
                {
                    targets.Add(slots[Values.SMALLSLOT + (slot % Values.SMALLSLOT / 2)]);
                }
            }
        }
        else
        {
            slot += Values.SMALLSLOT / 2;
            switch (slot % (Values.SMALLSLOT / 2))
            {
                case 0:

                    do
                    {
                        if (enemySlots[slot % (Values.SMALLSLOT / 2)] != null && enemySlots[slot % (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(enemySlots[slot % (Values.SMALLSLOT / 2)]);
                        }
                        if (enemySlots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)] != null && enemySlots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(enemySlots[slot % (Values.SMALLSLOT / 2) + (Values.SMALLSLOT / 2)]);
                        }
                        if (targets.Count < 1 && enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))] != null && enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].speed > 0 && enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].hitPoints > 0)
                        {
                            print(enemySlots.Count);
                            targets.Add(enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))]);
                        }
                        slot++;
                    } while (targets.Count < 1);
                    break;
                case 1:
                    do
                    {
                        if (enemySlots[slot % (Values.SMALLSLOT / 2)] != null && enemySlots[slot % (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(enemySlots[slot % (Values.SMALLSLOT / 2)]);
                        }
                        if (enemySlots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)] != null && enemySlots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(enemySlots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)]);
                        }
                        if (targets.Count < 1 && enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))] != null && enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].speed > 0 && enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].hitPoints > 0)
                        {
                            print(enemySlots.Count);
                            targets.Add(enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))]);
                        }
                        slot++;
                    } while (targets.Count < 1);
                    break;
                case 2:
                    do
                    {
                        if (enemySlots[slot % (Values.SMALLSLOT / 2)] != null && enemySlots[slot % (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(enemySlots[slot % (Values.SMALLSLOT / 2)]);
                        }
                        if (enemySlots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)] != null && enemySlots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(enemySlots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)]);
                        }
                        if (targets.Count < 1 && enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))] != null && enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].speed > 0 && enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].hitPoints > 0)
                        {
                            print(enemySlots.Count);
                            targets.Add(enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))]);
                        }
                        slot--;
                    } while (targets.Count < 1);
                    break;
                case 3:
                    do
                    {
                        if (enemySlots[slot % (Values.SMALLSLOT / 2)] != null && enemySlots[slot % (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(enemySlots[slot % (Values.SMALLSLOT / 2)]);
                        }
                        if (enemySlots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)] != null && enemySlots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)].hitPoints > 0)
                        {
                            targets.Add(enemySlots[(slot % (Values.SMALLSLOT / 2)) + (Values.SMALLSLOT / 2)]);
                        }
                        if (targets.Count < 1 && enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))] != null && enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].speed > 0 && enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))].hitPoints > 0)
                        {
                            print(enemySlots.Count);
                            targets.Add(enemySlots[Values.SMALLSLOT + (slot % (Values.SLOTMAX - Values.SMALLSLOT))]);
                        }
                        slot--;
                    } while (targets.Count < 1);
                    break;
            }
            while (targets.Count < 1)
            {
                slot++;
                if (enemySlots[Values.SMALLSLOT + (slot % Values.SMALLSLOT / 2)] != null && enemySlots[Values.SMALLSLOT + (slot % Values.SMALLSLOT / 2)].speed > 0 && enemySlots[Values.SMALLSLOT + (slot % Values.SMALLSLOT / 2)].hitPoints > 0)
                {
                    targets.Add(enemySlots[Values.SMALLSLOT + (slot % Values.SMALLSLOT / 2)]);
                }
            }
        }
        return targets;
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
