using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMaster : MonoBehaviour
{
    // References
    [SerializeField] private Conductor conductorObject;

    // Hero Stats Repository
    // Sprite and Body
    public Sprite[] sprites;
    public Color[] colors;

    // Enemy relations
    public int[] easyEnemies;
    public int[] hardEnemies;

    // Movement variables
    public float[] moveSpeed;
    public float[] jumpPower;
    public bool[] doubleJump;
    public bool[] airControl;

    // Attack variables
    public bool[] attackMods;
    public float[] rangedFrq;
    public float[] meleeFrq;
    public float[] rangedSlugSpeed;
    public float[] rangedDamage;
    public float[] meleeDamage;
    public float[] slugLifeSpan;

    // Timer defaults
    public float defaultActiveTimer;
    public float defaultCoolDownTimer;
    
    // Specials
}
