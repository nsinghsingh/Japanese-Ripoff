using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;

public class MakeImportCode : MonoBehaviour
{
    private PlayerData playerData;
    private int code;
    public GameObject popup;
    public Button add;
    public Text info;

    private void Start()
    {
        playerData = PlayerData.playerData;
    }

    public void appear()
    {
        popup.transform.localPosition = new Vector3(0, 0, 0);
        makeCode();
    }

    public void close()
    {
        popup.transform.localPosition = new Vector3(2000, 0, 0);
    }

    public void makeCode()
    {
        System.Random random = new System.Random();
        code = random.Next(999999999);
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM User WHERE importCode = '" + code + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        bool doesCodeExist = false;
        while (reader.Read())
        {
            doesCodeExist = true;
        }
        if (doesCodeExist || code == 0)
        {
            makeCode();
        }
        else
        {
            commandRead = dbcon.CreateCommand();
            query = "UPDATE User SET importCode = '" + code + "' WHERE name = '" + playerData.user + "'";
            commandRead.CommandText = query;
            reader = commandRead.ExecuteReader();
        }
        dbcon.Close();
        playerData.importCode = code;
        info.text = "Your new import code is " + code + ". You can use this code to import this account from another one in case you forget your password.";
    }
}
