using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Shooting : NetworkBehaviour
{
    public Vector2 target;

    private bool go;
    public PlayerWeapon weapon;
    private FixedJoystick joystick;

    private List<Touch> uiTouch;

    private float coolDown;
    bool canShoot;

    public Image manaDisplay;

    //spray weapons
    [SyncVar]
    bool sprayShooting;

    // Use this for initialization
    void Start()
    {
        uiTouch = new List<Touch>();
        weapon = new PlayerWeapon();
        Invoke("GetWeapon", 0.1f);
        joystick = GameObject.FindObjectOfType<Joystick>().GetComponent<FixedJoystick>();
    }

    void GetWeapon()
    {
        weapon = GetComponent<Player>().Turret.GetComponent<PlayerWeapon>();
        coolDown = weapon.coolDownTime;
        canShoot = true;
        sprayShooting = false;
    }

    private void Update()
    {
        if (coolDown < weapon.coolDownTime)
            coolDown += Time.deltaTime;

        manaDisplay.fillAmount = coolDown / weapon.coolDownTime;

        if (sprayShooting)
        {
            coolDown -= Time.deltaTime * weapon.sprayFactor;
            if(coolDown <= 0)
            {
                coolDown = 0;
                CmdSprayStop();
            }
        }

        if (!isLocalPlayer || PauseMenu.isOn)
            return;

        /*
        if (Input.GetMouseButton(0))
        {
            CmdAimToShoot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else
        {
            if (weapon.spray && sprayShooting)
                CmdSprayStop();
        }
        */
        

        //for android
        if (Input.touchCount > 0)
        {
            int i = 0;
            for (; i < Input.touchCount; i++)
            {
                int ind = Input.GetTouch(i).fingerId;
                bool t = true;

                foreach (PointerEventData pvd in joystick.touchs)
                    if (pvd.pointerId == ind)
                        t = false;


                if (Input.GetTouch(i).phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(ind))
                {
                    if (EventSystem.current.currentSelectedGameObject != null)
                    {
                        uiTouch.Add(Input.GetTouch(i));
                        StartCoroutine(Release(Input.GetTouch(i), 0.5f));
                    }
                }

                foreach (Touch to in uiTouch)
                    if (to.fingerId == ind)
                        t = false;

                if (t)
                {
                    CmdAimToShoot(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position));
                    break;
                }
            }
            if (i == Input.touchCount && sprayShooting)
                CmdSprayStop();
        }
        else if(sprayShooting)
        {
            CmdSprayStop();
        }
    }


    IEnumerator Release(Touch eventData, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        uiTouch.Remove(eventData);
    }

    [Command]
    private void CmdAimToShoot(Vector2 toward)
    {
        target = toward;
        go = true;
    }

    [ServerCallback]
    void FixedUpdate()
    {
        if (go)
        {
            float angle = AngleBetweenVector2(target, weapon.pivotPoint.position) + 90;
            angle = (angle + 360) % 360;

            float rotated = weapon.pivotPoint.eulerAngles.z;
            rotated = (rotated + 360) % 360;

            // Debug.Log(angle + " " + rotated + " " + angleDiff(angle, rotated));


            if (angleDiff(angle, rotated) < weapon.turningSpeed)
            {

                weapon.pivotPoint.eulerAngles = new Vector3(
                weapon.pivotPoint.eulerAngles.x,
                weapon.pivotPoint.eulerAngles.y,
                angle
                );

                if (canShoot && coolDown >= weapon.coolDownBullet)
                {
                    if (weapon.spray)
                    {
                        if (!sprayShooting)
                            CmdSprayStart(transform.name);
                    }
                    else
                    {
                        canShoot = false;
                        shoot();
                        Invoke("CanShoot", weapon.fireRate);
                    }
                }
                go = false;
            }
            else if ((angle > rotated && angle - rotated < 180) || (angle < rotated && rotated - angle > 180))
            {
                weapon.pivotPoint.Rotate(Vector3.forward, 50 * weapon.turningSpeed * Time.fixedDeltaTime);
            }
            else
            {
                weapon.pivotPoint.Rotate(Vector3.back, 50 * weapon.turningSpeed * Time.fixedDeltaTime);
            }
        }
    }

    [Command]
    void CmdSprayStart(string _playerID)
    {
        sprayShooting = true;
        RpcSetSpray(true);
    }

    [Command]
    void CmdSprayStop()
    {
        sprayShooting = false;
        RpcSetSpray(false);
    }

    [Command]
    public void CmdStopShooting()
    {
        if (weapon.spray)
        {
            sprayShooting = false;
            RpcSetSpray(false);
        }
    }

    [ClientRpc]
    void RpcSetSpray(bool go)
    {
        ParticleSystem sprayWeapon = weapon.NextParticleEffect();
        sprayWeapon.GetComponent<ParticleSpray>()._ID = transform.name;
        if (go)
            sprayWeapon.Play();
        else
            sprayWeapon.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void CanShoot()
    {
        canShoot = true;
    }

    [Client]
    void shoot()
    {
        CmdShot(transform.name, target);
    }

    [Command]
    void CmdShot(string _playerID, Vector2 tar)
    {
        //Debug.Log(_playerID + " shooted.");

        //Player _player = GameManger.GetPlayer(_playerID);

        RpcResetCoolDown();
        Transform bulletsCube = weapon.NextShotSpawmPoint();
        GameObject go = Instantiate(weapon.shot, bulletsCube.position, bulletsCube.rotation) as GameObject;
        go.GetComponent<Projectile>().target = tar;
        go.GetComponent<Projectile>()._ID = _playerID;
        go.GetComponent<Projectile>().weapon = weapon;

        NetworkServer.Spawn(go);
        //RpcInstantiateShell(_playerID, tar);

        RpcDoShootEffect();
    }

    [ClientRpc]
    private void RpcInstantiateShell(string _playerID, Vector2 tar)
    {
        Transform bulletsCube = weapon.NextShotSpawmPoint();
        GameObject go = Instantiate(weapon.shot, bulletsCube.position, bulletsCube.rotation) as GameObject;
        go.GetComponent<Projectile>().target = tar;
        go.GetComponent<Projectile>()._ID = _playerID;
        go.GetComponent<Projectile>().weapon = weapon;
    }

    [ClientRpc]
    private void RpcResetCoolDown()
    {
        coolDown -= weapon.coolDownBullet;
    }

    [ClientRpc]
    void RpcDoShootEffect()
    {
        ParticleSystem muzzle = weapon.NextParticleEffect();
        muzzle.Play();
    }

    private float angleDiff(float ang1, float ang2)
    {
        if (Mathf.Abs(ang1 - ang2) < 180)
            return Mathf.Abs(ang1 - ang2);
        else
            return 360 - Mathf.Abs(ang1 - ang2);
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }
}
