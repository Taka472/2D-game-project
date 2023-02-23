using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public enum Type
{
    dragon, golem, demon, angel, witch, spell
}

public class Card
{
    public int id;
    public string cardName;
    public string description;
    public Type type;
    public int cost;
    public int attack;
    public int health;
    public Sprite Image;

    public Card() { }
    public Card(int id, string name, string description, Type type, int cost, int attack, int health, Sprite Image)
    {
        this.id = id;
        cardName = name;
        this.description = description;
        this.type = type;
        this.cost = cost;
        this.attack = attack;
        this.health = health;
        this.Image = Image;
    }
}
