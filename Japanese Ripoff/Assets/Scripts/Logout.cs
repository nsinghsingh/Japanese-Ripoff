using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    private PlayerData playerData;
    public GameObject popup;

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.playerData;
    }

    public void appear()
    {
        popup.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void close()
    {
        popup.transform.localPosition = new Vector3(2000, 0, 0);
    }

    public void logout()
    {
        Destroy(playerData.gameObject);
        SceneManager.LoadScene(0);
    }
}
