using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ParticleSpray : NetworkBehaviour
{

    [HideInInspector]
    public string _ID;

    public PlayerWeapon weapon;

    private bool isLive = true;

    public GameObject hitEffect;

    List<string> playersDamaged;

    // Use this for initialization
    void Start () {
        playersDamaged = new List<string>();
    }

    IEnumerator Release(string player, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        playersDamaged.Remove(player);
    }

    [ServerCallback]
    private void OnParticleCollision(GameObject other)
    {
        string damagedId = other.transform.name;

        if (!isLive || damagedId == _ID)
            return;


        if (other.CompareTag("Player") && !playersDamaged.Contains(damagedId))
        {
            other.gameObject.GetComponent<Player>().RpcTakeDamage(weapon.damage, _ID);
            //TODO spray effect
            playersDamaged.Add(damagedId);
            StartCoroutine(Release(damagedId, 0.25f));
            //CmdOnHitPlayer(transform.position, other.gameObject.name);
        }
        else if (other.CompareTag("Wall"))
        {
            //CmdOnHit(transform.position);
        }
    }

    [Command]
    void CmdOnHitPlayer(Vector2 _pos, string _id)
    {
        Player player = GameManger.GetPlayer(_id);
        RpcDoHitEffect(_pos);
    }

    [Command]
    void CmdOnHit(Vector2 _pos)
    {
        RpcDoHitEffect(_pos);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector2 _pos)
    {
        return;
        GameObject _hitEffect = Instantiate(hitEffect, _pos, Quaternion.identity);
        Destroy(_hitEffect, 2);
    }
}
