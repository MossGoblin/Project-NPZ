using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMaster : MonoBehaviour, IAgent
{
    // References
    [SerializeField] private Conductor conductor;
    SpriteRenderer spriteRedenrer;
    Rigidbody2D rigidBody;
    [SerializeField] private Transform[] playerBase;
    [SerializeField] private LayerMask groundDef;

    // Current status
    [SerializeField] private int status;

    // Spatial variables
    [SerializeField] private bool didDoubleJump;
    [SerializeField] private bool onGround;
    private bool canDoubleJump;
    private float jumpPower;

    public void Init()
    {
        status = conductor.heroStatus;
        spriteRedenrer.sprite = conductor.ghostMaster.sprites[status];
        onGround = OnGround();
        canDoubleJump = conductor.ghostMaster.doubleJump[status];
        jumpPower = conductor.ghostMaster.jumpPower[status];
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
        if (onGround)
        {
            rigidBody.AddForce(new Vector2(0, jumpPower));
            onGround = OnGround();
        }
        else if (!onGround && canDoubleJump && !didDoubleJump)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            didDoubleJump = true;
            rigidBody.AddForce(new Vector2(0, jumpPower * 0.75f));
            onGround = OnGround();
        }
    }

    
    public void Move(float hrMovement)
    {
        // TODO :: HH Something wrong with the movement
        if (hrMovement != 0)
        {
            rigidBody.velocity = new Vector2(hrMovement * conductor.ghostMaster.moveSpeed[status], rigidBody.velocity.y);
        }
    }

    public bool OnGround()
    {
        if (rigidBody.velocity.y <= 0)
        {
            foreach (Transform groundPoint in playerBase)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(groundPoint.position.x, groundPoint.position.y), 0.5f, groundDef);
                for (int count = 0; count < colliders.Length; count++)
                {
                    if (colliders[count].gameObject != gameObject)
                    {
                        didDoubleJump = false;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void Swap(int state)
    {
        //Debug.Log($"HH: Swap to {state} Called");

        // Swap state
        status = state;
        Init();
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
    void FixedUpdate()
    {
        onGround = OnGround();
        Move(conductor.horizontal);
        //CheckBorders();
    }

}
