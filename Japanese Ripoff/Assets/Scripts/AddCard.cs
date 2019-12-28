using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;
using SimpleFileBrowser;

public class AddCard : MonoBehaviour
{
    private int id;
    private string cardName;
    private int hp;
    private int atk;
    private int def;
    private string atkText;
    private string defText;
    private string supText;
    private string rarity;
    private string image;
    private string passives;
    public GameObject popup;
    public InputField[] fields;
    public string[] values;
    public bool[] hasValues;
    public Button add;

    public void appear()
    {
        popup.transform.localPosition = new Vector3(0, 0, 0);
        fields[0].text = "";
        fields[0].onValueChanged.AddListener(delegate { requireValue(fields[0], 0, values[0]); });
        fields[1].text = "";
        fields[1].onValueChanged.AddListener(delegate { requireValue(fields[1], 1, values[1]); });
        fields[2].text = "";
        fields[2].onValueChanged.AddListener(delegate { requireValue(fields[2], 2, values[2]); });
        fields[3].text = "";
        fields[3].onValueChanged.AddListener(delegate { requireValue(fields[3], 3, values[3]); });
        fields[4].text = "";
        fields[4].onValueChanged.AddListener(delegate { requireValue(fields[4], 4, values[4]); });
        fields[5].text = "";
        fields[5].onValueChanged.AddListener(delegate { requireValue(fields[5], 5, values[5]); });
        fields[6].text = "";
        fields[6].onValueChanged.AddListener(delegate { requireValue(fields[6], 6, values[6]); });
        fields[7].text = "";
        fields[7].onValueChanged.AddListener(delegate { requireValue(fields[7], 7, values[7]); });
        hasValues = new bool[] { false, false, false, false, false, false, false, false, false, false };
        add.interactable = false;
    }

    public void close()
    {
        popup.transform.localPosition = new Vector3(2000, 0, 0);
        foreach (InputField field in fields)
        {
            field.onValueChanged.RemoveAllListeners();
        }
    }

    private void requireValue(InputField field, int index, string value)
    {
        
        if (field.text.Length > 0)
        {
            hasValues[index] = true;
            field.GetComponent<Image>().color = new Color(255, 255, 255);
            field.placeholder.GetComponent<Text>().text = "Enter a " + value + "...";
        }
        else
        {
            hasValues[index] = false;
            field.GetComponent<Image>().color = new Color(255, 0, 0);
            field.placeholder.GetComponent<Text>().text = "A " + value + " is required";
        }
        hasAllValues();
    }

    public void getPassives()
    {
        FileBrowser.SingleClickMode = true;
        FileBrowser.SetFilters(true, new FileBrowser.Filter("C# Scripts", ".cs"));
        FileBrowser.SetDefaultFilter(".cs");
        FileBrowser.ShowLoadDialog((path) => {
            passives = Path.GetFileNameWithoutExtension(path);
            string file = Path.GetFileName(path);
            hasValues[8] = true;
            if (!File.Exists(Application.dataPath + "/Resources/Passives/" + file))
            {
                path = path.Replace("\\", "/");
                File.Copy(path, Application.dataPath + "/Resources/Passives/" + file);
            }
            hasAllValues();
        },
                                    () => {
                                        hasValues[8] = false;
                                        passives = "";
                                    },
                                    false, null, "Select File", "Select");
    }

    public void getImage()
    {
        FileBrowser.SingleClickMode = true;
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetDefaultFilter(".jpg");
        FileBrowser.ShowLoadDialog((path) => {
            image = Path.GetFileNameWithoutExtension(path);
            string file = Path.GetFileName(path);
            hasValues[9] = true;
            if (!File.Exists(Application.dataPath + "/Resources/Cards/" + file))
            {
                path = path.Replace("\\", "/");
                File.Copy(path, Application.dataPath + "/Resources/Cards/" + file);
            }
            hasAllValues();
        },
                                        () => {
                                            image = "";
                                            hasValues[9] = false;
                                        },
                                        false, null, "Select File", "Select");
    }

    private void hasAllValues()
    {
        bool hasFalse = false;
        foreach (bool hasValue in hasValues)
        {
            if (!hasValue)
            {
                hasFalse = true;
            }
        }
        if (!hasFalse)
        {
            add.interactable = true;
        }
        else { add.interactable = false; }
    }

    public void addCard()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        getValues(dbcon);
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "INSERT INTO Card (id, name, hp, atk, def, rarity, passives, image, passiveATK, passiveDEF, passiveSUP) VALUES ('" + id + "', '" + cardName + "', '" + hp + "', '" + atk + "', '" + def + "', '" + rarity + "', '" + passives + "', '" + image + "', '" + atkText + "', '" + defText + "', '" + supText + "')";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        dbcon.Close();
        close();
    }

    private void getValues(IDbConnection dbcon)
    {
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT MAX(id) FROM Card";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        while (reader.Read())
        {
            id = int.Parse(reader[0].ToString()) + 1;
        }
        cardName = fields[0].text;
        hp = int.Parse(fields[1].text);
        atk = int.Parse(fields[2].text);
        def = int.Parse(fields[3].text);
        rarity = fields[4].text;
        atkText = fields[5].text;
        defText = fields[6].text;
        supText = fields[7].text;
    }
}
