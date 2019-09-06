using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeTrigger : MonoBehaviour
{
    // visual melee attack timer
    private float visualMelleAttackTimer;
    // references
    SpriteRenderer renderer;
    [SerializeField]
    Sprite baseSprite;
    [SerializeField]
    Sprite attackSprite;
    [SerializeField]
    GhostMaster ghostMaster;
    [SerializeField]
    Conductor conductor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // visual attack
            visualMelleAttackTimer = 1.0f;
            renderer.sprite = attackSprite;
            // Send damage to the player
            // TODO : Send damage to the player
            //other.GetComponent<HeroMaster>().
            // Send knockback to the player
            // get player speed
            float knockBackSpeed = ghostMaster.moveSpeed[conductor.heroStatus] * 1.5f;
            // get direction
            if (other.transform.position.x <= transform.position.x)
            {
                knockBackSpeed *= -1;
            }
            other.GetComponent<HeroMaster>().GetKnockBack(knockBackSpeed);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponentInParent<SpriteRenderer>();
        ghostMaster = FindObjectOfType<GhostMaster>();
        conductor = FindObjectOfType<Conductor>();
    }

    // Update is called once per frame
    void Update()
    {
        visualMelleAttackTimer -= Time.deltaTime;
        if (visualMelleAttackTimer <= 0 || GetComponentInParent<EnemyAI>().heroInRange == false)
        {
            CancelAttack();
        }
    }

    public void CancelAttack()
    {
        visualMelleAttackTimer = 0;
        renderer.sprite = baseSprite;
    }
}
