using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;

public class AddUser : MonoBehaviour
{
    public GameObject popup;
    private string username;
    private string password;
    private int level;
    private int stamina;
    private int money;
    private int importCode;
    public InputField nameField;
    public InputField passwordField;
    public InputField levelField;
    public InputField staminaField;
    public InputField moneyField;
    public InputField codeField;
    bool hasName = false;
    bool hasPassword = false;
    private string[] levelQuery;
    private string[] staminaQuery;
    private string[] moneyQuery;
    private string[] codeQuery;
    public Button add;

    public void appear()
    {
        popup.transform.localPosition = new Vector3(0, 0, 0);
        nameField.onValueChanged.AddListener(delegate { requireName(); });
        passwordField.onValueChanged.AddListener(delegate { requirePassword(); });
        nameField.text = "";
        passwordField.text = "";
        levelField.text = "";
        staminaField.text = "";
        moneyField.text = "";
        codeField.text = "";
        add.interactable = false;
    }

    public void close()
    {
        popup.transform.localPosition = new Vector3(2000, 0, 0);
        nameField.onValueChanged.RemoveAllListeners();
        passwordField.onValueChanged.RemoveAllListeners();
    }

    private void requireName()
    {
        if (nameField.text.Length > 0)
        {
            hasName = true;
            nameField.GetComponent<Image>().color = new Color(255, 255, 255);
            nameField.placeholder.GetComponent<Text>().text = "Enter a name...";
        }
        else
        {
            hasName = false;
            nameField.GetComponent<Image>().color = new Color(255, 0, 0);
            nameField.placeholder.GetComponent<Text>().text = "A name is required";
        }
        if (hasPassword && hasName) { add.interactable = true; }
        else { add.interactable = false; }
    }

    private void requirePassword()
    {
        if (passwordField.text.Length > 0)
        {
            hasPassword = true;
            passwordField.GetComponent<Image>().color = new Color(255, 255, 255);
            passwordField.placeholder.GetComponent<Text>().text = "Enter a password...";
        }
        else
        {
            hasPassword = false;
            passwordField.GetComponent<Image>().color = new Color(255, 0, 0);
            passwordField.placeholder.GetComponent<Text>().text = "A password is required";
        }
        if (hasPassword && hasName) { add.interactable = true; }
        else { add.interactable = false; }
    }

    private void addUser()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        //C:\Users\nihal\AppData\LocalLow\DefaultCompany\Japanese Ripoff
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "INSERT INTO User (name, password" + levelQuery[0] + staminaQuery[0] + moneyQuery[0] + codeQuery[0] + ") VALUES ('" + username + "', '" + password + "'" + levelQuery[1] + staminaQuery[1] + moneyQuery[1] + codeQuery[1] + ")";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        dbcon.Close();
    }

    private void getValues()
    {
        if (nameField.text.Length > 0) { username = nameField.text; }
        if (passwordField.text.Length > 0){ password = passwordField.text; }
        if (levelField.text.Length > 0){
            level = int.Parse(levelField.text);
            levelQuery = new string[] { ", level ", ", '" + level + "'" };
        }
        else { levelQuery = new string[] { "", ""};}
        if (staminaField.text.Length > 0){
        stamina = int.Parse(staminaField.text);
        staminaQuery = new string[] { ", stamina ", ", '" + stamina + "'" };
        }
        else { levelQuery = new string[] { "", "" }; }
        if (moneyField.text.Length > 0){
            money = int.Parse(moneyField.text);
            moneyQuery = new string[] { ", money ", ", '" + money + "'" };
        }
        else { moneyQuery = new string[] { "", "" }; }
        if (codeField.text.Length > 0){
            importCode = int.Parse(codeField.text);
            codeQuery = new string[] { ", importCode ", ", '" + importCode + "'" };
        }
        else { codeQuery = new string[] { "", "" }; }
    }

    public void checkValues()
    {
        getValues();
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        //C:\Users\nihal\AppData\LocalLow\DefaultCompany\Japanese Ripoff
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
            commandRead = dbcon.CreateCommand();
            query = "SELECT importCode FROM User WHERE importCode = '" + importCode + "'";
            commandRead.CommandText = query;
            reader = commandRead.ExecuteReader();
            bool codeExists = false;
            while (reader.Read()) { codeExists = true; }
            if (!codeExists) { addUser(); }
            else {
                
                codeField.GetComponent<Image>().color = new Color(255, 0, 0);
                codeField.text = "";
                codeField.placeholder.GetComponent<Text>().text = "Please give in a unique code or none";
                Invoke("setNormal", 4.0f);
            }
        }
        else{
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
    
    public void setNormal()
    {
        setNormal("Enter a new code...", codeField);
    }
}
