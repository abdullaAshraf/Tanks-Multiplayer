using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class FixedJoystick : Joystick
{
    Vector2 joystickPosition = Vector2.zero;
    private Camera cam = new Camera();
    public List<PointerEventData> touchs;


    void Start()
    {
        touchs = new List<PointerEventData>();
        joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickPosition;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        touchs.Add(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine(Release(eventData, 0.01f));
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    IEnumerator Release(PointerEventData eventData, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        touchs.Remove(eventData);
    }
}