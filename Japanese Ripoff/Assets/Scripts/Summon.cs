using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Summon : MonoBehaviour
{
    public int amount;
    public GameObject grid;
    public Inventory inventory;
    public GameObject summoning;
    public GameObject header;
    public GameObject footer;
    public GameObject displayedCard;
    private PlayerData playerData;
    private List<GameObject> obtainedCards = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.playerData;
    }
    
    private GameObject getUnit()
    {
        GameObject card = new GameObject();
        card.AddComponent<Card>();
        card.AddComponent<Image>();
        card.AddComponent<Button>();
        PlayerData.copyCardInfo(card, playerData.cards[0]);
        Card cardInfo = card.GetComponent<Card>();
        card.name = cardInfo.cardName;
        return card;
    }

    public void summon()
    {
        header.SetActive(false);
        footer.SetActive(false);
        amount -= 1;
        if (amount >= 0)
        {
            displayedCard.SetActive(true);
            obtainedCards.Add(getUnit());
            PlayerData.copyCardInfo(displayedCard, playerData.cards[0]);
            Card cardInfo = displayedCard.GetComponent<Card>();
            Image profile = displayedCard.GetComponent<Image>();
            profile.sprite = cardInfo.profile;
            displayedCard.name = cardInfo.cardName;
            Button button = displayedCard.GetComponent<Button>();
            button.onClick.AddListener(delegate () { summon(); });
        }
        else
        {
            displayedCard.SetActive(false);
            foreach (GameObject card in obtainedCards)
            {
                card.transform.SetParent(grid.transform);
                Card cardInfo = card.GetComponent<Card>();
                Image profile = card.GetComponent<Image>();
                profile.sprite = cardInfo.profile;
                Button button = card.GetComponent<Button>();
                button.onClick.AddListener(delegate () { inventory.seeCardInfo(cardInfo); });
                card.name = cardInfo.cardName;
                card.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                header.SetActive(true);
                footer.SetActive(true);
            }
        }
    }

    public void exitSummon()
    {
        amount = obtainedCards.Count;
        for (int i = 0; i < obtainedCards.Count; i++)
        {
            GameObject card = new GameObject();
            card.AddComponent<Card>();
            PlayerData.copyCardInfo(card, obtainedCards[i]);
            Card cardInfo = card.GetComponent<Card>();
            card.name = cardInfo.cardName;
            playerData.cards.Add(card);
            DontDestroyOnLoad(card);
            Destroy(obtainedCards[i]);
        }
        obtainedCards.Clear();

    }
}
