using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KermitPassive : Passive
{
    public
    override void attackPassive()
    {
        Fight fight = fightManager.GetComponent<Fight>();
        fight.attackBoost += 40;
    }
    public
    override void defensePassive()
    {
        Fight fight = fightManager.GetComponent<Fight>();
        fight.defenseBoost += 30;
        fight.attackBoost += 20;
    }
    public
    override void supportPassive()
    {
        Fight fight = fightManager.GetComponent<Fight>();
        fight.attackBoost = (int)(fight.attackBoost * 1.2);
        fight.defenseBoost += (int)(fight.defenseBoost * 1.2);
    }
}
