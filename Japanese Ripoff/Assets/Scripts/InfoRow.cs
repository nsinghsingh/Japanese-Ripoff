using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoRow : MonoBehaviour
{
    public GameObject changeUser;
    public GameObject deleteUser;
    public Text username;
    public Text level;
    public Text stamina;
    public Text exp;
    public Text money;
    public Button update;
    public Button delete;

    public void setRow(string usernameValue, string levelValue, string staminaValue, string expValue, string moneyValue, GameObject changer, GameObject deleter)
    {
        username.text = usernameValue;
        level.text = levelValue;
        stamina.text = staminaValue;
        exp.text = expValue;
        money.text = moneyValue;
        deleteUser = deleter;
        changeUser = changer;
        update.onClick.AddListener(delegate () { changeUser.GetComponent<ChangeUser>().appear(usernameValue); });
        delete.onClick.AddListener(delegate () { deleteUser.GetComponent<DeleteUser>().appear(usernameValue); });
    }
}
