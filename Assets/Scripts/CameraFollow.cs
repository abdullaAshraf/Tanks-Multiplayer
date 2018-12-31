using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public Transform map;

    public float smoothSpeed = 12.5f;
    public Vector3 offset;
    public float limitMinX;
    public float limitMaxX;
    public float limitMinY;
    public float limitMaxY;

    
    private void Start()
    {
        //Invoke("getMainPlayer", 0.2f);
    }
    
    private void getMainPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
            if (player.GetComponent<Player>().isLocalPlayer)
                target = player.transform;
    }

    public void setMainPlayer(Transform player)
    {
        target = player;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            desiredPosition.x = Mathf.Max(desiredPosition.x, limitMinX);
            desiredPosition.x = Mathf.Min(desiredPosition.x, limitMaxX);
            desiredPosition.y = Mathf.Max(desiredPosition.y, limitMinY);
            desiredPosition.y = Mathf.Min(desiredPosition.y, limitMaxY);
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
            transform.position = smoothPosition;
            //transform.LookAt(target);
        }
    }

}
