using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shortcuts : MonoBehaviour
{
    public GameObject shortcut;
    public GameObject[] panels;
    public GameObject[] cardLinks;
    public Logout logout;
    public CardScreen inventory;
    public Summon[] summon;
    public Settings settings;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            shortcut.SetActive(true);
            gameObject.GetComponent<Text>().enabled = false;
        }
        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            shortcut.SetActive(false);
            gameObject.GetComponent<Text>().enabled = true;
        }
        if (shortcut.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                exitCurrentScreen("h");
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                exitCurrentScreen("i");
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                exitCurrentScreen("g");
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                exitCurrentScreen("s");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                exitCurrentScreen("l");
            }
        }
    }

    public void exitCurrentScreen(string letter)
    {
        int index = 0;
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].activeSelf)
            {
                index = i;
            }
        }
        if(index == 0) //home
        {
            panels[index].SetActive(false);
            enterScreen(letter);
        }
        else if (index == 1) //inventory
        {
            inventory.exitInventory();
            panels[index].SetActive(false);
            enterScreen(letter);
        }
        else if (index == 2) //gacha
        {
            panels[index].SetActive(false);
            enterScreen(letter);
        }
        else if (index == 3) //cardInfo
        {
            foreach (GameObject link in cardLinks)
            {
                link.SetActive(false);
            }
            panels[index].SetActive(false);
            enterScreen(letter);
        }
        else if (index == 4) //summon
        {
            if(summon[0].amount == 0 || summon[1].amount == 0)
            {
                panels[index].SetActive(false);
                enterScreen(letter);
            }
        }
    }

    public void enterScreen(string letter)
    {
        if (letter.Equals("h"))
        {
            panels[0].SetActive(true);
        }
        else if (letter.Equals("i"))
        {
            inventory.enterInventory();
            panels[1].SetActive(true);
        }
        else if (letter.Equals("g"))
        {
            panels[2].SetActive(true);
        }
        else if (letter.Equals("s"))
        {
            settings.loadSettings();
        }
        else if (letter.Equals("l"))
        {
            logout.logout();
        }
    }
}
