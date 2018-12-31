using UnityEngine;

[System.Serializable]
public class PlayerHull
{
    public Hull hullData;

    public void GetSpeedValues(out float speedForce , out float toruqeForce , out float mass , out float linearDrag , out float AngularDrag)
    {
        //tanky : 50 , 30 , 3 , 5 , 6;
        speedForce = hullData.enginePower / 6;
        toruqeForce = hullData.enginePower / 5;
        linearDrag = hullData.weight / 20 + hullData.drag * 2.5f / 2;
        AngularDrag = hullData.weight / 20 + hullData.drag * 3.5f;
        mass = hullData.weight * 3 / 50;
    }

    public void SetValues(string _name)
    {
        hullData = Resources.Load<Hull>("ScriptObjects/Hull/"+_name);
    }
}
