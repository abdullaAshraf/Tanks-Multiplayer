using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreboardItem : MonoBehaviour {
    [SerializeField]
    Text usernameText;

    [SerializeField]
    Text killsText;

    [SerializeField]
    Text deathsText;

    [SerializeField]
    Text scoreText;

    public void Setup(string username , int score , int kills , int deaths)
    {
        usernameText.text = username;
        killsText.text = kills.ToString();
        deathsText.text = deaths.ToString();
        scoreText.text = score.ToString();
    }
}
