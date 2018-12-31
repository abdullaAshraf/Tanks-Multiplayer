using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    [SyncVar]
    private bool _isDead;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    public float maxHealth = 100;

    [SyncVar]
    private float currHealth;

    public Image healthDisplay;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    private Transform pivotPoint;
    private Transform turret;
    private Transform canvas;

    [HideInInspector]
    public int score, kills, deaths;

    [SyncVar]
    public string playerName;
    [SyncVar]
    public Color playerColor;

    private bool firstSetup = true;

    [SerializeField]
    private GameObject deahtEffect;

    private void Start()
    {
        pivotPoint = transform.Find("CenterPoint");
        canvas = transform.Find("Canvas");
        turret = pivotPoint.Find("Turret");

        Text playerNameText = canvas.Find("PlayerName").GetComponent<Text>();
        playerNameText.text = playerName;
        playerNameText.color = playerColor;
        score = kills = deaths = 0;
    }

    public void Setup()
    {
        CmdBroadCaseNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCaseNewPlayerSetup()
    {
        /*
        Debug.Log(playerName);
        for (int i = 0; i < NetworkServer.connections.Count; i++)
        {
            Debug.Log(NetworkServer.connections[i].isConnected);
        }
        */
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }
        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;
        currHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider2D _col = GetComponent<Collider2D>();
        if (_col != null)
            _col.enabled = true;

        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        if (_sprite != null)
            _sprite.enabled = true;

        SpriteRenderer _sprite2 = turret.gameObject.GetComponent<SpriteRenderer>();
        if (_sprite2 != null)
            _sprite2.enabled = true;

        canvas.GetComponent<Canvas>().enabled = true;
    }

    [ClientRpc]
    public void RpcTakeDamage(float _amount , string dealer)
    {
        if (isDead)
            return;

        currHealth -= _amount;

        if (currHealth <= 0)
        {
            currHealth = 0;
            Die(dealer);
            //RpcDied();
            return;
        }
    }

    private void Die(string dealer_id)
    {
        //Calculate scores
        deaths++;
        Player dealer = GameManger.GetPlayer(dealer_id);
        if (dealer != null)
        {
            dealer.kills++;
            dealer.score += GameManger.instance.matchSettings.killScore;
            GameManger.instance.onPlayerKilledCallback.Invoke(playerName, dealer.playerName);
        }
        
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Collider2D _col = GetComponent<Collider2D>();
        if (_col != null)
            _col.enabled = false;

        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        if (_sprite != null)
            _sprite.enabled = false;

        SpriteRenderer _sprite2 = turret.gameObject.GetComponent<SpriteRenderer>();
        if (_sprite2 != null)
            _sprite2.enabled = false;

        canvas.GetComponent<Canvas>().enabled = false;

        GameObject _graphicsInstance = (GameObject) Instantiate(deahtEffect, transform.position, Quaternion.identity);
        Destroy(_graphicsInstance, 3f);

        Debug.Log(transform.name + " is DEAD!");

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManger.instance.matchSettings.respawnTime);

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        Transform pivotPoint = transform.Find("CenterPoint");
        pivotPoint.localEulerAngles = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.1f);

        Setup();
    }

    private void Update()
    {
        healthDisplay.fillAmount = currHealth / maxHealth;

        /*
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.K))
            RpcTakeDamage(9999);
            */
            
    }
}
