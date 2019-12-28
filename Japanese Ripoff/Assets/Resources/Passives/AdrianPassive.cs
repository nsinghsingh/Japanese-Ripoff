using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdrianPassive : Passive
{
    public
    override void attackPassive()
    {
        Fight fight = fightManager.GetComponent<Fight>();
        fight.currentAttack += 911;
        fight.refreshStats();
    }
    public
    override void defensePassive()
    {
        Debug.Log("911"); Fight fight = fightManager.GetComponent<Fight>();
        fight.currentDefense += 911;
        fight.refreshStats();
    }
    public
    override void supportPassive()
    {
        Debug.Log("911"); Fight fight = fightManager.GetComponent<Fight>();
        fight.teamCurrentHP += 911;
        fight.refreshStats();
    }
}
