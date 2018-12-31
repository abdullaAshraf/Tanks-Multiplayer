﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankControllerPhysics : NetworkBehaviour {

    [SerializeField]
    float speedForce = 3f;

    [SerializeField]
    float toruqeForce = 3f;

    public float speedFactor = 1f;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    private Joystick joystick;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joystick = GameObject.FindObjectOfType<Joystick>();
    }

    public void SetHull(string hull)
    {
        float mass, linearDrag, AngulerDrag;
        PlayerHull playerHull = new PlayerHull();
        playerHull.SetValues(hull);
        playerHull.GetSpeedValues(out speedForce,out toruqeForce,out mass,out linearDrag,out AngulerDrag);
        GetComponent<Rigidbody2D>().mass = mass;
        GetComponent<Rigidbody2D>().drag = linearDrag;
        GetComponent<Rigidbody2D>().angularDrag = AngulerDrag;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        //android
        Vector2 moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);

        if(joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        CmdMovePlayer(moveInput.normalized);
    }

    [ServerCallback]
    private void FixedUpdate()
    {
        if (Mathf.Abs(moveVelocity.x) > 0.01f || Mathf.Abs(moveVelocity.y) > 0.01f)
        {
            float angle = Mathf.Atan2(moveVelocity.y, moveVelocity.x) * Mathf.Rad2Deg - 90;
            angle = (angle + 360) % 360;
            float angle2 = (angle + 180) % 360;
            rb.rotation = (rb.rotation + 360) % 360;
            if (angleDiff(angle, rb.rotation) > 90)
                angle = angle2;
            if (angleDiff(angle, rb.rotation) < 2)
            {
                rb.angularVelocity = 0;
                rb.MoveRotation(angle);
                rb.AddForce(moveVelocity * speedForce * speedFactor);
            }
            else if ((angle > rb.rotation && angle - rb.rotation < 180) || (angle < rb.rotation && rb.rotation - angle > 180))
            {
                rb.AddTorque(toruqeForce);
            }
            else
            {
                rb.AddTorque(-toruqeForce);
            }
        }

    }



    private float angleDiff(float ang1, float ang2)
    {
        if (Mathf.Abs(ang1 - ang2) < 180)
            return Mathf.Abs(ang1 - ang2);
        else
            return 360 - Mathf.Abs(ang1 - ang2);
    }

    [Command]
    private void CmdMovePlayer(Vector2 toward)
    {
        moveVelocity = toward;
    }

    [Command]
    public void CmdEditSpeedFactor(float amount)
    {
        speedFactor += amount;
    }

}
