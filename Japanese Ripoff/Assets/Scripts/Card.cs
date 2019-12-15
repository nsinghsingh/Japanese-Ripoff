using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Sprite profile;
    public int attack;
    public int defense;
    public int health;
    public int id;
    public string cardName;
    public string[] passivesText;
    public string rarity;
    public Sprite rarityIcon;
    public Passive Passives;

    public void attackPassive()
    {
        Passives.attackPassive();
    }
    public void defensePassive()
    {
        Passives.defensePassive();
    }
    public void supportPassive()
    {
        Passives.supportPassive();
    }
}
