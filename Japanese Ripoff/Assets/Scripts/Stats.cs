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
        InvokeRepeating("recoverStamina", 60.0f, 60.0f);
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = playerData.level.ToString();
        moneyText.text = playerData.money.ToString();
        getCurrentFill();
    }

    private void recoverStamina()
    {
        if (playerData.currentStamina < playerData.maxStamina)
        {
            playerData.currentStamina += 1;
        }
    }

    private void getCurrentFill()
    {
        float fillAmount = (float)playerData.currentExp / (float)playerData.maxExp;
        Expmask.fillAmount = fillAmount;
        fillAmount = (float)playerData.currentStamina / (float)playerData.maxStamina;
        Staminamask.fillAmount = fillAmount;
    }
}
