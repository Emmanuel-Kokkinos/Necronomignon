
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionList : MonoBehaviour
{
    public string mission; 

    public BeastManager beastManager;

    public List<Beast> enemies = new List<Beast>();

    public Summoner summoner;

    public int totalEnemies = 11;

    void Start()
    {
        mission = LevelChecker.lastClick;
    }
    //gets the beasts coresponding to the mission
    private void Awake()
    {
        mission = LevelChecker.lastClick;
        summoner = new Summoner();

        MissionCheck();
        TourMission();
        RedRoach();

    }
    
    /*
     * Determine the fights of the tournament
     */
    public void TourMission()
    {
        /*
        First Battle is lazyly put together (John the shitty Neo Carcosan)
        2 fight should be against Demon Chick
        Third fight, finals is against Gabriel
        2nd fight should be decent team, tiers 3 and 4s
        third fight all tier 4s and 5s and well thought out combination
        */
        if (mission == "John")
        {
            Beast kitsune = beastManager.getFromName("Kitsune");
            enemies.Add(beastManager.getFromName("Zeograth"));

            int ran = -1;
            //ran = Random.Range(-1, Values.SMALLSLOT - 1);
            ran = (kitsune.size == 0) ? Random.Range(-1, Values.SMALLSLOT - 1) : Random.Range(Values.SMALLSLOT, totalEnemies - 1);

            while (ran >= 0)
            {
                enemies.Add(null);
                ran--;
            }

            enemies.Add(kitsune);
            

            while (enemies.Count < totalEnemies)
            {
                enemies.Add(null);
            }

            foreach (Beast b in enemies)
            {
                if (b != null)
                {
                    b.setTierUpper(2);
                }
            }

            summoner.xp = 15;
        }
        else if(mission == "DemonChick")
        {
            Beast b = new Beast();
            b = beastManager.getFromName("Conglomerate");
            b.power = 2;
            enemies.Add(b);
            enemies.Add(null);
            enemies.Add(null);
            b = beastManager.getFromName("Wyvern");
            b.defence = 30;
            enemies.Add(b);
            enemies.Add(beastManager.getFromName("FallborneDragon"));
            enemies.Add(beastManager.getFromName("DreamSlime"));
            enemies.Add(null);
            enemies.Add(beastManager.getFromName("Dryad"));
            enemies.Add(null);
            enemies.Add(null);
            enemies.Add(null);

            foreach (Beast be in enemies)
            {
                if (be != null)
                {
                    be.setTierUpper(3);
                }
            }

            summoner.xp = 70;
        }
        else if(mission == "Gabriel")
        {
            Beast b = new Beast();

            b = beastManager.getFromName("Luzuria");
            b.defence = 25;

            enemies.Add(b);
            enemies.Add(null);
            enemies.Add(BeastManager.getFromNameS("FallborneDragon"));
            enemies.Add(BeastManager.getFromNameS("Mandoro"));
            enemies.Add(beastManager.getFromName("Dryad"));
            enemies.Add(null);
            enemies.Add(null);
            b = beastManager.getFromName("Cthulhu");
            b.power = 3;
            enemies.Add(b);
            enemies.Add(null);
            enemies.Add(null);
            enemies.Add(null);
            foreach (Beast be in enemies)
            {
                if (be != null)
                {
                    be.setTierUpper(5);
                }
            }
            summoner.xp = 120;
        }
        else if (mission == "Boss")
        {

        }
    }
    public void RedRoach()
    {
        if (Player.RedRoach)
        {
            //summoner.xp *= 3;
            summoner.xp = (int)Mathf.Pow(summoner.xp, 1.7f);
            foreach (Beast be in enemies)
            {
                if (be != null)
                {
                    be.setTierLower(5);
                }
            }
        }
    }
    public void MissionCheck()
    {

        // Kitsune in a random position
        if (mission == "first")
        {
            Beast kitsune = beastManager.getFromName("Kitsune");

            int ran = -1;
            //ran = Random.Range(-1, Values.SMALLSLOT - 1);
            ran = (kitsune.size == 0) ? Random.Range(-1, Values.SMALLSLOT - 1) : Random.Range(Values.SMALLSLOT, totalEnemies - 1);

            while (ran >= 0)
            {
                enemies.Add(null);
                ran--;
            }

            enemies.Add(kitsune);

            while (enemies.Count < totalEnemies)
            {
                enemies.Add(null);
            }

            foreach (Beast b in enemies)
            {
                if (b != null)
                {
                    b.setTierUpper(2);
                }
            }

            summoner.xp = 2;
        }

        // Conglomerate back row, Dryad front row
        if (mission == "second")
        {
            int ran = -1;
            ran = Random.Range(-1, 3);

            while (ran >= 0)
            {
                enemies.Add(null);
                ran--;
            }

            enemies.Add(beastManager.getFromName("Dryad"));

            while (enemies.Count < 4)
            {
                enemies.Add(null);
            }

            ran = -1;
            ran = Random.Range(-1, 3);

            while (ran >= 0)
            {
                enemies.Add(null);
                ran--;
            }

            enemies.Add(beastManager.getFromName("Conglomerate"));

            while (enemies.Count < totalEnemies)
            {
                enemies.Add(null);
            }
            foreach (Beast b in enemies)
            {
                if (b != null)
                {
                    b.setTierUpper(3);
                }
            }
            summoner.xp = 25;
        }

        // Dryad and Wyvern front row, Conglomerate back row
        if (mission == "third")
        {
            int poN = -1;
            int poD = -1;
            int poW = -1;
            poN = Random.Range(0, Values.SMALLSLOT / 2);

            while (poD == -1 || poD == poN)
            {
                poD = Random.Range(0, Values.SMALLSLOT / 2);
            }

            while (poW == -1 || poW == poN || poW == poD)
            {
                poW = Random.Range(0, Values.SMALLSLOT / 2);
            }

            for (int x = 0; x < Values.SMALLSLOT / 2; x++)
            {
                if (x == poD)
                {
                    enemies.Add(beastManager.getFromName("Dryad"));
                }
                else if (x == poW)
                {
                    enemies.Add(beastManager.getFromName("Wyvern"));
                }
                else
                {
                    enemies.Add(null);
                }
            }

            int ran = Random.Range(-1, 3);

            while (ran >= 0)
            {
                enemies.Add(null);
                ran--;
            }

            enemies.Add(beastManager.getFromName("Conglomerate"));

            while (enemies.Count < totalEnemies)
            {
                enemies.Add(null);
            }
            foreach (Beast b in enemies)
            {
                if (b != null)
                {
                    b.setTierUpper(3);
                }
            }
            summoner.xp = 50;
        }

        // High defence small beast in the front blocking Dryad, DreamSlime in the front blocking a random small beast with low damage
        if (mission == "fourth")
        {
            Beast b = new Beast();
            int i = -1;

            while (b.defence < 24 || b.size == 1)
            {
                i = Random.Range(0, BeastManager.beastsList.Beasts.Count);
                b = BeastManager.getFromNameS(BeastManager.beastsList.Beasts[i].name);
            }

            enemies.Add(b);
            enemies.Add(null);
            enemies.Add(null);
            enemies.Add(BeastManager.getFromNameS("DreamSlime"));
            enemies.Add(BeastManager.getFromNameS("Dryad"));
            enemies.Add(null);
            enemies.Add(null);
            b = new Beast();

            do
            {
                i = Random.Range(0, BeastManager.beastsList.Beasts.Count);
                b = BeastManager.getFromNameS(BeastManager.beastsList.Beasts[i].name);
                if (enemies[0].name == b.name)
                {
                    print("Scenario apocalypse");
                    b.power = 1;
                }
            }
            while (b.power < 10 || b.name == "DreamSlime" || b.size == 1 || enemies.Contains(b));

            enemies.Add(b);
            enemies.Add(null);
            enemies.Add(null);
            enemies.Add(null);
            foreach (Beast be in enemies)
            {
                if (be != null)
                {
                    be.setTierUpper(4);
                }
            }
            summoner.xp = 100;
        }

        // Hardcoded places
        if (mission == "sample")
        {
            enemies.Add(beastManager.getFromName("Dryad"));
            enemies.Add(beastManager.getFromName("Conglomerate"));
            enemies.Add(beastManager.getFromName("Wyvern"));
            enemies.Add(null);
            enemies.Add(null);
            enemies.Add(null);
            enemies.Add(null);
            enemies.Add(beastManager.getFromName("Kitsune"));
            enemies.Add(null);
            enemies.Add(null);
            enemies.Add(null);
        }

        // the mission with 4 randomly placed, random small beasts
        if (mission == "random")
        {
            List<int> beast = new List<int>();
            int beastCost = 0;
            int ran = -1;

            while (beastCost < Values.TOTAL_BEAST_COST - Values.BEAST_COST_MIN)
            {
                //randomly picks a beast based on it's number in the list
                while (beast.Contains(ran) || ran == -1)
                {
                    ran = Random.Range(0, BeastManager.beastsList.Beasts.Count);
                }

                if (BeastManager.beastsList.Beasts[ran].tier != -2)
                {
                    beastCost += BeastManager.beastsList.Beasts[ran].cost;
                    beast.Add(ran);
                }
                else
                {
                    ran = -1;
                }
            }

            while (beast.Count < Values.SQUADMAX)
            {
                beast.Add(-1);
            }

            ran = -1;

            List<int> position = new List<int>();

            while (enemies.Count < Values.SMALLSLOT)
            {
                //loops random numbers that will go on to assign a slot to each beast
                while (position.Contains(ran) || ran == -1)
                {
                    ran = Random.Range(0, Values.SMALLSLOT);
                }

                position.Add(ran);

                if (beast[ran] < 0)
                {
                    enemies.Add(null);
                }
                else
                {
                    enemies.Add(beastManager.getFromName(BeastManager.beastsList.Beasts[beast[ran]].name));
                }
            }
            foreach (Beast b in enemies)
            {
                if (b != null)
                {
                    b.setTierLower(2);
                }
            }
            enemies.Add(null);
            enemies.Add(null);
            enemies.Add(null);
            summoner.xp = 150;
        }

        //the mission with 1-4 random beast with random placement 
        if (mission == "randomer")
        {
            List<int> beast = new List<int>();

            int ran = -1;

            int godhimself = Random.Range(1, Values.SQUADMAX + 1);

            while (beast.Count < godhimself)
            {
                while (beast.Contains(ran) || ran == -1)
                {
                    ran = Random.Range(0, BeastManager.beastsList.Beasts.Count);
                }
                if (BeastManager.beastsList.Beasts[ran].tier != -2)
                {
                    beast.Add(ran);
                }
                else
                {
                    ran = -1;
                }

            }
            while (beast.Count < totalEnemies)
            {
                beast.Add(-1);
            }

            ran = -1;

            List<int> position = new List<int>();

            while (enemies.Count < Values.SMALLSLOT)
            {
                while (position.Contains(ran) || ran == -1)
                {
                    ran = Random.Range(0, totalEnemies);
                }
                position.Add(ran);
                if (beast[ran] < 0)
                {
                    enemies.Add(null);
                }
                else
                {
                    enemies.Add(beastManager.getFromName(BeastManager.beastsList.Beasts[beast[ran]].name));
                }
            }
            foreach (Beast b in enemies)
            {
                if (b != null)
                {
                    b.setTierUpper(5);
                }
            }
            while (enemies.Count < totalEnemies)
            {
                enemies.Add(null);
            }
            summoner.xp = 250;
        }

        if (mission == "bottom")
        {
            List<int> position = new List<int>();
            int ran = -1;
            while (position.Count < Values.SMALLSLOT)
            {
                while (position.Contains(ran) || ran == -1)
                {
                    ran = Random.Range(0, Values.SMALLSLOT);
                }
                position.Add(ran);

            }
            ran = Random.Range(0, 4);
            List<string> beasts = new List<string>();
            Beast b = new Beast();

            while (beasts.Count < 4)
            {
                switch (ran)
                {
                    case 0:
                        for (int x = 0; x < BeastManager.beastsList.Beasts.Count; x++)
                        {
                            if ((b.maxHP > BeastManager.beastsList.Beasts[x].maxHP || b.name == "") && !beasts.Contains(BeastManager.beastsList.Beasts[x].name))
                            {
                                if (BeastManager.beastsList.Beasts[x].tier != -2)
                                {
                                    b = BeastManager.beastsList.Beasts[x];
                                }
                            }
                        }
                        beasts.Add(b.name);
                        break;
                    case 1:
                        for (int x = 0; x < BeastManager.beastsList.Beasts.Count; x++)
                        {
                            if ((b.power > BeastManager.beastsList.Beasts[x].power || b.name == "") && !beasts.Contains(BeastManager.beastsList.Beasts[x].name))
                            {

                                if (BeastManager.beastsList.Beasts[x].tier != -2)
                                {
                                    b = BeastManager.beastsList.Beasts[x];
                                }

                            }
                        }
                        print(b.name);
                        beasts.Add(b.name);
                        break;
                    case 2:
                        for (int x = 0; x < BeastManager.beastsList.Beasts.Count; x++)
                        {
                            if ((b.defence > BeastManager.beastsList.Beasts[x].defence || b.name == "") && !beasts.Contains(BeastManager.beastsList.Beasts[x].name))
                            {
                                if (BeastManager.beastsList.Beasts[x].tier != -2)
                                {
                                    b = BeastManager.beastsList.Beasts[x];
                                }
                            }
                        }
                        print(b.name);
                        beasts.Add(b.name);
                        break;
                    case 3:
                        for (int x = 0; x < BeastManager.beastsList.Beasts.Count; x++)
                        {
                            if ((b.speed > BeastManager.beastsList.Beasts[x].speed || b.name == "") && !beasts.Contains(BeastManager.beastsList.Beasts[x].name))
                            {
                                if (BeastManager.beastsList.Beasts[x].tier != -2)
                                {
                                    b = BeastManager.beastsList.Beasts[x];
                                }
                            }
                        }
                        print(b.name);
                        beasts.Add(b.name);
                        break;
                }

                b = new Beast();
            }
            beasts.Add(null);
            beasts.Add(null);
            beasts.Add(null);
            beasts.Add(null);
            foreach (string str in beasts)
            {
                print(str);
            }
            for (int x = 0; x < beasts.Count; x++)
            {
                if (beasts[position[x]] == null || beasts[position[x]] == "")
                {
                    enemies.Add(null);
                }
                else
                {
                    print(beasts[position[x]]);
                    enemies.Add(BeastManager.getFromNameS(beasts[position[x]]));
                }
            }
            enemies.Add(null);
            enemies.Add(null);
            enemies.Add(null);
            foreach (Beast be in enemies)
            {
                if (be != null)
                {
                    be.setTierLower(2);
                }
            }
            summoner.xp = 400;
        }
        if (mission == "top")
        {
            List<int> position = new List<int>();
            int ran = -1;
            while (position.Count < Values.SMALLSLOT)
            {
                while (position.Contains(ran) || ran == -1)
                {
                    ran = Random.Range(0, Values.SMALLSLOT);
                }
                position.Add(ran);

            }
            ran = Random.Range(0, 4);
            List<string> beasts = new List<string>();
            Beast b = new Beast();
            while (beasts.Count < 4)
            {
                print(ran);
                if (ran == 0)
                {
                    for (int x = 0; x < BeastManager.beastsList.Beasts.Count; x++)
                    {
                        if ((b.maxHP < BeastManager.beastsList.Beasts[x].maxHP || b.name == "") && !beasts.Contains(BeastManager.beastsList.Beasts[x].name))
                        {
                            if (BeastManager.beastsList.Beasts[x].tier != -2)
                            {
                                b = BeastManager.beastsList.Beasts[x];
                            }
                        }
                    }
                    print(b.name);
                    beasts.Add(b.name);
                }
                else if (ran == 1)
                {
                    for (int x = 0; x < BeastManager.beastsList.Beasts.Count; x++)
                    {
                        if ((b.power < BeastManager.beastsList.Beasts[x].power || b.name == "") && !beasts.Contains(BeastManager.beastsList.Beasts[x].name))
                        {

                            if (BeastManager.beastsList.Beasts[x].tier != -2)
                            {
                                b = BeastManager.beastsList.Beasts[x];
                            }

                        }
                    }
                    print(b.name);
                    beasts.Add(b.name);
                }
                else if (ran == 2)
                {
                    for (int x = 0; x < BeastManager.beastsList.Beasts.Count; x++)
                    {
                        if ((b.defence < BeastManager.beastsList.Beasts[x].defence || b.name == "") && !beasts.Contains(BeastManager.beastsList.Beasts[x].name))
                        {
                            if (BeastManager.beastsList.Beasts[x].tier != -2)
                            {
                                b = BeastManager.beastsList.Beasts[x];
                            }
                        }
                    }
                    print(b.name);
                    beasts.Add(b.name);
                }
                else if (ran == 3)
                {
                    for (int x = 0; x < BeastManager.beastsList.Beasts.Count; x++)
                    {
                        if ((b.speed < BeastManager.beastsList.Beasts[x].speed || b.name == "") && !beasts.Contains(BeastManager.beastsList.Beasts[x].name))
                        {
                            if (BeastManager.beastsList.Beasts[x].tier != -2)
                            {
                                b = BeastManager.beastsList.Beasts[x];
                            }
                        }
                    }
                    print(b.name);
                    beasts.Add(b.name);

                }
                b = new Beast();
            }
            beasts.Add(null);
            beasts.Add(null);
            beasts.Add(null);
            beasts.Add(null);
            for (int x = 0; x < beasts.Count; x++)
            {
                if (beasts[position[x]] == null || beasts[position[x]] == "")
                {
                    enemies.Add(null);
                }
                else
                {
                    print(beasts[position[x]]);
                    enemies.Add(BeastManager.getFromNameS(beasts[position[x]]));
                }
            }
            foreach (Beast be in enemies)
            {
                if (be != null)
                {
                    be.setTierLower(2);
                }
            }
            enemies.Add(null);
            enemies.Add(null);
            enemies.Add(null);
            summoner.xp = 800;
        }
        else if (mission == "boss")
        {
            while (enemies.Count < Values.SMALLSLOT)
            {
                enemies.Add(null);
            }
            int ran = Random.Range(0, BeastManager.beastsList.Beasts.Count);
            Beast b = BeastManager.getFromNameS(BeastManager.beastsList.Beasts[ran].name);
            while (b.tier == -2)
            {
                ran = Random.Range(0, BeastManager.beastsList.Beasts.Count);
                b = BeastManager.getFromNameS(BeastManager.beastsList.Beasts[ran].name);
            }
            b.size = 1;
            b.setTierLower(4);
            b.maxHP *= 6;
            b.power *= 6;
            b.defence *= 6;
            enemies.Add(null);
            enemies.Add(b);
            enemies.Add(null);

            summoner.xp = 2000;
            //summoner.addXP(Player.summoner.xp*4);
        }
        else if (mission == "even")
        {
            List<int> beast = new List<int>();
            int beastCost = 0;
            int ran = -1;

            while (beastCost < Values.TOTAL_BEAST_COST)
            {
                //randomly picks a beast based on it's number in the list
                while (beast.Contains(ran) || ran == -1)
                {
                    ran = Random.Range(0, BeastManager.beastsList.Beasts.Count);
                }

                if (BeastManager.beastsList.Beasts[ran].tier != -2 && BeastManager.beastsList.Beasts[ran].size != 1)
                {
                    beastCost += BeastManager.beastsList.Beasts[ran].cost;
                    beast.Add(ran);
                }
                else
                {
                    ran = -1;
                }
                if (beastCost + Values.BEAST_COST_MIN > Values.TOTAL_BEAST_COST)
                {
                    break;
                }
            }

            while (beast.Count < Values.SQUADMAX)
            {
                beast.Add(-1);
            }

            ran = -1;

            List<int> position = new List<int>();

            while (enemies.Count < Values.SMALLSLOT)
            {
                //loops random numbers that will go on to assign a slot to each beast
                while (position.Contains(ran) || ran == -1)
                {
                    ran = Random.Range(0, Values.SMALLSLOT);
                }

                position.Add(ran);

                if (beast[ran] < 0)
                {
                    enemies.Add(null);
                }
                else
                {
                    enemies.Add(beastManager.getFromName(BeastManager.beastsList.Beasts[beast[ran]].name));
                }
            }
            foreach (Beast b in enemies)
            {
                if (b != null)
                {
                    b.setTierLower(3);
                }
            }
            enemies.Add(null);
            enemies.Add(null);
            enemies.Add(null);
            summoner.xp = Player.summoner.xp;
        }
    }
}
