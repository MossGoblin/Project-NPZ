using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingCheck : MonoBehaviour
{
    // References
    [SerializeField] HeroMaster heroMaster;
    [SerializeField] CircleCollider2D floorCollider;

    // Start is called before the first frame update
    void Start()
    {
        Collider2D ceilingCollider = gameObject.GetComponent<CircleCollider2D>();
        Physics2D.IgnoreCollision(floorCollider, ceilingCollider, true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            //heroMaster.onGround = true;
            heroMaster.SetCanJump(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            //heroMaster.onGround = true;
            heroMaster.SetCanJump(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "TerrainMap")
        {
            //heroMaster.onGround = false;
            heroMaster.SetCanJump(true);
        }
    }
}
