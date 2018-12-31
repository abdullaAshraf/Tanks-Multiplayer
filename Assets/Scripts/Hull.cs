using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hull",menuName = "Hulls")]
public class Hull : ScriptableObject {
    public string name = "Tanky";

    public float maxHealth = 100;

    public float enginePower = 300f; //100 to 500 hp
    public float weight = 50f; //5 to 75 ton
    public float drag = 2f;
}
