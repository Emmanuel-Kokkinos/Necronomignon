using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
//organises and calculates damage on one or many beasts
public class Attack : MonoBehaviour
{
    public BeastManager beastManager;
    public DemoBattleManager demoBattleManager;
    public BattleManager battleManager;
    public HealthManager healthManager;

    public Summoner summoner;

    readonly UnityEngine.Random Random = new UnityEngine.Random();

    float modifier = 1;
    int damage;

    //takes all beasts in targets and checks and adds status effects, kills if target is too doomed
    public void InitiateAttack(Beast attacker, List<Beast> targets, bool inFront, Summoner summ)
    {
        summoner = summ;
        if (beastManager.moveManager.movesList == null)
        {
            beastManager.moveManager.start();
        }
        if (attacker.hitPoints <= 0 && targets[0].name != "Target")
        {
            battleManager.RemoveBeast(attacker);
            return;
        }
        //running the method for each beast
        foreach (Beast target in targets)
        {
            if (attacker != null && target != null && attacker.speed != 0 && target.speed != 0 && attacker.statusTurns[(int)Move.types.Sleep] <= 0 && target.hitPoints > 0)
            {
                if (targets[0].name != "Target")
                {
                    if (attacker.statusTurns[(int)Move.types.Paralyze] > 0)
                    {
                        int r = UnityEngine.Random.Range(0, 2);
                        if (r > 0)
                        {
                            healthManager.DisplayDamageOutput(attacker, "Paralyzed", new Color(255f / 255f, 255f / 255f, 0f / 255f));
                            print(attacker.name + " was paralized and unable to move");
                            return;
                        }
                    }

                    if (inFront)
                    {
                        if (attacker.Move_A.buff.isActive)
                        {
                            target.buffs.Add(new Buff(attacker.Move_A.buff));
                        }
                        if (attacker.Move_A.healing)
                        {
                            print(attacker.name + " has healed " + target.name);
                            break;
                        }
                    }
                    else if (!inFront)
                    {
                        if (attacker.Move_B.buff.isActive)
                        {
                            target.buffs.Add(new Buff(attacker.Move_B.buff));
                        }
                        if (attacker.Move_B.healing)
                        {
                            print(attacker.name + " has healed " + target.name);
                            break;
                        }
                    }

                    if (attacker.statusTurns[(int)Move.types.Blind] > 0)
                    {
                        healthManager.DisplayDamageOutput(attacker, "Blinded", new Color(255f / 255f, 255f / 255f, 255f / 255f));
                        print(attacker.name + " attacks blindly");
                    }
                }

                if (attacker.cursed == target && targets[0].name != "Target")
                {
                    if (attacker.curse(target))
                    {
                        healthManager.DisplayDamageOutput(target, "DOOM!", new Color(25f / 255f, 25f / 255f, 25f / 255f));
                        print("Doom has consumed " + target.name);
                        modifier = 1;
                        healthManager.UpdateHealth(target, target.maxHP);
                    }
                    else
                    {
                        healthManager.DisplayDamageOutput(target, "Doom lingers...", new Color(25f / 255f, 25f / 255f, 25f / 255f));
                        print("Doom lingers over " + target.name);
                        modifier = 1;
                    }
                }
                else if (!isMiss(attacker, target))
                {
                    //adjusts the modifier for a critical hit
                    modifier *= isCrit(attacker, target);
                    //adjusts the modifier if the attack is blocked
                    modifier *= isGuard(attacker, target);
                    //checks if a status effect can be added, or if there is enough corruption to kill the target
                    checkIfStatus(attacker, target, inFront);
                    CalculateDamage(attacker, target, inFront);
                }
            }
            else if (attacker.statusTurns[(int)Move.types.Sleep] > 0)
            {
                healthManager.DisplayDamageOutput(attacker, "Asleep", new Color(3f / 255f, 157f / 255f, 252f / 255f));
                print(attacker.name + " was asleep and unable to move");
            }

            modifier = 1;
        }
    }

    // Checks if the attack will miss
    private bool isMiss(Beast attacker, Beast target)
    {
        /*
         * Old version keeping for reference in the future and because Alex wants us to :)
         * 
        float missChance = ((float)attacker.dexterity) / (float)target.speed;

        //Get Random variable 
        int rand = Random.Range(1, 100);
        int rando = Random.Range(1, 20);

        //If random variable is less than the miss chance, then the attack misses
        if (rando <= 1 || rand >= missChance * 100)
        {
            Debug.Log("Attack Misses");
            return true;
        }
        else
        {
            Debug.Log(attacker.name + " attacked " + target.name);
            return false;
        }
        */
        if(attacker.statusTurns[(int)Move.types.Paralyze] > 0)
        {
            if (UnityEngine.Random.Range(0, 2) > 0)
            {
                healthManager.DisplayDamageOutput(attacker, "Paralyzed", new Color(255f / 255f, 255f / 255f, 0f / 255f));
                print(attacker.name + " was paralized and unable to move");
                return true;
            }
        }
        if (attacker.statusTurns[(int)Move.types.Sleep] > 0)
        {
            healthManager.DisplayDamageOutput(attacker, "Asleep", new Color(3f / 255f, 157f / 255f, 252f / 255f));
            print(attacker.name + " was asleep and unable to move");
            return true;
        }
        float poisonMod = 1;
        if (attacker.statusTurns[(int)Move.types.Poison] > 0)
        {
            poisonMod = .85f;
        }

        float missChance = 100;
        missChance += Mathf.Floor((attacker.dexterity* poisonMod) / 10);
        missChance -= Mathf.Floor(target.speed / 10);
        if (attacker.statusTurns[(int)Move.types.Blind] > 0)
        {
            missChance *= 1-(float)(attacker.statusTurns[(int)Move.types.Blind] * .2);
        }

        int rand = UnityEngine.Random.Range(1, 100);

        if(rand < 95)
        {
            print(attacker.name + " attacked " + target.name);
            return false;
        }
        else
        {
            healthManager.DisplayDamageOutput(target, "MISS!", Color.white);
            print("Attack Misses");
            return true;
        }
    }

    // Checks if the attack will be a critical hit
    private float isCrit(Beast attacker, Beast target)
    {
        //Calculate the chance that an attack is a critical hit
        //(d20roll) + ({Attacker.Speed/2} + Attacker.Skill)/({Target.Speed/2} + Target.Skill)
        int rand = UnityEngine.Random.Range(1, 20);
        float criticalChance = rand + (((attacker.speed / 2) + attacker.dexterity) / ((target.speed / 2) + target.dexterity));
        

        float ra = (float)(rand + target.defence / attacker.power);
        int critChance = (int)Mathf.Round(ra);

        //If random variable is more than critical hit chance, the attack has a modifier of 2, otherwise it's modifier is 1
        if (critChance >= 20)
        {
            healthManager.DisplayDamageOutput(target, "CRIT!", Color.white);
            Debug.Log("Critical Hit!");

            return 2;
        }
        return 1;
    }

    // Checks if the defender will guard the attack
    private float isGuard(Beast attacker, Beast target)
    {
        //Calculate the chance that an attack is blocked
        //(d10roll) + (TargetDefense/AttackerPower)

        int rand = UnityEngine.Random.Range(1, 10);

        float ra = (float)(rand + (((float)target.defence / (float)attacker.power) * 1));

        int blockChance = (int)Mathf.Round(ra);

        //Get new random variable

        if (blockChance >= 10)
        {
            float vary = 0.32f;

            int vary2 = UnityEngine.Random.Range(1, 33);

            vary += (float)vary2 / 100;

            healthManager.DisplayDamageOutput(target, "GUARD!", Color.white);
            Debug.Log("Attack is Blocked!");
            return vary;
        }

        return 1;
    }

    //calculates the chance of status effect and adds it if it works
    //also checks if the target is currupted enough to die
    void checkIfStatus(Beast attacker, Beast target, bool front)
    {
        if(attacker.cursed != null)
        {
            
            if (attacker.curse(target))
            {
                healthManager.UpdateHealth(target, target.hitPoints);
                return;
            }
        }
        float effectChance = 0;
        int type = 0;
        if (front)
        {
            effectChance = (float)attacker.Move_A.condition_chance * 100;
            type = (int)attacker.Move_A.type;
        }
        else
        {
            effectChance = (float)attacker.Move_B.condition_chance * 100;
            type = (int)attacker.Move_B.type;
        }

        int rand = UnityEngine.Random.Range(1, 100);

        if (rand < effectChance && type != (int)Move.types.Doom && type != (int)Move.types.Corrupt && target.statusTurns[type]<=0)
        {
            print("status effect on " + target.name);
            target.statusTurns[type] = 3;

            GameObject effectPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Move"));
            SetAnimatorController(effectPrefab, type);
            effectPrefab.GetComponent<Animator>().SetTrigger("effect");
            effectPrefab.transform.SetParent(battleManager.getSlot(target).transform);
            effectPrefab.transform.localPosition = new Vector3(0, 100);
            effectPrefab.transform.localRotation = Quaternion.identity;
            effectPrefab.transform.localScale = new Vector3(1, 1);
        }
        //this is where doom is cast, after this point doom is charged and completed in another place
        else if(rand < effectChance && type != (int)Move.types.Corrupt && type == (int)Move.types.Doom && target.statusTurns[type] <= 0)
        {
            print(target.name + " has been doomed");
            attacker.curse(target);
        }
        //here is where corruption is added and if neccisary, deleted 
        else if(rand < effectChance && type == (int)Move.types.Corrupt)
        {
            target.statusTurns[type]++;
            if(target.statusTurns[type] > 5)
            {
                healthManager.UpdateHealth(target, target.hitPoints);
            }
        }
    }

    void SetAnimatorController(GameObject effectPrefab, int type)
    {
        switch(type)
        {
            case 0:
                effectPrefab.GetComponent<Animator>().runtimeAnimatorController = 
                    Resources.Load("Beasts_Moves/statusEffects/sleep/sleep_controller") as RuntimeAnimatorController;
                break;
            case 1:
                effectPrefab.GetComponent<Animator>().runtimeAnimatorController = 
                    Resources.Load("Beasts_Moves/statusEffects/fire/fire_controller") as RuntimeAnimatorController;
                break;
            case 2:
                effectPrefab.GetComponent<Animator>().runtimeAnimatorController =
                    Resources.Load("Beasts_Moves/statusEffects/poison/poison_controller") as RuntimeAnimatorController;
                break;
            case 3:
                effectPrefab.GetComponent<Animator>().runtimeAnimatorController =
                    Resources.Load("Beasts_Moves/statusEffects/paralysis/paralysis_controller") as RuntimeAnimatorController;
                break;
            case 4:
                break;
            case 5:
                effectPrefab.GetComponent<Animator>().runtimeAnimatorController =
                    Resources.Load("Beasts_Moves/statusEffects/blind/blind_controller") as RuntimeAnimatorController;
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                effectPrefab.GetComponent<Animator>().runtimeAnimatorController =
                    Resources.Load("Beasts_Moves/statusEffects/stunned/stunned_controller") as RuntimeAnimatorController;
                break;
            default:
                break;
        }
    }

    // Calculates the damage taking the different multipliers into account
    void CalculateDamage(Beast attacker, Beast target, bool inFront)
    {
        //    Random Random = new Random();
        attacker.setAttacks();
        float dmg;
        float burnMod = 1;
        float poisonMod = 1;
        if (target.statusTurns[(int)Move.types.Burn] >0)
        {
            print("burn reduced " + target.name + "'s defence");
            burnMod = .8f;
        }
        if (attacker.statusTurns[(int)Move.types.Poison] > 0)
        {
            print("poison reduced " + attacker.name + "'s attack");
            poisonMod = .85f;
        }
        //Calculates the damage if the attacker is in row A

        //adjusts the modifier for each status effect on the attacker and target
        //this checks buffs and debuffs for the attackers power
        foreach(Buff bu in attacker.buffs)
        {
            if (!bu.defenceBuff)
            {
                if (bu.upBuff)
                {
                    modifier += modifier * bu.change;
                }
                else
                {
                    modifier *= 1- bu.change;
                }
            }
        }
        //this checks buff and debuff that affect the targets defence
        foreach(Buff bu in target.buffs)
        {
            if (bu.defenceBuff)
            {
                if (bu.upBuff)
                {
                    modifier *= bu.change;
                }
                else
                {
                    modifier = modifier * bu.change;
                }
            }
        }
        //alters the attack and defence based on poisen or burn
        if (inFront)
        {
            //dmg = (attacker.power* poisonMod) * attacker.Move_A.power / (target.defence*burnMod);
            dmg = (float)(((((float)attacker.power * (float)poisonMod) + (float)summoner.getLevel()) * (100 / (100 + ((float)target.defence * (float)burnMod))) * 2)*((float)attacker.Move_A.power / 50) + (Math.Pow((float)summoner.getLevel(),(float) 1.3) / 8));
            print(attacker.Move_A.name);
        }
        else
        {
            //dmg = (attacker.power * poisonMod) * attacker.Move_B.power / (target.defence * burnMod);
            dmg = (float)(((((float)attacker.power * (float)poisonMod) + (float)summoner.getLevel()) * (100 / (100 + ((float)target.defence * (float)burnMod))) * 2) * ((float)attacker.Move_B.power / 50) + (Math.Pow((float)summoner.getLevel(), (float)1.3) / 8));
            print(attacker.Move_B.name);
        }

        float vary = 0.89f;

        float vary2 = UnityEngine.Random.Range(1, 20);
        vary += vary2 / 100;
        print("Damage before types = " + (int)(dmg * vary * modifier));
        calculateType(attacker, target);

        damage = (int)(dmg * vary * modifier); //Convert damage to an integer
        if (damage < 1)
        {
            damage = 1;
        }
        Debug.Log("This is damage done " + damage);
        if (target.name == "Target")
        {
            demoBattleManager.totalDamage += damage;
        }
        else
        {
            healthManager.UpdateHealth(target, damage);
            Color type = GetTypeColor(attacker);
            healthManager.DisplayDamageOutput(target, damage.ToString(), type);
        }

        int rand = UnityEngine.Random.Range(0, 2);
        if (target.statusTurns[(int)Move.types.Sleep] > 0 && rand > 0 && rand<5)
        {
            print(target.name + " woke up");
            target.statusTurns[(int)Move.types.Sleep] = 0;
        }
    }

    //Change damage output color
    public Color GetTypeColor(Beast attacker)
    {
        Color color = Color.white;
        switch (attacker.type)
        {
            case Beast.types.Water:
                color = new Color(3f / 255f, 157f / 255f, 252f / 255f);
                break;
            case Beast.types.Fire:
                color = new Color(209f / 255f, 0f / 255f, 0f / 255f);
                break;
            case Beast.types.Earth:
                color = new Color(31f / 255f, 107f / 255f, 27f / 255f);
                break;
            case Beast.types.Air:
                color = new Color(255f / 255f, 255f / 255f, 0f / 255f);
                break;
            case Beast.types.Dark:
                color = new Color(52f / 255f, 7f / 255f, 120f / 255f);
                break;
            case Beast.types.Light:
                color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                break;
            case Beast.types.Horror:
                color = new Color(25f / 255f, 25f / 255f, 25f / 255f);
                break;
            case Beast.types.Cosmic:
                color = new Color(5f / 255f, 255f / 255f, 234f / 255f);
                break;
            case Beast.types.Normal:
                color = new Color(255f / 255f, 213f / 255f, 5f / 255f);
                break;
        }

        return color;
    }

    // Checks for type advantages and dissadvantages
    void calculateType(Beast attacker, Beast target)
    {
        
        int[] attackertype = new int[2];
        attackertype[0] = (int)attacker.type;
        attackertype[1] = (int)attacker.secondType;
        int[] defendertype = new int[2];
        defendertype[0] = (int)target.type;
        defendertype[1] = (int)target.secondType;
        for(int x = 0; x < attackertype.Length; x++)
        {
            for(int y = 0; y < defendertype.Length; y++)
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
        
    }
}
