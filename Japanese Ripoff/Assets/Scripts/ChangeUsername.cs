using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using System;
using System.IO;

public class ChangeUsername : MonoBehaviour
{
    private PlayerData playerData;
    public InputField newUsername;
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
        setNormal();
    }

    public void changeUsername()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT name FROM User WHERE name = '" + newUsername.text + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        bool doesUserExist = false;
        while (reader.Read())
        {
            doesUserExist = true;
        }
        if (!doesUserExist && newUsername.text.Length > 0)
        {
            commandRead = dbcon.CreateCommand();
            query = "UPDATE User SET name = '" + newUsername.text + "' WHERE name = '" + playerData.user + "'";
            commandRead.CommandText = query;
            reader = commandRead.ExecuteReader();
            playerData.user = newUsername.text;
            close();
        }
        else
        {
            newUsername.GetComponent<Image>().color = new Color(255, 0, 0);
            newUsername.text = "";
            newUsername.placeholder.GetComponent<Text>().text = "Please give in an unused name";
            newUsername.onValueChanged.AddListener(delegate { setNormal(); });

        }
        dbcon.Close();
    }

    public void setNormal()
    {
        newUsername.GetComponent<Image>().color = new Color(255, 255, 255);
        newUsername.placeholder.GetComponent<Text>().text = "Enter new username..";
        newUsername.onValueChanged.RemoveAllListeners();
    }
}
