using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;

public class ImportAccount : MonoBehaviour
{
    public GameObject popup;
    public GameObject warning;
    public InputField importField;
    public Button submit;
    private PlayerData playerData;
    private string oldName;

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.playerData;
    }

    public void appear()
    {
        popup.transform.localPosition = new Vector3(0, 0, 0);
        importField.text = "";
        importField.onValueChanged.AddListener(delegate { requireValue(); });
    }

    public void close()
    {
        popup.transform.localPosition = new Vector3(2000, 0, 0);
        warning.transform.localPosition = new Vector3(2000, 0, 0);
        importField.onValueChanged.RemoveAllListeners();
    }

    public void warn()
    {
        warning.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void requireValue()
    {
        if (importField.text.Length > 0)
        {
            submit.interactable = true;
            importField.GetComponent<Image>().color = new Color(255, 255, 255);
            importField.placeholder.GetComponent<Text>().text = "Enter an import code...";
        }
        else
        {
            submit.interactable = false;
            importField.GetComponent<Image>().color = new Color(255, 0, 0);
            importField.placeholder.GetComponent<Text>().text = "An import code is required";
        }
    }

    public void checkCode()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT name FROM User WHERE importCode = '" + importField.text + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        bool doesCodeExist = false;
        while (reader.Read())
        {
            doesCodeExist = true;
            oldName = reader[0].ToString();
        }
        if (doesCodeExist && int.Parse(importField.text) != 0)
        {
            warn();
        }
        else
        {
            submit.interactable = false;
            importField.text = "";
            importField.GetComponent<Image>().color = new Color(255, 0, 0);
            importField.placeholder.GetComponent<Text>().text = "No account has that import code";
        }
        dbcon.Close();
    }

    public void importAccount()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT password FROM User WHERE name = '" + playerData.user + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        string password = "";
        while (reader.Read())
        {
            password = reader[0].ToString();
        }
        commandRead = dbcon.CreateCommand();
        query = "DELETE FROM User WHERE name = '" + playerData.user + "';UPDATE User SET name = '" + playerData.user + "', password = '" + password + "' WHERE importCode = '" + importField.text + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        commandRead = dbcon.CreateCommand();
        query = "DELETE FROM NumberOfCardsPerUser WHERE fkUser = '" + playerData.user + "';UPDATE NumberOfCardsPerUser SET fkUser = '" + playerData.user + "' WHERE fkUser = '" + oldName + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        dbcon.Close();
        playerData.refresh();
        close();
    }
}
