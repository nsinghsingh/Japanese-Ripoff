using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;

public class DeleteUser : MonoBehaviour
{
    public GameObject popup;
    private string username;

    public void appear(string username)
    {
        popup.transform.localPosition = new Vector3(0, 0, 0);
        this.username = username;
    }

    public void close()
    {
        popup.transform.localPosition = new Vector3(2000, 0, 0);
    }

    public void delete()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        //C:\Users\nihal\AppData\LocalLow\DefaultCompany\Japanese Ripoff
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "DELETE FROM User WHERE name = '" + username + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        close();
    }
}
