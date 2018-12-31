using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{

    public string name = "Glock";

    public float damage = 10f;
    public float range = 100f;
    public float coolDownTime = 3f;
    public float turningSpeed = 5;

    public float targetImpact = 1f;
    public float shooterImpact = 1f;

    public GameObject graphics;
}