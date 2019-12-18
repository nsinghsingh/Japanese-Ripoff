using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    private PlayerData playerData;

    private void Start()
    {
        playerData = PlayerData.playerData;
    }

    public void loadSettings()
    {
        if (playerData.user.Equals("admin"))
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
    }
}
