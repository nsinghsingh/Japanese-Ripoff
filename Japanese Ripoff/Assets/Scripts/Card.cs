using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image profile;
    public int attack;
    public int defense;
    public int health;
    public int id;
    public string[] passivesText;
    public string rarity;
    public Image rarityIcon;
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
