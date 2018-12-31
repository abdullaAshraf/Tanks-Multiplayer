using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManger : MonoBehaviour
{

    private float coolDown;
    private float effectCoolDown;
    private bool running = false;

    private Ability ability;

    private Player localPlayer;

    [SerializeField]
    private Image abilityFiller;

    [SerializeField]
    private Image abilityImage;

    [SerializeField]
    private Text debugText;

    // Use this for initialization
    void Start()
    {
        Invoke("SetValues", 0.2f);
    }

    private void SetValues()
    {
        localPlayer = GameManger.GetLocalPlayer();
        //debugText.text = localPlayer.playerName;
        ability = new Ability();
        ability.SetValues(localPlayer.playerAbility);
        coolDown = 0;
        abilityFiller.fillAmount = 0;
        effectCoolDown = ability.effectTime;
        abilityImage.sprite = ability.graphics;
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            if (effectCoolDown >= 0.01f)
                effectCoolDown -= Time.deltaTime;
            else
            {
                running = false;
                ability.counterEffect(localPlayer);
            }
        }
        if (coolDown >= 0.01f)
        {
            coolDown -= Time.deltaTime;
            abilityFiller.fillAmount = coolDown / ability.coolDownTime;
        }


        //debug on pc
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartEffect();
        }
    }

    public void StartEffect()
    {
        if (coolDown >= 0.01f || running)
            return;
        running = true;
        abilityFiller.fillAmount = 1;
        coolDown = ability.coolDownTime;
        effectCoolDown = ability.effectTime;
        ability.effect(localPlayer);
    }

}
