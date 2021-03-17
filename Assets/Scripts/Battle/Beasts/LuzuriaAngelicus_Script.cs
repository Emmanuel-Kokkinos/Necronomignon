using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class LuzuriaAngelicus_Script : Parent_Script, Parent_Beast
{
    [SerializeField] GameObject backPrefab;
     
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
        endOfAttack();
    }

    public void front_special() 
    {
        battleManager.targets.Clear();
        battleManager.targets = findRowTargets();
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
        endOfAttack();
    }

    void ProjectileAnimation()
    {
        GameObject player = battleManager.getSlot(battleManager.currentTurn);
        GameObject target = battleManager.getSlot(battleManager.targets[0]);

        GameObject movePrefab = Instantiate(backPrefab);
        movePrefab.transform.SetParent(target.transform);
        movePrefab.transform.localPosition = new Vector3(0, 0);
        movePrefab.transform.localRotation = Quaternion.identity;
        movePrefab.transform.localScale = new Vector3(20, 20);

        
    }

    //changes targets to the front row (if there are front row targets) or back row(if there are no front row targets)
    //it takes into consideration the position of the attacker and the availible targets 
    List<Beast> findRowTargets()
    {
        List<Beast> slots = battleManager.slots;
        List<Beast> enemySlots = battleManager.enemySlots;
        List<Beast> targets = battleManager.targets;

        int slot = battleManager.getCurrentBeastSlot();
        if (battleManager.roundOrderTypes[battleManager.turn] == "Player")
        {
            //this for loop will aquier upto three targets from a single row
            for (int x = 0; x < enemySlots.Count; x++)
            {
                //this if statment tries to get all three beasts in the front row
                if (x < (Values.SMALLSLOT / 2) && enemySlots[x] != null && enemySlots[x].hitPoints > 0)
                {
                    if (slot + 1 == x || slot == x || slot - 1 == x)
                    {
                        targets.Add(enemySlots[x]);
                    }
                }

                else if (x == Values.SMALLSLOT / 2 && targets.Count < 1)
                {
                    for (int y = 0; y < Values.SMALLSLOT / 2; y++)
                    {
                        if (enemySlots[y] != null && enemySlots[y].hitPoints > 0)
                        {
                            targets.Add(enemySlots[y]);
                        }
                    }
                }
                //this else if checks to see if any targets from the front row have been added and if so
                //breaks the loop, if not adds the beasts from the back row

                if (x >= (Values.SMALLSLOT / 2) && enemySlots[x] != null && enemySlots[x].hitPoints > 0)
                {
                    print("help");
                    //this is the dynamic if to check for beasts in the front, I had to have it work dynamically or else it would never work
                    if (x == (Values.SMALLSLOT / 2) && targets.Count >= 1)
                    {
                        break;
                    }


                    if (slot % (Values.SMALLSLOT / 2) + 1 == x % (Values.SMALLSLOT / 2) || slot % (Values.SMALLSLOT / 2) == x % (Values.SMALLSLOT / 2) || slot % (Values.SMALLSLOT / 2) - 1 == x % (Values.SMALLSLOT / 2))
                    {
                        print("slot " + x);
                        targets.Add(enemySlots[x]);
                    }
                }
            }
            //this is to cover situations that would normally have no availible target
            if (targets.Count <= 0)
            {
                for (int x = 0; x < enemySlots.Count; x++)
                {
                    if (x < (Values.SMALLSLOT / 2) && enemySlots[x] != null && enemySlots[x].hitPoints > 0)
                    {
                        targets.Add(enemySlots[x]);
                    }
                    else if (x >= (Values.SMALLSLOT / 2) && enemySlots[x] != null && enemySlots[x].hitPoints > 0)

                    {
                        if (targets.Count > 0)
                        {
                            break;
                        }
                        targets.Add(enemySlots[x]);

                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < slots.Count; x++)
            {
                if (x < (Values.SMALLSLOT / 2) && slots[x] != null && slots[x].hitPoints > 0)
                {
                    if (slot + 1 == x || slot == x || slot - 1 == x)
                    {
                        targets.Add(slots[x]);
                    }
                }
                else if (x == Values.SMALLSLOT / 2 && targets.Count < 1)
                {
                    for (int y = 0; y < Values.SMALLSLOT / 2; y++)
                    {
                        if (slots[y] != null && slots[y].hitPoints > 0)
                        {
                            targets.Add(slots[y]);
                        }
                    }
                }
                if (x >= (Values.SMALLSLOT / 2) && slots[x] != null && slots[x].hitPoints > 0)
                {
                    print(targets.Count);
                    if (targets.Count - (x - (Values.SMALLSLOT / 2)) >= 1)
                    {
                        print("broken");
                        break;
                    }
                    if (slot + 1 == x || slot == x || slot - 1 == x)
                    {
                        targets.Add(slots[x]);
                    }
                }
            }
            if (targets.Count <= 0)
            {
                for (int x = 0; x < Values.SMALLSLOT / 2; x++)
                {
                    if (slots[x] != null && slots[x].hitPoints > 0)
                    {
                        targets.Add(slots[x]);
                    }
                }
                if (targets.Count <= 0)
                {
                    for (int x = 0; x < slots.Count; x++)
                    {
                        if (x < (Values.SMALLSLOT / 2) && slots[x] != null && slots[x].hitPoints > 0)
                        {
                            targets.Add(slots[x]);
                        }
                        else if (x >= (Values.SMALLSLOT / 2) && slots[x] != null && slots[x].hitPoints > 0)

                        {
                            if (targets.Count > 0)
                            {
                                break;
                            }
                            targets.Add(slots[x]);
                        }
                    }
                }
            }
        }
        return targets;
    }

    public void Play_SoundFX()
    {
        throw new NotImplementedException();
    }
}
