using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;
using UnityEngine.UI;

public class ChangeUser : MonoBehaviour
{
    private PlayerData playerData;
    private string username;
    private int level;
    private int stamina;
    private int exp;
    private int money;
    private int importCode;
    private string oldName;
    public GameObject popup;
    public InputField namefield;
    public InputField levelfield;
    public InputField staminafield;
    public InputField expfield;
    public InputField moneyfield;
    public InputField codefield;

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.playerData;
    }

    public void appear(string oldName)
    {
        popup.transform.localPosition = new Vector3(0, 0, 0);
        namefield.text = "";
        levelfield.text = "";
        staminafield.text = "";
        expfield.text = "";
        moneyfield.text = "";
        codefield.text = "";
        this.oldName = oldName;
    }

    public void close()
    {
        popup.transform.localPosition = new Vector3(2000, 0, 0);
    }

    private void getValues(IDbConnection dbcon)
    {
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM User WHERE name = '" + oldName + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        while (reader.Read())
        {
            if (namefield.text.Length > 0){username = namefield.text;}
            else{username = oldName;}
            if (levelfield.text.Length > 0) { level = int.Parse(levelfield.text); }
            else { level = int.Parse(reader[2].ToString()); }
            if (staminafield.text.Length > 0) { stamina = int.Parse(staminafield.text); }
            else { stamina = int.Parse(reader[3].ToString()); }
            if (expfield.text.Length > 0) { exp = int.Parse(expfield.text); }
            else { exp = int.Parse(reader[4].ToString()); }
            if (moneyfield.text.Length > 0) { money = int.Parse(moneyfield.text); }
            else { money = int.Parse(reader[5].ToString()); }
            if (codefield.text.Length > 0) { importCode = int.Parse(codefield.text); }
            else { importCode = int.Parse(reader[6].ToString()); }
        }
    }

    public void changeUser()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        //C:\Users\nihal\AppData\LocalLow\DefaultCompany\Japanese Ripoff
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        getValues(dbcon);
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "UPDATE User SET name = '" + username + "', level = '" + level + "', stamina = '" + stamina + "', exp = '" + exp + "', money = '" + money + "', importCode = '" + importCode + "' WHERE name = '" + oldName + "'" ;
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        close();
        dbcon.Close();
    }
}
