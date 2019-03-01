using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMaster : MonoBehaviour, IHero
{
    // References
    [SerializeField] private Conductor conductor;

    // Current stats
    [SerializeField] private int status;


    public void Init()
    {
        // Self-References
        SpriteRenderer spriteRedenrer = gameObject.GetComponent<SpriteRenderer>();

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

    public void Move(float horizontal)
    {
        throw new System.NotImplementedException();
    }

    public void OnGround()
    {
        throw new System.NotImplementedException();
    }

    public void Swap()
    {
        throw new System.NotImplementedException();
    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
