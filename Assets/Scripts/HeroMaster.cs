using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMaster : MonoBehaviour, IAgent
{
    // References
    [SerializeField] private Conductor conductor;
    SpriteRenderer spriteRedenrer;
    Rigidbody2D rigidBody;

    // Current stats
    [SerializeField] private int status;


    public void Init()
    {
        status = conductor.heroStatus;
        spriteRedenrer.sprite = conductor.ghostMaster.sprites[status];
    }

    public void AttackMelee()
    {
        throw new System.NotImplementedException();
    }

    public void AttackRange()
    {
        throw new System.NotImplementedException();
    }

    public void Jump()
    {
        throw new System.NotImplementedException();
    }

    public void Move(float hrMovement)
    {
        if (hrMovement != 0)
        {
            rigidBody.velocity = new Vector2(hrMovement * conductor.ghostMaster.moveSpeed[status], rigidBody.velocity.y);
        }
    }

    public void OnGround()
    {
        throw new System.NotImplementedException();
    }

    public void Swap(int state)
    {
        Debug.Log($"HH: Swap to {state} Called");

        // Swap state
        status = state;

        // Swap sprite
        spriteRedenrer.sprite = conductor.ghostMaster.sprites[status];
    }


    // Start is called before the first frame update
    void Start()
    {
        // Self-References
        spriteRedenrer = gameObject.GetComponent<SpriteRenderer>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Move(conductor.horizontal);
    }
}
