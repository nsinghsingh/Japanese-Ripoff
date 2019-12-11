using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats: MonoBehaviour
{
    public Text levelText;
    public Text moneyText;
    public Text micromoneyText;
    public Image Expmask;
    public Image Staminamask;
    public PlayerData playerData;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = playerData.level.ToString();
        moneyText.text = playerData.money.ToString();
        micromoneyText.text = playerData.micromoney.ToString();
        getCurrentFill();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerData.level -= 1;
            playerData.currentExp += 1;
            playerData.currentStamina += 1;
        }
    }
    void getCurrentFill()
    {
        float fillAmount = playerData.currentExp / playerData.maxExp;
        Expmask.fillAmount = fillAmount;
        fillAmount = playerData.currentStamina / playerData.maxStamina;
        Staminamask.fillAmount = fillAmount;
    }
}
