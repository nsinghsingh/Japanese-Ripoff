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
    private bool hasName;
    private bool hasPassword;
    public InputField nameField;
    public InputField passwordField;
    public Text error;
    public Text submitText;
    public Text title;
    public Button Link;
    public Image ErrorField;
    public Button submitButton;

    // Start is called before the first frame update
    void Start()
    {
        setLogin("", 0);
        nameField.onValueChanged.AddListener(delegate { requireName(); });
        passwordField.onValueChanged.AddListener(delegate { requirePassword(); });
        submitButton.interactable = false;
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
        if (hasPassword && hasName) { submitButton.interactable = true; }
        else { submitButton.interactable = false; }
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
        if (hasPassword && hasName) { submitButton.interactable = true; }
        else { submitButton.interactable = false; }
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
            Invoke("setNormal", 2.0f);
        }
        else if (status == 2)
        {
            ErrorField.color = new Color(0, 255, 0, 255);
            Invoke("setNormal", 2.0f);
        }
        submitText.text = "Login";
        passwordField.text = "";
        passwordField.GetComponent<Image>().color = new Color(255, 255, 255);
        passwordField.placeholder.GetComponent<Text>().text = "Enter a password...";
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
            Invoke("setNormal", 2.0f);
        }
        else if (status == 2)
        {
            ErrorField.color = new Color(0, 255, 0, 255);
            Invoke("setNormal", 2.0f);
        }
        submitText.text = "Register";
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
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT name FROM User WHERE name = '" + nameField.text + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        bool userNotExist = true;
        while (reader.Read())
        {
            userNotExist = false;
        }
        if (userNotExist)
        {
            commandRead = dbcon.CreateCommand();
            query = "INSERT INTO User (name, password) VALUES('" + nameField.text + "', '" + passwordField.text + "')";
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
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT name, password FROM User WHERE name = '" + nameField.text + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        bool userExist = false;
        bool isPasswordRight = false;
        while (reader.Read())
        {
            userExist = true;
            isPasswordRight = passwordField.text == reader[1].ToString();
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

    public void setNormal()
    {
        error.text = "";
        ErrorField.color = new Color(0, 0, 0, 0);
    }
}
