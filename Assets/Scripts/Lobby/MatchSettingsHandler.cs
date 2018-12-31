using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MatchSettingsHandler : MonoBehaviour {

    [SerializeField]
    public MatchSettings matchSettings;

    [SerializeField]
    private Image mapImage;

    private void Start()
    {
        //Load maps and options
    }

    public void ChangeMap(Dropdown change)
    {
        matchSettings.mapName = change.options[change.value].text;
        mapImage.sprite = Resources.Load<Sprite>("Sprites/map/" + change.options[change.value].text);
    }

    public void ChangeType(Dropdown change)
    {
        matchSettings.matchType = change.options[change.value].text;
    }

    public void ChangeTime(Dropdown change)
    {
        switch (change.value)
        {
            case 0:
                matchSettings.matchTime = 3 * 60;
                break;
            case 1:
                matchSettings.matchTime = 5 * 60;
                break;
            case 2:
                matchSettings.matchTime = 10 * 60;
                break;
            case 3:
                matchSettings.matchTime = 20 * 60;
                break;
            case 4:
                matchSettings.matchTime = 30 * 60;
                break;
            case 5:
                matchSettings.matchTime = 1000 * 60;
                break;
        }
    }

    public void ChangeScore(Dropdown change)
    {
        switch (change.value)
        {
            case 0:
                matchSettings.ScoreLimit = 20;
                break;
            case 1:
                matchSettings.ScoreLimit = 100;
                break;
            case 2:
                matchSettings.ScoreLimit = 500;
                break;
            case 3:
                matchSettings.ScoreLimit = 1000;
                break;
            case 4:
                matchSettings.ScoreLimit = 5000;
                break;
            case 5:
                matchSettings.ScoreLimit = 1000000;
                break;
        }
    }



}
