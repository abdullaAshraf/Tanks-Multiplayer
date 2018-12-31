using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed;
    public float turningSpeed;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<CameraFollow>().setMainPlayer(transform);
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
    }

    private void FixedUpdate()
    {
        if (PauseMenu.isOn)
            return;
        if (Mathf.Abs(moveVelocity.x) > 0.01f || Mathf.Abs(moveVelocity.y) > 0.01f)
        {
            float angle = Mathf.Atan2(moveVelocity.y, moveVelocity.x) * Mathf.Rad2Deg - 90;
            angle = (angle + 360) % 360;
            float angle2 = (angle + 180) % 360;
            rb.rotation = (rb.rotation + 360) % 360;
            /*
            if (angleDiff(angle, rb.rotation) > 90)
                angle = angle2;
                */
            if (angleDiff(angle, rb.rotation) < turningSpeed)
            {
                rb.MoveRotation(angle);
                rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
            }
            else if ((angle > rb.rotation && angle - rb.rotation < 180) || (angle < rb.rotation && rb.rotation - angle > 180))
            {
                rb.transform.Rotate(Vector3.back * -90 * turningSpeed * Time.fixedDeltaTime);
                //rb.MoveRotation(Mathf.Min(rb.rotation + 400 * Time.fixedDeltaTime, angle));
            }
            else
            {
                rb.transform.Rotate(Vector3.forward * -90 * turningSpeed * Time.fixedDeltaTime);
                //rb.MoveRotation(Mathf.Max(rb.rotation - 400 * Time.fixedDeltaTime, angle));
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
}
