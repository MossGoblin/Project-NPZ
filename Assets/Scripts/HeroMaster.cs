using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMaster : MonoBehaviour, IAgent
{
    // References
    [SerializeField] private Conductor conductor;
    [SerializeField] private ProjectileDepo prjDepo;
    SpriteRenderer spriteRedenrer;
    Rigidbody2D rigidBody;
    Transform transform;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform floorCheck;
    [SerializeField] private LayerMask groundDef;
    [SerializeField] private Transform shooter;

    // Current status
    [SerializeField] private int status;

    // Spatial variables
    [SerializeField] private bool didDoubleJump;
    [SerializeField] public bool onGround;
    [SerializeField] private bool canJump;
    private bool canDoubleJump;
    private float jumpPower;
    [SerializeField] private bool facingRight;

    public void Init()
    {
        status = conductor.heroStatus;
        spriteRedenrer.sprite = conductor.ghostMaster.sprites[status];
        canJump = true;
        //onGround = OnGround();
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
            //onGround = OnGround();
        }
        else if (!onGround && canDoubleJump && !didDoubleJump)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            didDoubleJump = true;
            rigidBody.AddForce(new Vector2(0, jumpPower /* * 0.75f*/));
            //onGround = OnGround();
        }
    }

    
    public void Move(float hrMovement)
    {
        if (hrMovement != 0)
        {
            rigidBody.velocity = new Vector2(hrMovement * conductor.ghostMaster.moveSpeed[status], rigidBody.velocity.y);
            FlipPlayer();
        }
    }

    public void SetOnGround(bool grounded)
    {
        onGround = grounded;
        didDoubleJump = false;
    }

    public void SetCanJump(bool noCeiling)
    {
        canJump = noCeiling;
    }

    public void Shoot()
    {
        // TODO : HERO : SHOOT Instantiate bullet
        Vector2 position;
        Vector2 direction = shooter.position - transform.position;
        direction.Normalize();
        Quaternion rotation;
        position = shooter.position;
        rotation = shooter.rotation;
        GameObject projectile = Instantiate(prjDepo.projectiles[0], position, rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = projectile.GetComponent<ShurikenController>().speed * direction;
    }

    //public bool OnGround()
    //{
    //    if (rigidBody.velocity.y <= 0)
    //    {
    //        foreach (Transform groundPoint in playerBase)
    //        {
    //            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(groundPoint.position.x, groundPoint.position.y), 0.2f, groundDef);
    //            for (int count = 0; count < colliders.Length; count++)
    //            {
    //                if (colliders[count].gameObject != gameObject)
    //                {
    //                    didDoubleJump = false;
    //                    return true;
    //                }
    //            }
    //        }
    //    }
    //    return false;
    //}

    //private void OnCollisionEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Terrain")
    //    {
    //        onGround = true;
    //    }
    //}

    public void Swap(int state)
    {
        //Debug.Log($"HH: Swap to {state} Called");

        // Swap state
        status = state;
        Init();
    }

    private void FlipPlayer()
    {
        if (onGround || (/*!onGround && */conductor.ghostMaster.airControl[status]))
        {
            if ((rigidBody.velocity.x >= 0 && facingRight) || (rigidBody.velocity.x < 0 && !facingRight))
            {
                facingRight = !facingRight;
                float newScaleX = transform.localScale.x * -1;
                transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Self-References
        spriteRedenrer = gameObject.GetComponent<SpriteRenderer>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        transform = gameObject.GetComponent<Transform>();
        
        Init();

        // Outer References
        prjDepo = FindObjectOfType<ProjectileDepo>();
    }
    private void CheckBorders()
    {
        // Some magic numbers
        /*
        min x = -10
        max x = 61
        min y = -9
        max y = 16
        */

        if (transform.position.x <= -10 ||
            transform.position.x >= 61 ||
            transform.position.y <= -9 ||
            transform.position.y >= 16)
        {
            Debug.Log("Out of borders");
            conductor.RestartLevel("Fell off the map");
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        // Fake Friction
        Vector3 correctedVelocity = rigidBody.velocity;
        correctedVelocity.y = rigidBody.velocity.y;
        correctedVelocity.z = 0.00f;
        correctedVelocity.x *= 0.75f;

        if (onGround)
        {
            rigidBody.velocity = correctedVelocity;
        }

        //onGround = OnGround();
        Move(conductor.horizontal);
        CheckBorders();
    }

    public void GetKnockBack(float knockBackAmount)
    {
        // TODO : FIND A WAY TO APPLY KNOCKBACK
        Vector2 velocityKnockBackAddition = new Vector2(knockBackAmount, rigidBody.velocity.y);
        rigidBody.velocity += velocityKnockBackAddition;
    }
}
