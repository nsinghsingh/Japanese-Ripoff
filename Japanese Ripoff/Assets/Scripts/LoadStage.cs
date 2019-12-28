using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadStage : MonoBehaviour
{
    public int sceneIndex;
    private PlayerData playerData;
    public int neededStamina;
    public string stageName;
    public Text text;
    public Button play;

    public void Start()
    {
        playerData = PlayerData.playerData;
        text.text = stageName + " STA: " + neededStamina;
    }

    public void loadSelect()
    {
        play.onClick.RemoveAllListeners();
        play.onClick.AddListener(delegate () { loadScene(); });
    }

    public void loadScene()
    {
        if (playerData.currentStamina < neededStamina)
        {
            Debug.Log("no stamina");
            //TO DO Popup
        }
        else
        {
            playerData.currentStamina -= neededStamina;
            DontDestroyOnLoad(playerData);
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
