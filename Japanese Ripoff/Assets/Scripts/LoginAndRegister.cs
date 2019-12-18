using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginAndRegister : MonoBehaviour
{
    private bool isLogingIn;
    public InputField username;
    public InputField password;
    public Text error;
    public Text submitButton;
    public Text title;
    public Button Link;
    public Image ErrorField;

    // Start is called before the first frame update
    void Start()
    {
        setLogin("", 0);
    }

    public void setLogin(string message, int status)
    {
        isLogingIn = true;
        title.text = "Login";
        error.text = message;
        if (status == 0) {
            ErrorField.color = new Color(0, 0, 0, 0);
        }
        else if (status == 1)
        {
            ErrorField.color = new Color(255, 0, 0, 255);
        }
        else if (status == 2)
        {
            ErrorField.color = new Color(0, 255, 0, 255);
        }
        submitButton.text = "Login";
        password.text = "";
        Link.onClick.RemoveAllListeners();
        Link.onClick.AddListener(delegate () { setRegister("", 0); });
        Link.GetComponent<Text>().text = "Still don't have an account?";
        
    }

    public void setRegister(string message, int status)
    {
        isLogingIn = false;
        title.text = "Register";
        error.text = message;
        if (status == 0)
        {
            ErrorField.color = new Color(0, 0, 0, 0);
        }
        else if (status == 1)
        {
            ErrorField.color = new Color(255, 0, 0, 255);
        }
        else if (status == 2)
        {
            ErrorField.color = new Color(0, 255, 0, 255);
        }
        submitButton.text = "Register";
        Link.onClick.RemoveAllListeners();
        Link.onClick.AddListener(delegate () { setLogin("", 0); });
        Link.GetComponent<Text>().text = "Go back";
    }

    public void submit()
    {
        if (isLogingIn)
        {
            login();
        }
        else
        {
            register();
        }
    }

    public void register()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        //C:\Users\nihal\AppData\LocalLow\DefaultCompany\Japanese Ripoff
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT name FROM User WHERE name = '" + username.text + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        bool userNotExist = true;
        while (reader.Read())
        {
            userNotExist = false;
        }
        Debug.Log(userNotExist);
        if (userNotExist)
        {
            commandRead = dbcon.CreateCommand();
            query = "INSERT INTO User (name, password) VALUES('" + username.text + "', '" + password.text + "')";
            commandRead.CommandText = query;
            reader = commandRead.ExecuteReader();
            setLogin("You succesfully created an account", 2);
        }
        else
        {
            setRegister("That user already exists!", 1);
        }
        dbcon.Close();   
    }

    public void login()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        //C:\Users\nihal\AppData\LocalLow\DefaultCompany\Japanese Ripoff
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT name, password FROM User WHERE name = '" + username.text + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        bool userExist = false;
        bool isPasswordRight = false;
        while (reader.Read())
        {
            userExist = true;
            isPasswordRight = password.text == reader[1].ToString();
        }
        dbcon.Close();
        if (userExist && isPasswordRight)
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(1);
        }
        else
        {
            setLogin("Your password or username is wrong", 1);
        }
    }
}
