using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // self references
    private int selfType;
    [SerializeField]
    private GameObject projectile;

    public bool meleeAttackTriggerBool;


    private void Awake()
    {
        {
            selfType = GetComponentInParent<EnemyController>().selfType;
        }
    }
    
    public void Attack()
    {
        switch (selfType)
        {
            case 1:
                MeleeAttack();
                break;
            case 2:
                ProjectileAttack();
                break;
            default:
                BeamAttack();
                break;
        }
    }

    private void MeleeAttack()
    {
    }

    private void ProjectileAttack()
    {

    }

    private void BeamAttack()
    {

    }

    private void Update()
    {
    }
}
