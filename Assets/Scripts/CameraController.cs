using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // Modifiers
    private Vector2 velocity;
    public float smoothTimeX;
    public float smoothTimeY;

    // Camera bounds
    public bool bounds;
    public Vector3 minCameraPosition;
    public Vector3 maxCameraPosition;


    // Hero reference
    public GameObject hero;

    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float positionX = Mathf.SmoothDamp(transform.position.x, hero.transform.position.x, ref velocity.x, smoothTimeX);
        float positionY = Mathf.SmoothDamp(transform.position.y, hero.transform.position.y, ref velocity.y, smoothTimeY);
        transform.position = new Vector3(positionX, positionY, transform.position.z);

        if (bounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPosition.x, maxCameraPosition.x),
                Mathf.Clamp(transform.position.y, minCameraPosition.y, maxCameraPosition.y),
                transform.position.z);
        }
    }
}
