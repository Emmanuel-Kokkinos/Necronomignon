﻿using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

[System.Serializable]
public class Beast
{
    public string name = "";
    public int hitPoints;
    public int defence;
    public int power;
    public int speed;
    public int dexterity;
    public int number_MOVES;
    public int tier;
    public int id;
    public types type;
    public int moveA;
    public Move Move_A;
    public int moveB;
    public Move Move_B;
    public string static_img;

    public enum types { Water, Fire, Earth, Air, Dark, Light };
    BeastManager bm = new BeastManager();


    public Beast()
    {
       
    }

    public Beast(Beast b)
    {
        this.name = b.name;
        this.hitPoints = b.hitPoints;
        this.defence = b.defence;
        this.power = b.power;
        this.speed = b.speed;
        this.dexterity = b.dexterity;
        this.number_MOVES = b.number_MOVES;
        this.tier = b.tier;
        this.type = b.type;
        this.moveA = b.moveA;
        this.Move_A = b.Move_A;
        this.moveB = b.moveB;
        this.Move_B = b.Move_B;
        this.static_img = b.static_img;
    }

    public void setAttacks()
    {
        bm = new BeastManager();
        bm.moveManager = new MoveManager();
        bm.moveManager.start();
        this.Move_A = bm.getMove(this.moveA);
        this.Move_B = bm.getMove(this.moveB);

    }

    /*
    private bool summoned = true;
    private int rating;
    private int hitPoints = 700;
    private int defense = 60;
    private int power = 50;
    private int speed = 35;
    private int skill = 30;
    private int MOVES = 2;
    private int moveA = 60;
    private int moveB = 40;
    private String name;
    private String static_image;
    private int id;

    public bool Summoned { get => summoned; set => summoned = value; }
    public String Name { get => name; set => name = value; }
    public int HitPoints { get => hitPoints; set => hitPoints = value; }
    public int Defense { get => defense; set => defense = value; }
    public int Power { get => power; set => power = value; }
    public int Speed { get => speed; set => speed = value; }
    public int Skill { get => skill; set => skill = value; }
    public int MOVES_NUMBER { get => MOVES; set => MOVES = value; }
    public int MoveA { get => moveA; set => moveA = value; }
    public int MoveB { get => moveB; set => moveB = value; }
    public string Static_image { get => static_image; set => static_image = value; }
    public int Rating { get => rating; set => rating = value; }
    public int Id { get => id; set => id = value; }*/
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

            if (b.name.Equals(this.name) /*&& b.id == this.id*/)
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
