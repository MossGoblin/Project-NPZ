using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // HERO SECTION
    [SerializeField] public string[] hNames;
    [SerializeField] public Sprite[] hSprites;
    [SerializeField] public Color[] hColor;
    [SerializeField] public float[] hJumpPower;
    [SerializeField] public float[] hMoveSpeed;
    [SerializeField] public bool[] hDoubleJump;
    [SerializeField] public bool[] hAerialControl;
    [SerializeField] public float[] hActiveTimer;
    [SerializeField] public float[] hCoodownTimer;
    [SerializeField] public float hActiveToken;
    [SerializeField] public float[] hCooldownToken;
    [SerializeField] public int[] bulletDamage;
    [SerializeField] public float[] bulletSpeed;
    [SerializeField] public float[] bulletLifespan;

    // ENEMY SECTION

}
