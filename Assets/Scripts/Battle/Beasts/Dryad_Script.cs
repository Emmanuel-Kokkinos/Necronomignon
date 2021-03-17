using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dryad_Script : Parent_Script, Parent_Beast
{
    HealthManager healthManager;

    void Start()
    {
        base.start();
        GameObject g = GameObject.Find("GameManager");

        if (g != null)
        {
            healthManager = g.GetComponent<HealthManager>();
        }
    }

    public void back_special()
    {
        battleManager.targets.Clear();
        battleManager.targets.Add(getWeakestFriend());
        battleManager.cancelGuard = true;

        healthManager.heal(battleManager.targets[0], (int)(battleManager.targets[0].maxHP * ((double)battleManager.currentTurn.Move_A.power / 100)));

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

    //looks for which friendly unit has the lowest amount of health proportional to their max health
    Beast getWeakestFriend()
    {
        Beast b = battleManager.currentTurn;
        List<Beast> players = battleManager.players;
        List<bool> playersActive = battleManager.playersActive;
        List<Beast> enemies = battleManager.enemies;
        List<bool> enemiesActive = battleManager.enemiesActive;

        if (battleManager.roundOrderTypes[battleManager.turn] == "Player")
        {
            for (int x = 0; x < Values.SQUADMAX; x++)
            {
                //checks each friendly beasts proportional health remaining and campares it to who ever had the the prieviously lowest proportional health
                //defaults on the healer
                if (players[x] != null && b != null && playersActive[x] && ((double)players[x].hitPoints / (double)players[x].maxHP) < ((double)b.hitPoints / (double)b.maxHP))
                {
                    b = players[x];
                }
            }
        }
        else
        {
            for (int x = 0; x < Values.SQUADMAX; x++)
            {
                if (enemies[x] != null && b != null && enemiesActive[x] && ((double)enemies[x].hitPoints / (double)enemies[x].maxHP) < ((double)b.hitPoints / (double)b.maxHP))
                {
                    b = enemies[x];
                }
            }
        }
        return b;
    }

    public int HealAmount(Beast attacker, Beast target)
    {
        return (int)(target.maxHP * ((double)attacker.Move_A.power / 100));
    }

    public void Play_SoundFX()
    {
        throw new System.NotImplementedException();
    }
}
