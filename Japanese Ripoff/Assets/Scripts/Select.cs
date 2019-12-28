using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Select : MonoBehaviour
{
    public GameObject cardGrid;
    public GameObject selectionGrid;
    public CardScreen cards;
    public GameObject[] selectedCards = new GameObject[5];

    private void setSelection()
    {
        foreach (Transform child in cardGrid.transform)
        {
            GameObject card = child.gameObject;
            card.AddComponent<LongClick>();
            LongClick hold = card.GetComponent<LongClick>();
            hold.requiredHoldTime = 0.2f;
            hold.onLongClick = new UnityEvent();
            hold.onLongClick.AddListener(delegate () {
                selectCard(card);
            });
        }
    }

    private void selectCard(GameObject card)
    {
        for (int i = 0; i < selectedCards.Length; i++)
        {
            if (selectedCards[i] == null)
            {
                selectedCards[i] = card;
                card.transform.SetParent(selectionGrid.transform);
                Destroy(selectionGrid.transform.GetChild(i).gameObject);
                card.transform.SetSiblingIndex(i);
                Destroy(card.GetComponent<LongClick>());
                break;
            }
        }
    }

    public void saveSelectedCards()
    {
        GameObject cards = new GameObject("CardsSelected");
        DontDestroyOnLoad(cards);
        for (int i = 0; i < selectedCards.Length; i++)
        {
            if (selectedCards[i] != null)
            {
                GameObject card = new GameObject();
                card.AddComponent<Card>();
                card.AddComponent<Image>();
                PlayerData.copyCardInfo(card, selectedCards[i]);
                Card cardInfo = card.GetComponent<Card>();
                Image image = card.GetComponent<Image>();
                card.name = cardInfo.cardName;
                image.sprite = cardInfo.profile;
                card.transform.SetParent(cards.transform);
            }
        }
    }

    public void resetSelection()
    {
        foreach (Transform child in selectionGrid.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
            GameObject blank = new GameObject();
            blank.AddComponent<Image>();
            blank.transform.SetParent(selectionGrid.transform);
            blank.transform.localScale = new Vector3(1, 1, 1);
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 180);
        }
        cards.exitInventory();
        cards.enterInventory();
        setSelection();
    }

}
