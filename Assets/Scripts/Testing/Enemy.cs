using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    public float speed;
    public float turningSpeed;
    private Transform playerPos;
    private Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float angle = Mathf.Atan2(playerPos.position.y, playerPos.position.x) * Mathf.Rad2Deg - 90;
        angle = (angle + 360) % 360;
        float angle2 = (angle + 180) % 360;
        rb.rotation = (rb.rotation + 360) % 360;
        if (Mathf.Abs(angle - rb.rotation) > 90)
            angle = angle2;
        //Debug.Log(angle + " " + rb.rotation);
        if (angleDiff(angle, rb.rotation) < turningSpeed)
        {

            rb.MoveRotation(angle);
            //rb.MovePosition(rb.position + new Vector2(playerPos.position.x , playerPos.position.y) *  speed * Time.fixedDeltaTime);
            rb.transform.position = Vector2.MoveTowards(rb.transform.position, playerPos.position, speed * Time.deltaTime);
        }
        else if ((angle > rb.rotation && angle - rb.rotation < 180) || (angle < rb.rotation && rb.rotation - angle > 180))
        {
            rb.transform.Rotate(Vector3.back * -90 * turningSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.transform.Rotate(Vector3.forward * -90 * turningSpeed * Time.fixedDeltaTime);
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
