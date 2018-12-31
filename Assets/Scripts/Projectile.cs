using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour
{
    [HideInInspector]
    public string _ID;

    [HideInInspector]
    public Vector2 target;

    [HideInInspector]
    public PlayerWeapon weapon;

    public GameObject hitEffect;

    private bool isLive = true;

    Rigidbody2D rb;

    private float distanceTravelled;

    private void Start()
    {
        distanceTravelled = 0;
        rb = GetComponent<Rigidbody2D>();
    }

    [ServerCallback]
    void Update()
    {
        rb.velocity = transform.right * weapon.bulletSpeed;

        distanceTravelled += Time.deltaTime;

        if (distanceTravelled > weapon.range)
        {
            RemoveShell();
        }
    }

    [Command]
    void CmdOnHitPlayer(Vector2 _pos, string _id)
    {
        Player player = GameManger.GetPlayer(_id);
        player.GetComponent<Rigidbody2D>().AddForceAtPosition(weapon.targetImpact * target / 10, transform.position);
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
        GameObject _hitEffect = Instantiate(hitEffect, _pos, Quaternion.identity);
        Destroy(_hitEffect, 2);
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isLive || other.transform.name == _ID)
            return;

        
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().RpcTakeDamage(weapon.damage * (1 - weapon.maxDamageDecreaseOverRange * (distanceTravelled / weapon.range)), _ID);
            CmdOnHitPlayer(transform.position, other.gameObject.name);
            RemoveShell();
        }
        else if(other.CompareTag("Wall"))
        {
            CmdOnHit(transform.position);
            RemoveShell();
        }
    }

    private void DestroyShell()
    {
        NetworkServer.Destroy(gameObject);
    }

    private void RemoveShell()
    {
        isLive = false;
        GetComponent<Renderer>().enabled = false;
        Invoke("DestroyShell", .1f);
    }
}
