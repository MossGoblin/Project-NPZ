using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollisionTrigger : MonoBehaviour
{
    [SerializeField] BoxCollider2D playerCollider;
    [SerializeField] BoxCollider2D platformCollider;
    [SerializeField] BoxCollider2D platformTrigger;
    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);

        // try to 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Player") || (other.gameObject.tag == "PlayerElement"))
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //if ((other.gameObject.tag == "Player") || (other.gameObject.tag == "PlayerElement"))
        if (other.gameObject.tag == "PlayerElement")
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        }
    }
}
