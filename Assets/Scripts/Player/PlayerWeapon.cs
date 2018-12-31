using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerWeapon : MonoBehaviour
{
    public string name = "Classic";

    public float damage = 30f;
    public float range = 2f;
    public float coolDownTime = 3f;
    public float coolDownBullet = 1.5f;

    public float fireRate = 1f;

    public float turningSpeed = 5;

    public float bulletSpeed = 20f;

    public float targetImpact = 1f;
    public float shooterImpact = 1f;

    public float hitsThrough = 1f;
    public float damageDecresePerHit = 1f;

    public float areaDamageRadius = 0f;
    public float areaDamage = 1f;
    public float maxDamageDecreaseOverRange = 0.5f;

    //spray weapons
    public bool spray = false;
    public float sprayFactor = 3f;


    public Transform pivotPoint;
    public GameObject shot;

    public List<Transform> spawmPoints;
    public List<ParticleSystem> particleEffects;

    private int curSpwam = -1;
    private int curEffect = -1;

    public Transform NextShotSpawmPoint()
    {
        curSpwam = (curSpwam + 1) % spawmPoints.Count;
        return spawmPoints[curSpwam];
    }

    public ParticleSystem NextParticleEffect()
    {
        curEffect = (curEffect + 1) % particleEffects.Count;
        return particleEffects[curEffect];
    }
}