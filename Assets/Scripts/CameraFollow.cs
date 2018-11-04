using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    //private Vector3 offset;
    private bool cameraMove;
    private float distance;
    private float speed;
    private float y;
    private float x;
    private float z;
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        distance = 7.21f;
        y = angles.y;
        x = angles.x;
        speed = 50.0f;

        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (!player.GetComponent<Player>().isPaused)
        {
            y += Input.GetAxis("Mouse X") * speed * distance * 0.02f;
            x += Input.GetAxis("Mouse Y") * speed * distance * 0.02f;

            var rotation = Quaternion.Euler(Mathf.Clamp(-x, 2, 65), y, 0);
            var position = rotation * new Vector3(0.0f, 0.0f, -distance) + player.transform.position;
            transform.rotation = rotation;
            transform.position = position;

            RaycastHit hit = new RaycastHit();
            Ray wallRay = new Ray(player.transform.position, transform.position - player.transform.position);

            if (Physics.Raycast(wallRay, out hit, distance))
            {
                if (!hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.CompareTag("Enemy"))
                {
                    transform.position = hit.point;
                }
            }

            lastPosition = transform.position;
            lastRotation = transform.rotation;
        }
        else
        {
            transform.position = lastPosition;
            transform.rotation = lastRotation;
        }
    }
}
