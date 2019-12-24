using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;

public class Summon : MonoBehaviour
{
    public int amount;
    public int price;
    public GameObject grid;
    public Inventory inventory;
    public GameObject summoning;
    public GameObject header;
    public GameObject footer;
    public GameObject displayedCard;
    public int[] chances = new int[3];
    private PlayerData playerData;
    private List<GameObject> obtainedCards = new List<GameObject>();
    private int maxCards;
    private string rarity;

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.playerData;
    }
    
    public void checkSummon()
    {
        if (playerData.money >= amount * price)
        {
            playerData.money -= amount * price;
            summon();
        }
        else
        {
            // TO DO Show popup
        }
    }

    private GameObject getUnit()
    {
        GameObject randomCard = new GameObject();
        randomCard.AddComponent<Card>();
        System.Random random = new System.Random();
        string connection = "URI=file:" + Application.persistentDataPath + "/main";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        chooseRarity(random.Next(101), dbcon);
        getCard(dbcon, random, randomCard);
        dbcon.Close();
        return randomCard;
    }

    private void chooseRarity(int chance, IDbConnection dbcon)
    {
        if (chance < chances[0]) { rarity = "n"; }
        else if (chance < chances[1]) { rarity = "r"; }
        else if (chance < chances[2]) { rarity = "sr"; }
        else if (chance <= 100) { rarity = "ssr"; }
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT COUNT(*) FROM Card WHERE rarity = '" + rarity + "'";
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        while (reader.Read())
        {
            maxCards = int.Parse(reader[0].ToString());
        }
    }

    private void getCard(IDbConnection dbcon, System.Random random, GameObject card)
    {
        IDbCommand commandRead = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM Card WHERE rarity = '" + rarity + "'";
        commandRead = dbcon.CreateCommand();
        commandRead.CommandText = query;
        reader = commandRead.ExecuteReader();
        int count = 1;
        int chosenCard = random.Next(1, maxCards + 1);
        while (reader.Read())
        {
            if (count == chosenCard)
            {
                Card cardInfo = card.GetComponent<Card>();
                cardInfo.id = int.Parse(reader[0].ToString());
                cardInfo.cardName = reader[1].ToString();
                card.name = cardInfo.cardName;
                cardInfo.health = int.Parse(reader[2].ToString());
                cardInfo.attack = int.Parse(reader[3].ToString());
                cardInfo.defense = int.Parse(reader[4].ToString());
                cardInfo.rarity = reader[5].ToString();
                cardInfo.passivesText = new string[] { reader[6].ToString(), reader[7].ToString(), reader[8].ToString() };
                card.AddComponent(Type.GetType(reader[9].ToString()));
                cardInfo.Passives = card.GetComponent<Passive>();
                cardInfo.profile = Resources.Load<Sprite>("Cards/" + reader[10].ToString());
                if (cardInfo.rarity.Equals("ssr"))
                {
                    cardInfo.rarityIcon = Resources.Load<Sprite>("Icons/SuperSuperRareRarity");
                }
                else if (cardInfo.rarity.Equals("sr"))
                {
                    cardInfo.rarityIcon = Resources.Load<Sprite>("Icons/SuperRareRarity");
                }
                else if (cardInfo.rarity.Equals("r"))
                {
                    cardInfo.rarityIcon = Resources.Load<Sprite>("Icons/RareRarity");
                }
                else
                {
                    cardInfo.rarityIcon = Resources.Load<Sprite>("Icons/NormalRarity");
                }
                card.AddComponent<Image>();
                card.AddComponent<Button>();
            }
            count += 1;
        }
    }

    private void summon()
    {
        
        header.SetActive(false);
        footer.SetActive(false);
        amount -= 1;
        if (amount >= 0)
        {
            displayedCard.SetActive(true);
            obtainedCards.Add(getUnit());
            PlayerData.copyCardInfo(displayedCard, obtainedCards[obtainedCards.Count - 1]);
            Card cardInfo = displayedCard.GetComponent<Card>();
            Image profile = displayedCard.GetComponent<Image>();
            profile.sprite = cardInfo.profile;
            displayedCard.name = cardInfo.cardName;
            Button button = displayedCard.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
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
