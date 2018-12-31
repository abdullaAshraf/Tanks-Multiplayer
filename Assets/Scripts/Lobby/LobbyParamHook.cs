using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


namespace Prototype.NetworkLobby {

    public class LobbyParamHook : LobbyHook {
        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);
            LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
            Player player = gamePlayer.GetComponent<Player>();

            player.playerName = lobby.playerName;
            player.playerColor = lobby.playerColor;
            player.playerAbility = lobby.playerAbility;
            player.playerTurret = lobby.playerTurret;
            player.playerHull = lobby.playerHull;
            player.matchSettings = lobby.matchSettings;
        }
    }
}