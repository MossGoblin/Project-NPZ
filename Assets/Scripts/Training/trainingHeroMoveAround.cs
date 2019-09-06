using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainingHeroMoveAround : MonoBehaviour
{
    private float horizontal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        transform.position = new Vector3(transform.position.x + horizontal * Time.deltaTime, transform.position.y);
    }
}
