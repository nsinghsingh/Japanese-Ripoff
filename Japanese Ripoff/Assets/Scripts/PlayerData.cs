﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int id = 1;
    public int level = 1;
    public int money = 0;
    public float currentExp = 0;
    public float maxExp = 10;
    public float currentStamina = 0;
    public float maxStamina = 20;
    public List<Card> cards = new List<Card>();
    public List<Card> enemies = new List<Card>();
    public List<Card> allies = new List<Card>();

    public static PlayerData playerData { get; private set; }

    private void Awake()
    {
        if (playerData == null)
        {
            playerData = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
