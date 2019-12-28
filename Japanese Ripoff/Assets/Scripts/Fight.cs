using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fight : MonoBehaviour
{
    public Text[] passives = new Text[3];
    public GameObject cards;
    public GameObject[] allies = new GameObject[5];
    public GameObject[][] enemies;
    public GameObject[] cardSlots = new GameObject[3];
    public GameObject[] enemySlots = new GameObject[3];
    public Image enemyHP;
    public Image teamHP;
    public Text atkText;
    public Text defText;
    public Button go;
    public Canvas mainCanvas;
    public GameObject cross;
    public int attackBoost;
    public int defenseBoost;
    public int teamCurrentHP;
    public int enemyCurrentHP;
    public int currentAttack;
    public int currentDefense;
    private int enemyAttackBoost;
    private int enemyDefenseBoost;
    private int teamTotalHP = 0;
    private int enemyTotalHP = 0;
    private int currentWave = 0;


    // Start is called before the first frame update
    void Start()
    {
        setCards();
        setStats();
    }

    public void setCards()
    {
        for (int i = 0; i < allies.Length; i++)
        {
            try
            {
                allies[i] = GameObject.Find("CardsSelected").transform.GetChild(i).gameObject;
            }
            catch (System.Exception) { }
        }
        Destroy(GameObject.Find("CardsSelected"));
        foreach (GameObject ally in allies)
        {
            ally.GetComponent<Image>().enabled = true;
            ally.transform.SetParent(cards.transform);
            ally.AddComponent<CanvasGroup>();
            ally.AddComponent<DragAndDrop>();
            Destroy(ally.GetComponent<Button>());
            ally.GetComponent<DragAndDrop>().canvas = mainCanvas;
            ally.GetComponent<RectTransform>().sizeDelta = new Vector2(140, 140);
        }
    }

    public void setStats()
    {
        foreach (GameObject ally in allies)
        {
            Card cardInfo = ally.GetComponent<Card>();
            teamTotalHP += cardInfo.health;
            teamCurrentHP = teamTotalHP;
            teamHP.fillAmount = 1f;
        }
    }

    public void refreshStats()
    {
        if (teamCurrentHP > teamTotalHP)
        {
            teamCurrentHP = teamTotalHP;
        }
        teamHP.fillAmount = (float)teamCurrentHP / (float)teamTotalHP;
        if (enemyCurrentHP > enemyTotalHP)
        {
            enemyCurrentHP = enemyTotalHP;
        }
        enemyHP.fillAmount = (float)enemyCurrentHP / (float)enemyTotalHP;
        atkText.text = "ATK: " + currentAttack;
        defText.text = "DEF: " + currentDefense;
    }

    public void setEnemies()
    {
        System.Random random = new System.Random();
        int position = random.Next(enemySlots.Length);
        for (int i = 0; i < enemies[currentWave].Length; i++)
        {
            GameObject slot = enemySlots[i + position % enemySlots.Length];
            foreach  (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
            enemies[currentWave][i].transform.SetParent(slot.transform);
            enemies[currentWave][i].GetComponent<RectTransform>().localPosition = new Vector2(0, 50);
            enemyTotalHP += enemies[currentWave][i].GetComponent<Card>().health;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int attack = 0;
        int defense = 0;
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].transform.childCount > 0)
            {
                GameObject card = cardSlots[i].transform.GetChild(0).gameObject;
                Card cardInfo = card.GetComponent<Card>();
                passives[i].text = cardInfo.passivesText[i];
                if (i == 0) {
                    attack += cardInfo.attack; 
                }
                else if (i == 1) {
                    defense += cardInfo.defense;
                }
            }
            else
            {
                passives[i].text = "none";
            }
        }
        atkText.text = "ATK: " + attack;
        currentAttack = attack;
        defText.text = "DEF: " + defense;
        currentDefense = defense;
    }

    public void startRound()
    {
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].transform.childCount > 0)
            {
                GameObject card = cardSlots[i].transform.GetChild(0).gameObject;
                Card cardInfo = card.GetComponent<Card>();
                if (i == 0)
                {
                    cardInfo.attackPassive();
                }
                else if (i == 1)
                {
                    cardInfo.defensePassive();
                }
                else if(i == 2)
                {
                    cardInfo.supportPassive();
                }
            }
        }
        for (int i = 0; i < enemySlots.Length; i++)
        {
            if (enemySlots[i].transform.childCount > 0)
            {
                GameObject card = enemySlots[i].transform.GetChild(0).gameObject;
                Card cardInfo = card.GetComponent<Card>();
                if (i == 0)
                {
                    cardInfo.attackPassive();
                }
                else if (i == 1)
                {
                    cardInfo.defensePassive();
                }
                else if (i == 2)
                {
                    cardInfo.supportPassive();
                }
            }
        }
        alliesAttack();
    }

    public void alliesAttack()
    {
        GameObject defendingEnemy = enemySlots[1].transform.GetChild(0).gameObject;
        Card cardInfo = defendingEnemy.GetComponent<Card>();
        currentAttack = (currentAttack * attackBoost / 100) - (cardInfo.defense * enemyDefenseBoost / 100);
        if (currentAttack < 0) { currentAttack = 0; }
        enemyCurrentHP -= currentAttack;
        if (enemyCurrentHP < 0)
        {
            for (int i = 0; i < enemies[currentWave].Length; i++)
            {
                Destroy(enemies[currentWave][i]);
            }
            currentWave += 1;
            if (currentWave >= enemies.GetLength(0))
            {

            }
            else
            {
                setEnemies();
                refreshStats();
                
            }
        }
        else
        {
            refreshStats();
            enemiesAttack();
        }
    }

    public void enemiesAttack()
    {

    }
}
