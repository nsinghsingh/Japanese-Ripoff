using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;
using UnityEngine.UI;
using SimpleFileBrowser;
using UnityEditor;

public class ChangeCard : MonoBehaviour
{
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
    private int id;
    public GameObject popup;
    public InputField nameField;
    public InputField hpField;
    public InputField atkField;
    public InputField defField;
    public InputField rarityField;
    public InputField atkTextField;
    public InputField defTextField;
    public InputField supTextField;


    public void appear(int id)
    {
        popup.transform.localPosition = new Vector3(0, 0, 0);
        nameField.text = "";
        hpField.text = "";
        atkField.text = "";
        defField.text = "";
        rarityField.text = "";
        image = "";
        passives = "";
        this.id = id;
    }

    public void close()
    {
        popup.transform.localPosition = new Vector3(2000, 0, 0);
    }

    public void getPassives()
    {
        FileBrowser.SingleClickMode = true;
        FileBrowser.SetFilters(true, new FileBrowser.Filter("C# Scripts", ".cs"));
        FileBrowser.SetDefaultFilter(".cs");
        FileBrowser.ShowLoadDialog((path) => {
            passives = Path.GetFileNameWithoutExtension(path);
            string file = Path.GetFileName(path);
            if (!File.Exists(Application.dataPath + "/Resources/Passives/" + file))
            {
                path = path.Replace("\\", "/");
                FileUtil.CopyFileOrDirectory(path, Application.dataPath + "/Resources/Passives/" + file);
            }
        },
                                    () => {
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
            if (!File.Exists(Application.dataPath + "/Resources/Cards/" + file))
            {
                path = path.Replace("\\", "/");
                FileUtil.CopyFileOrDirectory(path, Application.dataPath + "/Resources/Cards/" + file);
            }
        },
                                        () => {
                                            image = "";
                                        },
                                        false, null, "Select File", "Select");
    }

    private void getValues(IDbConnection dbcon)
    {
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM Card WHERE id = '" + id + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        while (reader.Read())
        {
            if (nameField.text.Length > 0) { cardName = nameField.text; }
            else { cardName = reader[1].ToString(); }
            if (hpField.text.Length > 0) { hp = int.Parse(hpField.text); }
            else { hp = int.Parse(reader[2].ToString()); }
            if (atkField.text.Length > 0) { atk = int.Parse(atkField.text); }
            else { atk = int.Parse(reader[3].ToString()); }
            if (defField.text.Length > 0) { def = int.Parse(defField.text); }
            else { def = int.Parse(reader[4].ToString()); }
            if (rarityField.text.Length > 0) { rarity = rarityField.text; }
            else { rarity = reader[5].ToString(); }
            if (atkTextField.text.Length > 0) { atkText = atkTextField.text; }
            else { atkText = reader[6].ToString(); }
            if (defTextField.text.Length > 0) { defText = defTextField.text; }
            else { defText = reader[7].ToString(); }
            if (supTextField.text.Length > 0) { supText = supTextField.text; }
            else { supText = reader[8].ToString(); }
            if (passives.Length == 0) { passives = reader[9].ToString(); }
            if (image.Length == 0) { image = reader[10].ToString(); }
        }
    }

    public void changeCard()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        getValues(dbcon);
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "UPDATE Card SET name = '" + cardName + "', hp = '" + hp + "', atk = '" + atk + "', def = '" + def + "', rarity = '" + rarity + "', passives = '" + passives + "', image = '" + image + "', passiveATK = '" + atkText + "', passiveDEF = '" + defText + "', passiveSUP = '" + supText + "' WHERE id = '" + id + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        close();
        dbcon.Close();
    }

}
