using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;

public class UsersPerLevelGraph : MonoBehaviour
{
    public Text[] xLabels = new Text[4];
    public Text[] yLabels = new Text[4];
    public Image[] columns = new Image[4];
    private int[] values = new int[4];

    public void loadStats()
    {
        setX();
        setValues();
        setY();
        setColumns();
    }

    private void setX()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT MAX(level) FROM User";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        float maxValue = 100;
        while (reader.Read())
        {
            maxValue = int.Parse(reader[0].ToString());
        }
        dbcon.Close();
        for (int i = 0; i < xLabels.Length; i++)
        {
            xLabels[i].text = ((int)(maxValue / xLabels.Length * (xLabels.Length - i))).ToString();
        }
    }

    private void setValues()
    {
        int recordedUsers = 0;
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        for (int i = xLabels.Length-1; i >= 0; i--)
        {
            IDbCommand commandRead = dbcon.CreateCommand();
            IDataReader reader;
            string query = "SELECT COUNT(*) FROM User WHERE level <= '" + int.Parse(xLabels[i].text) + "'";
            commandRead.CommandText = query;
            reader = commandRead.ExecuteReader();
            while (reader.Read())
            {
                values[i] = (int.Parse(reader[0].ToString()) - recordedUsers);
                recordedUsers += values[i];
            }
        }
        dbcon.Close();
    }

    private void setY()
    {
        float maxValue = 0;
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] > maxValue)
            {
                maxValue = values[i];
            }
        }
        for (int i = 0; i < yLabels.Length; i++)
        {
            yLabels[i].text = ((int)(maxValue / yLabels.Length * (yLabels.Length - i))).ToString();
        }
    }

    private void setColumns()
    {
        float maxValue = int.Parse(yLabels[0].text);
        for (int i = 0; i < columns.Length; i++)
        {
            columns[i].fillAmount = (float)values[i] / maxValue;
        }
    }

}
