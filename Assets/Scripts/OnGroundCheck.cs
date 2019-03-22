using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundCheck : MonoBehaviour
{
    // References
    [SerializeField] HeroMaster heroMaster;
    [SerializeField] CircleCollider2D ceilingCollider;
    //    [SerializeField] CircleCollider2D floorCollider;

    // Start is called before the first frame update
    void Start()
    {
        Collider2D floorCollider = gameObject.GetComponent<CircleCollider2D>();
        Physics2D.IgnoreCollision(floorCollider, ceilingCollider, true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            //heroMaster.onGround = true;
            heroMaster.SetOnGround(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            //heroMaster.onGround = true;
            heroMaster.SetOnGround(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "TerrainMap")
        {
            //heroMaster.onGround = false;
            heroMaster.SetOnGround(false);
        }
    }
}
