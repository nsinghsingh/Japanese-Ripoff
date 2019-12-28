using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown;
    private float pointerDownTimer;
    public float requiredHoldTime;
    public UnityEvent onLongClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
        pointerDownTimer = 0;
    }

    private void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer > requiredHoldTime && onLongClick != null)
            {
                onLongClick.Invoke();
                pointerDown = false;
                pointerDownTimer = 0;
            }
        }
    }
}
