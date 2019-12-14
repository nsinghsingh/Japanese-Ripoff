using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadStage : MonoBehaviour
{
    public int levelIndex;
    private PlayerData playerData;
    public int neededStamina;
    public string stageName;
    public Text text;

    public void Start()
    {
        playerData = PlayerData.playerData;
        text.text = stageName + " STA: " + neededStamina;
    }

    public void loadScene()
    {
        if (playerData.currentStamina < neededStamina)
        {
            Debug.Log("no stamina");
        }
        else
        {
            DontDestroyOnLoad(playerData);
            SceneManager.LoadScene(levelIndex);
        }
    }
}
