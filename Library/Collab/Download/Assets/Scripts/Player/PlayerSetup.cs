using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] componentToDisable;

    [SerializeField]
    string remoteLayerName = "remotePlayer";

    // Use this for initialization
    void Start () {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            //gameObject.layer = LayerMask.NameToLayer(localLayerName);

            Camera.main.GetComponent<CameraFollow>().setMainPlayer(transform);
            Invoke("SetupPlayer" , 0.1f);
        }
    }

    private void SetupPlayer()
    {
        GetComponent<Player>().Setup();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManger.RegisterPlayer(_netID, _player);
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentToDisable.Length; i++)
        {
            componentToDisable[i].enabled = false;
        }
    }

    void OnDisable()
    {
        /*
        if (isLocalPlayer)
        {
            BackToLobby();
        }
        */

        GameManger.UnRegisterPlayer(transform.name);
    }

    void BackToLobby()
    {
        FindObjectOfType<NetworkLobbyManager>().ServerReturnToLobby();
    }

}
