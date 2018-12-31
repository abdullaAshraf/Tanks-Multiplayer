using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillsFeed : MonoBehaviour {

    [SerializeField]
    GameObject killsFeedItem;

    [SerializeField]
    Transform killsFeedList;

    void Start () {
        GameManger.instance.onPlayerKilledCallback += OnKill;
	}

    public void OnKill(string player, string source)
    {
        GameObject itemGO = Instantiate(killsFeedItem, killsFeedList) as GameObject;
        KillsFeedItem item = itemGO.GetComponent<KillsFeedItem>();
        if (item != null)
        {
            item.Setup("<b>" + source + "</b>" + " eliminated " + "<b>" + player + "</b>");
        }

        Destroy(itemGO, 4f);
    }

}
