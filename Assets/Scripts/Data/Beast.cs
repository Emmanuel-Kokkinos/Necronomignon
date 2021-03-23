using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
//using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Beast
{
    public string name = "";
    public int maxHP;
    public int hitPoints;
    public int defence;
    public int power;
    public int speed;
    public int dexterity;
    public int number_MOVES;
    public int tier;
    public int id;
    public int size;
    public int cost;
    [XmlElement(Namespace = "Beast")]
    public types type;
    public types secondType;
    public int moveA;
    public Move Move_A;
    public int moveB;
    public Move Move_B;
    public string static_img;
    public StatGradient statGradients;
    public List<Buff> buffs = new List<Buff>();
    public bool nonCombatant;
    public bool isEnemy = false;

    public enum types {[XmlEnum(Name = "Normal")] Normal, [XmlEnum(Name = "Water")] Water, [XmlEnum(Name = "Fire")] Fire, [XmlEnum(Name = "Earth")] Earth, [XmlEnum(Name = "Air")] Air, [XmlEnum(Name = "Dark")] Dark, [XmlEnum(Name = "Light")] Light, [XmlEnum(Name = "Horror")] Horror, [XmlEnum(Name = "Cosmic")] Cosmic };

    public int[] statusTurns = new int[8];
    BeastManager bm = new BeastManager();

    public Beast cursed = null;
    public int curseCharge = 0;

    public Beast(){}

    public Beast(Beast b)
    {
        this.name = b.name;
        this.hitPoints = b.hitPoints;
        this.maxHP = b.maxHP;
        this.defence = b.defence;
        this.power = b.power;
        this.speed = b.speed;
        this.dexterity = b.dexterity;
        this.number_MOVES = b.number_MOVES;
        this.tier = b.tier;
        this.size = b.size;
        this.cost = b.cost;
        this.type = b.type;
        this.secondType = b.secondType;
        this.moveA = b.moveA;
        this.Move_A = b.Move_A;
        this.moveB = b.moveB;
        this.Move_B = b.Move_B;
        this.static_img = b.static_img;
        this.statGradients = b.statGradients;
    }

    public void setAttacks()
    {
        bm = new BeastManager();
        bm.moveManager = new MoveManager();
        bm.moveManager.start();
        this.Move_A = bm.getMove(this.moveA);
        this.Move_B = bm.getMove(this.moveB);
    }

    public void setTierUpper(int x)
    {
        if(x<1 || x > 5)
        {
            return;
        }
        this.tier = UnityEngine.Random.Range(1, x+1);
    }

    public void setTierLower(int x)
    {
        if (x < 1 || x > 5)
        {
            return;
        }
        this.tier = UnityEngine.Random.Range(x, 6);
    }

    public bool curse(Beast target)
    {
        if (this.cursed == null)
        {
            this.cursed = target;
            this.curseCharge = 3;
            return false;
        }
        else if (this.curseCharge > 1)
        {
            this.curseCharge--;
            return false;
        }
        else if(this.curseCharge <= 1)
        {
            target = this.cursed;
            this.cursed = null;
            this.curseCharge = 0;
            return true;
        }
        return false;
    }

    override
    public String ToString()
    {
        String str = "";
        str += "Name = " + name + "\n";
        str += "HP = " + hitPoints + "\n";
        str += "Defence = " + defence + "\n";
        str += "Power = " + power + "\n";
        str += "Speed = " + speed + "\n";
        str += "Dexterity = " + dexterity + "\n";
        str += "Moves = " + number_MOVES + "\n";
        str += "Tier = " + tier + "\n";
        str += "img = " + static_img + "\n";
        str += "Move A = " + Move_A + "\n";
        str += "Move B = " + Move_B + "\n";
        return str;
    }
    override
    public bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (obj.GetType() == typeof(Beast))
        {
            Beast b = (Beast)obj;

            if (b.name.Equals(this.name) && b.id == this.id)
            {
                return true;
            }
        }
        else { 
           Debug.Log("The boolean expresion is wrong"); 
        }
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
