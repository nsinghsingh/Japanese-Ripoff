using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;

public class TotalPlayersGraph : MonoBehaviour
{
    public Text[] xLabels = new Text[4];
    public Text[] yLabels = new Text[4];
    public Image[] columns = new Image[4];

    public void loadStats()
    {
        setX();
        setY();
        setColumns();
    }

    private void setX()
    {
        for (int i = 0; i < xLabels.Length; i++)
        {
            xLabels[i].text = (DateTime.Now.Day - xLabels.Length + 1 + i) + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
        }
    }

    private void setY()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT MAX(totalUsers) FROM Stat WHERE date = '" + xLabels[0].text + "' OR date = '" + xLabels[1].text + "' OR date = '" + xLabels[2].text + "' OR date = '" + xLabels[3].text + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        float maxValue = 100;
        while (reader.Read())
        {
            maxValue = int.Parse(reader[0].ToString());
        }
        dbcon.Close();
        for (int i = 0; i < yLabels.Length; i++)
        {
            yLabels[i].text = ((int)(maxValue / yLabels.Length * (yLabels.Length-i))).ToString();
        }
    }

    private void setColumns()
    {
        float maxValue = int.Parse(yLabels[0].text);
        for (int i = 0; i < columns.Length; i++)
        {
            string connection = "URI=file:" + Application.persistentDataPath + "/main";
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();
            IDbCommand commandRead = dbcon.CreateCommand();
            IDataReader reader;
            string query = "SELECT totalUsers FROM Stat WHERE date = '" + xLabels[i].text + "'";
            commandRead.CommandText = query;
            reader = commandRead.ExecuteReader();
            while (reader.Read())
            {
                columns[i].fillAmount = float.Parse(reader[0].ToString()) / maxValue;
            }
            dbcon.Close();
        }
    }
}
