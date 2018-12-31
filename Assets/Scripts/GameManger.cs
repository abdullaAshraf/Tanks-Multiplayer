using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.Collections;

public class GameManger : NetworkBehaviour
{

    public static GameManger instance;

    public MatchSettings matchSettings;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject scoreboard;

    [SerializeField]
    Text timeText;

    [SerializeField]
    Text informationText;

    private int secondsLeft;

    bool gameOver = false;

    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManger in scene.");
        }
        instance = this;
        PauseMenu.isOn = false;
        gameOver = false;
        pauseMenu.SetActive(false);
    }

    public void setMatchSettigns(MatchSettings ms)
    {
        matchSettings = ms;
        secondsLeft = matchSettings.matchTime;
        if (secondsLeft >= 1000 * 60)
            timeText.enabled = false;
        else
            InvokeRepeating("ClockTime", 0, 1f);
    }

    private void ClockTime()
    {
        if (--secondsLeft == 0)
        {
            CancelInvoke();
            GameOver();
        }
        if (secondsLeft == 10)
            timeText.color = Color.red;
        string m = (secondsLeft / 60).ToString();
        if (m.Length == 1)
            m = "0" + m;
        string s = (secondsLeft % 60).ToString();
        if (s.Length == 1)
            s = "0" + s;
        timeText.text = m + ":" + s;
    }

    public void GameOver()
    {
        if (gameOver)
            return;
        gameOver = true;
        scoreboard.SetActive(true);
        StartCoroutine(PauseGame(5f));
    }

    public IEnumerator PauseGame(float pauseTime)
    {
        Time.timeScale = 0f;
        float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1f;
        EndGame();
    }

    public void EndGame()
    {
        NetworkLobbyManager networkLobbyManager = GameObject.FindObjectOfType<NetworkLobbyManager>();
        networkLobbyManager.ServerReturnToLobby();
        //networkLobbyManager.StopServer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //TogglePauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }

        foreach (Player player in players.Values.ToArray())
        {
            if (player.score >= matchSettings.ScoreLimit)
                GameOver();
        }
    }

    public void ToggleStandings()
    {
        scoreboard.SetActive(!scoreboard.active);
    }

    void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }
    #region Player tracking

    private const string PLAYER_DI_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    private static string localPlayer = "player";

    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_DI_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;          
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static void SetLocalPlayer(string _player)
    {
        localPlayer = _player;
        //GameManger.instance.informationText.text = _player;
    }


    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    public static Player GetLocalPlayer()
    {
        if (players.ContainsKey(localPlayer))
            return players[localPlayer];

        Debug.LogError("no main player!");
        return null;
    }

    public static Player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

    /*
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200,200,200,500));
        GUILayout.BeginVertical();

        foreach(string _player in players.Keys)
        {
            GUILayout.Label(_player + " - " + players[_player].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    */

    #endregion
}
