using UnityEngine;
using UnityEngine.UI;

public class KillsFeedItem : MonoBehaviour {

    [SerializeField]
    Text killsFeedText;

    public void Setup(string text)
    {
        killsFeedText.text = text;
    }
}
