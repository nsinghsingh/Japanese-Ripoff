using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;

public class DeleteCard : MonoBehaviour
{
    public GameObject popup;
    private int id;

    public void appear(int id)
    {
        popup.transform.localPosition = new Vector3(0, 0, 0);
        this.id = id;
    }

    public void close()
    {
        popup.transform.localPosition = new Vector3(2000, 0, 0);
    }

    public void delete()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "DELETE FROM Card WHERE id = '" + id + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        dbcon.Close();
        close();
    }
}
