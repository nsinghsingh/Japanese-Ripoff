using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;
using UnityEngine.UI;

public class SeeAllUsers : MonoBehaviour
{
    private int counter = 0;
    public GameObject row;
    public GameObject grid;
    public GameObject changeUser;
    public GameObject deleteUser;

    public void loadUsers()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        //C:\Users\nihal\AppData\LocalLow\DefaultCompany\Japanese Ripoff
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM User WHERE NOT name = 'admin'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        while (reader.Read())
        {
            counter += 1;
            GameObject infoRow = Instantiate(row, new Vector2(0, 0), Quaternion.identity);
            infoRow.GetComponent<InfoRow>().setRow(reader[0].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), changeUser, deleteUser);
            if ((counter % 2) != 0)
            {
                infoRow.GetComponent<Image>().color = new Color(255, 255, 255);
            }
            infoRow.transform.SetParent(grid.transform);
            infoRow.transform.localScale = new Vector3(1, 1, 1);
        }
        counter = 0;
        dbcon.Close();
    }

    public void unloadUsers()
    {
        foreach (Transform child in grid.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
