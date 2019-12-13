using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public GameObject icon;
    public string help = "";
    public Font font;
    private GameObject tooltip;


    public void makeBubble()
    {
        Vector2 location = icon.transform.position;
        location.x += 400;
        tooltip = new GameObject();
        tooltip.transform.parent = icon.transform;
        tooltip.transform.position = location;
        tooltip.AddComponent<Text>();
        Text message = tooltip.GetComponent<Text>();
        message.text = help;
        message.fontSize = 32;
        message.font = font;
        message.color = new Color(0, 0, 0);
        message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
    }

    public void destroyBubble()
    {
        Destroy(tooltip);
    }
}
