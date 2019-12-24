using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;
using UnityEngine.UI;

public class SeeAllCards : MonoBehaviour
{
    private int counter = 0;
    public GameObject row;
    public GameObject grid;
    public GameObject changeCard;
    public GameObject deleteCard;
    public GameObject amount;

    public void loadCards()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM Card";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        while (reader.Read())
        {
            counter += 1;
            GameObject cardRow = Instantiate(row, new Vector2(0, 0), Quaternion.identity);
            cardRow.transform.SetParent(grid.transform);
            cardRow.transform.localScale = new Vector3(1, 1, 1);
            cardRow.GetComponent<CardRow>().setRow(int.Parse(reader[0].ToString()), reader[1].ToString(), reader[5].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[9].ToString(), changeCard, deleteCard);
            if ((counter % 2) != 0)
            {
                cardRow.GetComponent<Image>().color = new Color(255, 255, 255);
            }
        }
        amount.GetComponent<Text>().text = counter + " cards";
        counter = 0;
        dbcon.Close();
    }

    public void unloadCards()
    {
        foreach (Transform child in grid.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
