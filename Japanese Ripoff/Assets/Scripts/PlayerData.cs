using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class PlayerData : MonoBehaviour
{
    public string user = "admin";
    public int level;
    public int money;
    public int currentExp;
    public int maxExp;
    public int currentStamina;
    public int maxStamina;
    public List<Card> cards = new List<Card>();
    public List<Card> enemies = new List<Card>();
    public List<Card> allies = new List<Card>();

    public static PlayerData playerData { get; private set; }

    private void Awake()
    {
        if (playerData == null)
        {
            playerData = this;
            DontDestroyOnLoad(gameObject);
            makeSQLConnection();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void makeSQLConnection()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        readData(dbcon);
    }

    private void readData(IDbConnection dbcon)
    {
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        commandRead.CommandText = "SELECT * FROM NumberOfCardsPerUser LEFT JOIN Card ON id = fkCardId WHERE fkUser = '" + user + "'";
        reader = commandRead.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log("name: " + reader[0].ToString());
            Debug.Log("name: " + reader[1].ToString());
            Debug.Log("name: " + reader[2].ToString());
            Debug.Log("name: " + reader[3].ToString());
            Debug.Log("name: " + reader[4].ToString());
            Debug.Log("name: " + reader[5].ToString());
        }
        dbcon.Close();
    }
}
