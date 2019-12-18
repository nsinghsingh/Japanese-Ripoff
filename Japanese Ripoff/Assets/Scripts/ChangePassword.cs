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
    }

    public void close()
    {
        popup.transform.localPosition = new Vector3(2000, 0, 0);
        setNormal("");
    }

    public void changePassword()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        //C:\Users\nihal\AppData\LocalLow\DefaultCompany\Japanese Ripoff
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
        if (isPasswordRight)
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
            oldPassword.onValueChanged.AddListener(delegate { setNormal(oldPassword.text); });

        }
        dbcon.Close();
    }

    public void setNormal(string old)
    {
        oldPassword.GetComponent<Image>().color = new Color(255, 255, 255);
        oldPassword.text = old;
        oldPassword.placeholder.GetComponent<Text>().text = "Enter old password..";
        oldPassword.onValueChanged.RemoveAllListeners();
    }
}
