using UnityEngine;

[System.Serializable]
public class Ability
{
    public string name = "Nitro";

    public float effectTime = 5f;
    public float coolDownTime =  30f;

    public Sprite graphics;

    public delegate void Effect(Player player);

    public delegate void CounterEffect(Player player);

    public Effect effect;

    public CounterEffect counterEffect;

    public void SetValues(string _name)
    {
        name = _name;
        graphics = Resources.Load<Sprite>("Sprites/ability/" + name); 
        
        if (name == "Nitro")
        {
            effectTime = 5f;
            coolDownTime = 30f;
            effect = SpeedPlayer;
            counterEffect = UnSpeedPlayer;
        }
    }

    public void SpeedPlayer(Player player)
    {
        player.GetComponent<TankControllerPhysics>().CmdEditSpeedFactor(.5f);
    }

    public void UnSpeedPlayer(Player player)
    {
        player.GetComponent<TankControllerPhysics>().CmdEditSpeedFactor(-.5f);
    }
}