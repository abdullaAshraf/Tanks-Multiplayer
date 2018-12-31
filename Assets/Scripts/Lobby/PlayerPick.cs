using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPick : MonoBehaviour {

    [SerializeField]
    private Image turretImage;

    [SerializeField]
    private Image hullImage;

    [SerializeField]
    private Image abilityImage;

    [SerializeField]
    private Image paintImage;

    public void ChangeTurret(Dropdown change)
    {
        
        turretImage.sprite = Resources.Load<Sprite>("Sprites/turret/" + change.options[change.value].text);
    }

    public void ChangeHull(Dropdown change)
    {
        hullImage.sprite = Resources.Load<Sprite>("Sprites/hull/" + change.options[change.value].text);
    }

    public void ChangeAbility(Dropdown change)
    {
        abilityImage.sprite = Resources.Load<Sprite>("Sprites/ability/" + change.options[change.value].text);
    }

    public void ChangePaint(Dropdown change)
    {
        //TODO handle change paint
    }
}
