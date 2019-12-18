using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats: MonoBehaviour
{
    public Text levelText;
    public Text moneyText;
    public Image Expmask;
    public Image Staminamask;
    private PlayerData playerData;

    void Start()
    {
        playerData = PlayerData.playerData;
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = playerData.level.ToString();
        moneyText.text = playerData.money.ToString();
        getCurrentFill();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerData.level += 1;
            playerData.currentExp += 1;
            playerData.currentStamina += 1;
        }
    }
    void getCurrentFill()
    {
        float fillAmount = (float)playerData.currentExp / (float)playerData.maxExp;
        Expmask.fillAmount = fillAmount;
        fillAmount = (float)playerData.currentStamina / (float)playerData.maxStamina;
        Staminamask.fillAmount = fillAmount;
    }
}
