using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private PlayerData playerData;
    private List<GameObject> cards = new List<GameObject>();
    public GameObject cardGrid;
    public List<GameObject> cardsDisplayed = new List<GameObject>();
    public GameObject inventoryScreen;
    public GameObject cardInfoScreen;
    public Text[] info = new Text[7];
    public Image rarity;
    public Image profile;

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.playerData;
        cards = playerData.cards;
        enterInventory();
    }

    public void enterInventory()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject card = new GameObject();
            card.AddComponent<Card>();
            card.AddComponent<Button>();
            card.AddComponent<Image>();
            card.transform.SetParent(cardGrid.transform);
            Card cardInfo = cards[i].GetComponent<Card>();
            Image profile = card.GetComponent<Image>();
            profile.sprite = cardInfo.profile;
            Button button = card.GetComponent<Button>();
            button.onClick.AddListener(delegate () { seeCardInfo(cardInfo); });
            card.name = cardInfo.cardName;
            cardsDisplayed.Add(card);
            card.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
        }
    }

    public void exitInventory()
    {
        for (int i = 0; i < cardsDisplayed.Count; i++)
        {
            Card cardInfo = cardsDisplayed[i].GetComponent<Card>();
            Destroy(cardsDisplayed[i]);
        }
        cardsDisplayed.Clear();
    }

    public void seeCardInfo(Card cardInfo)
    {
        info[0].text = cardInfo.cardName;
        info[1].text = cardInfo.health.ToString();
        info[2].text = cardInfo.attack.ToString();
        info[3].text = cardInfo.defense.ToString();
        info[4].text = cardInfo.passivesText[0];
        info[5].text = cardInfo.passivesText[1];
        info[6].text = cardInfo.passivesText[2];
        rarity.sprite = cardInfo.rarityIcon;
        profile.sprite = cardInfo.profile;
        exitInventory();
        inventoryScreen.SetActive(false);
        cardInfoScreen.SetActive(true);
    }
}
