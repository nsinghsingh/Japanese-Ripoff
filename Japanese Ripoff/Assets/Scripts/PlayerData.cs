using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public string user;
    public int level;
    public int money;
    public int currentExp;
    public int maxExp;
    public int currentStamina;
    public int maxStamina;
    public int importCode;
    public List<GameObject> cards = new List<GameObject>();

    public static PlayerData playerData { get; private set; }

    private void Awake()
    {
        if (playerData == null)
        {
            playerData = this;
            DontDestroyOnLoad(gameObject);
            playerData.user = GameObject.Find("LoginFunctionality").GetComponent<LoginAndRegister>().nameField.text;
            Destroy(GameObject.Find("LoginFunctionality"));
            makeSQLConnection();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void refresh()
    {
        foreach (GameObject card in cards)
        {
            Destroy(card);
        }
        makeSQLConnection();
    }

    private void makeSQLConnection()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        getCards(dbcon);
        getUser(dbcon);
        updateStats(dbcon);
        dbcon.Close();
    }

    private void getCards(IDbConnection dbcon)
    {
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM NumberOfCardsPerUser LEFT JOIN Card ON id = fkCardId WHERE fkUser = '" + user + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        while (reader.Read())
        {
            GameObject card = new GameObject();
            DontDestroyOnLoad(card);
            card.AddComponent<Card>();
            Card cardInfo = card.GetComponent<Card>();
            cardInfo.id = int.Parse(reader[3].ToString());
            cardInfo.cardName = reader[4].ToString();
            card.name = reader[4].ToString();
            cardInfo.health = int.Parse(reader[5].ToString());
            cardInfo.attack = int.Parse(reader[6].ToString());
            cardInfo.defense = int.Parse(reader[7].ToString());
            cardInfo.rarity = reader[8].ToString();
            cardInfo.passivesText = new string[] { reader[9].ToString(), reader[10].ToString(), reader[11].ToString() };
            card.AddComponent(Type.GetType(reader[12].ToString()));
            cardInfo.Passives = card.GetComponent<Passive>();
            cardInfo.profile = Resources.Load<Sprite>("Cards/" + reader[13].ToString());
            if (cardInfo.rarity.Equals("ssr"))
            {
                cardInfo.rarityIcon = Resources.Load<Sprite>("Icons/SuperSuperRareRarity");
            }
            else if (cardInfo.rarity.Equals("sr"))
            {
                cardInfo.rarityIcon = Resources.Load<Sprite>("Icons/SuperRareRarity");
            }
            else if (cardInfo.rarity.Equals("r"))
            {
                cardInfo.rarityIcon = Resources.Load<Sprite>("Icons/RareRarity");
            }
            else
            {
                cardInfo.rarityIcon = Resources.Load<Sprite>("Icons/NormalRarity");
            }
            cards.Add(card);
        }
    }

    private void getUser(IDbConnection dbcon)
    {
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        commandRead = dbcon.CreateCommand();
        string query = "SELECT * FROM User WHERE name = '" + user + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        while (reader.Read())
        {
            level = int.Parse(reader[2].ToString());
            currentStamina = int.Parse(reader[3].ToString());
            currentExp = int.Parse(reader[4].ToString());
            money = int.Parse(reader[5].ToString());
            importCode = int.Parse(reader[6].ToString());
            maxExp = 20 + level * 3;
            maxStamina = 20 + level;
        }
    }

    public static void copyCardInfo(GameObject newCard, GameObject oldCard)
    {
        Card newInfo = newCard.GetComponent<Card>();
        Card oldInfo = oldCard.GetComponent<Card>();
        newInfo.profile = oldInfo.profile;
        newInfo.attack = oldInfo.attack;
        newInfo.defense = oldInfo.defense;
        newInfo.health = oldInfo.health;
        newInfo.id = oldInfo.id;
        newInfo.cardName = oldInfo.cardName;
        newInfo.passivesText = oldInfo.passivesText;
        newInfo.rarity = oldInfo.rarity;
        newInfo.rarityIcon = oldInfo.rarityIcon;
        newInfo.Passives = oldInfo.Passives;
    }

    private void setUser(IDbConnection dbcon)
    {
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "UPDATE User SET name = '" + user + "', level = '" + level + "', stamina = '" + currentStamina + "', exp = '" + currentExp + "', money = '" + money + "', importCode = '" + importCode + "' WHERE name = '" + user + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
    }

    private void setCards(IDbConnection dbcon)
    {
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "DELETE FROM NumberOfCardsPerUser WHERE fkUser = '" + user + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        foreach (GameObject card in cards)
        {
            commandRead = dbcon.CreateCommand();
            query = "INSERT INTO NumberOfCardsPerUser (fkUser, fkCardId) VALUES ('" + user + "', '" + card.GetComponent<Card>().id + "')";
            commandRead.CommandText = query;
            reader = commandRead.ExecuteReader();
            Destroy(card);
        }
    }

    private void updateStats(IDbConnection dbcon)
    {
        string date = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "DELETE FROM Stat WHERE date = '" + date + "'; INSERT INTO Stat (date, totalUsers) VALUES ('" + date + "', (SELECT COUNT(*) FROM User))";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
    }

    private void OnDestroy()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        setUser(dbcon);
        setCards(dbcon);
        dbcon.Close();
    }
}
