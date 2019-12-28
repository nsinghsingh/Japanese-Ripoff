using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;

public class Home : MonoBehaviour
{
    public GameObject storyStages;
    public GameObject trainingStages;
    public GameObject bossStages;
    public GameObject stage;
    public GameObject home;
    public GameObject select;
    public Button play;
    private PlayerData playerData;

    void Start()
    {
        playerData = PlayerData.playerData;
        getStages();
    }

    private void getStages()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM NumberOfStagesPerUser LEFT JOIN Stage ON sceneIndex = fkStage WHERE fkUser = '" + playerData.user + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        bool hasStages = false;
        while (reader.Read())
        {
            GameObject stage = Instantiate(this.stage, new Vector2(0, 0), Quaternion.identity);
            hasStages = true;
            if (reader[5].ToString().Equals("story"))
            {
                stage.transform.SetParent(storyStages.transform);
            }
            else if (reader[5].ToString().Equals("training"))
            {
                stage.transform.SetParent(trainingStages.transform);
            }
            else if (reader[5].ToString().Equals("boss"))
            {
                stage.transform.SetParent(bossStages.transform);
            }
            stage.transform.localScale = new Vector3(1, 1, 1);
            LoadStage scene = stage.GetComponent<LoadStage>();
            scene.sceneIndex = int.Parse(reader[2].ToString());
            scene.stageName = reader[3].ToString();
            scene.neededStamina = int.Parse(reader[4].ToString());
            scene.text = stage.GetComponentInChildren<Text>();
            scene.play = play;
            Button button = stage.GetComponent<Button>();
            button.onClick.AddListener(delegate () { home.SetActive(false); });
            button.onClick.AddListener(delegate () { select.SetActive(true); });
            Select selection = select.GetComponent<Select>();
            button.onClick.AddListener(delegate () { selection.resetSelection(); });
            button.onClick.AddListener(delegate () { scene.loadSelect(); });
        }
        if (!hasStages)
        {
            commandRead = dbcon.CreateCommand();
            query = "INSERT INTO NumberOfStagesPerUser (fkStage, fkUser) VALUES ('4', '" + playerData.user + "')";
            commandRead.CommandText = query;
            reader = commandRead.ExecuteReader();
            getStages();
        }
        dbcon.Close();
    }
}
