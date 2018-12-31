using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Scoreboard : MonoBehaviour {

    [SerializeField]
    GameObject playerScoreboardItem;

    [SerializeField]
    Transform playerScoreboardList;


    private void OnEnable()
    {
        Player[] players = GameManger.GetAllPlayers();
        Array.Sort(players, ComparePlayers);

        foreach(Player player in players)
        {
            GameObject itemGO = Instantiate(playerScoreboardItem , playerScoreboardList) as GameObject;
            PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
            if(item != null)
            {
                item.Setup(player.playerName, player.score, player.kills, player.deaths);
            }
            if (player.isLocalPlayer)
                item.GetComponent<Image>().color = Color.cyan;
        }
    }

    private void OnDisable()
    {
        foreach(Transform child in playerScoreboardList)
        {
            Destroy(child.gameObject);
        }
    }


    public static int ComparePlayers(Player p1, Player p2)
    {
        if (p1.score > p2.score)
            return -1;
        else if (p1.score == p2.score)
            return 0;
        else
            return 1;
    }
}
