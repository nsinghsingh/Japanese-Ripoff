using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using System;
using System.IO;

public class ChangePassword : MonoBehaviour
{
    private PlayerData playerData;
    private bool[] hasValues = new bool[2];
    public Button change;
    public InputField oldPassword;
    public InputField newPassword;
    public GameObject popup;

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.playerData;   
    }

    public void appear()
    {
        popup.transform.localPosition = new Vector3(0, 0, 0);
        oldPassword.text = "";
        oldPassword.onValueChanged.AddListener(delegate { requireValue(oldPassword, 0, "password"); });
        newPassword.text = "";
        newPassword.onValueChanged.AddListener(delegate { requireValue(newPassword, 1, "password"); });
    }

    public void close()
    {
        popup.transform.localPosition = new Vector3(2000, 0, 0);
    }

    public void changePassword()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT password FROM User WHERE name = '" + playerData.user + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        bool isPasswordRight = false;
        while (reader.Read())
        {
            isPasswordRight = oldPassword.text == reader[0].ToString();
        }
        if (isPasswordRight && newPassword.text.Length > 0)
        {
            commandRead = dbcon.CreateCommand();
            query = "UPDATE User SET password = '" + newPassword.text +"' WHERE name = '" + playerData.user + "'";
            commandRead.CommandText = query;
            reader = commandRead.ExecuteReader();
            close();
        }
        else
        {
            oldPassword.GetComponent<Image>().color = new Color(255, 0, 0);
            oldPassword.text = "";
            oldPassword.placeholder.GetComponent<Text>().text = "Please give in the right password";
        }
        dbcon.Close();
    }

    private void requireValue(InputField field, int index, string value)
    {

        if (field.text.Length > 0)
        {
            hasValues[index] = true;
            field.GetComponent<Image>().color = new Color(255, 255, 255);
            field.placeholder.GetComponent<Text>().text = "Enter a " + value + "...";
        }
        else
        {
            hasValues[index] = false;
            field.GetComponent<Image>().color = new Color(255, 0, 0);
            field.placeholder.GetComponent<Text>().text = "A " + value + " is required";
        }
        hasAllValues();
    }

    private void hasAllValues()
    {
        bool hasFalse = false;
        foreach (bool hasValue in hasValues)
        {
            if (!hasValue)
            {
                hasFalse = true;
            }
        }
        if (!hasFalse)
        {
            change.interactable = true;
        }
        else { change.interactable = false; }
    }
}
