using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRow : MonoBehaviour
{
    public GameObject changeCard;
    public GameObject deleteCard;
    public Text cardName;
    public Text rarity;
    public Text hp;
    public Text atk;
    public Text def;
    public Text passives;
    public Button update;
    public Button delete;

    public void setRow(int id, string cardNameValue, string rarityValue, string hpValue, string atkValue, string defValue, string passivesValue, GameObject changer, GameObject deleter)
    {
        cardName.text = cardNameValue;
        rarity.text = rarityValue;
        hp.text = hpValue;
        atk.text = atkValue;
        def.text = defValue;
        passives.text = passivesValue;
        deleteCard = deleter;
        changeCard = changer;
        update.onClick.AddListener(delegate () { changeCard.GetComponent<ChangeCard>().appear(id); });
        delete.onClick.AddListener(delegate () { deleteCard.GetComponent<DeleteCard>().appear(id); });
    }
}
