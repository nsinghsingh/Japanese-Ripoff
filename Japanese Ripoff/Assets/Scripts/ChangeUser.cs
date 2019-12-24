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
    private string username;
    private int level;
    private int stamina;
    private int exp;
    private int money;
    private int importCode;
    private string oldName;
    public GameObject popup;
    public InputField nameField;
    public InputField levelField;
    public InputField staminaField;
    public InputField expField;
    public InputField moneyField;
    public InputField codeField;

    public void appear(string oldName)
    {
        popup.transform.localPosition = new Vector3(0, 0, 0);
        nameField.text = "";
        levelField.text = "";
        staminaField.text = "";
        expField.text = "";
        moneyField.text = "";
        codeField.text = "";
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
            if (nameField.text.Length > 0) { username = nameField.text; }
            else { username = oldName; }
            if (levelField.text.Length > 0) { level = int.Parse(levelField.text); }
            else { level = int.Parse(reader[2].ToString()); }
            if (staminaField.text.Length > 0) { stamina = int.Parse(staminaField.text); }
            else { stamina = int.Parse(reader[3].ToString()); }
            if (expField.text.Length > 0) { exp = int.Parse(expField.text); }
            else { exp = int.Parse(reader[4].ToString()); }
            if (moneyField.text.Length > 0) { money = int.Parse(moneyField.text); }
            else { money = int.Parse(reader[5].ToString()); }
            if (codeField.text.Length > 0) { importCode = int.Parse(codeField.text); }
            else { importCode = int.Parse(reader[6].ToString()); }
        }
        changeUser(dbcon);
    }

    public void changeUser(IDbConnection dbcon)
    {
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "UPDATE User SET name = '" + username + "', level = '" + level + "', stamina = '" + stamina + "', exp = '" + exp + "', money = '" + money + "', importCode = '" + importCode + "' WHERE name = '" + oldName + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        close();
        dbcon.Close();
    }

    public void checkValues()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT name FROM User WHERE name = '" + username + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        bool userExists = false;
        while (reader.Read())
        { userExists = true; }
        if (!userExists)
        {
            getValues(dbcon);
        }
        else
        {
            nameField.GetComponent<Image>().color = new Color(255, 0, 0);
            nameField.text = "";
            nameField.placeholder.GetComponent<Text>().text = "Please give in a unique name";
            nameField.onValueChanged.AddListener(delegate { setNormal("Enter a new name...", nameField); });
        }
        dbcon.Close();
    }

    public void setNormal(string placeholder, InputField field)
    {
        field.GetComponent<Image>().color = new Color(255, 255, 255);
        field.placeholder.GetComponent<Text>().text = placeholder;
        field.onValueChanged.RemoveAllListeners();
    }
}
