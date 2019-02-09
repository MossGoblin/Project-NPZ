using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostProps : MonoBehaviour
{
    // Character values
    [SerializeField] public string[] hrNames;
    [SerializeField] public float[] hrJumpFactor;
    [SerializeField] public float[] hrMoveSpeed;
    [SerializeField] public bool[] hrDoubleJump;
    [SerializeField] public Sprite[] hrSprites;
    [SerializeField] public float[] hrMaxTimers;
    [SerializeField] public float[] hrActiveTempo;
    [SerializeField] public float[] hrCoolDownTempo;

    [SerializeField] private float atf; // Active Time Factor
    [SerializeField] private float cdf; // CoolDown Factor



    // Start is called before the first frame update
    void Start()
    {
        // Init scalars
        hrNames = new string[3] { "Ninja", "Pirate", "Zombie" };
        hrJumpFactor = new float[3] { 12f, 9.5f, 7.5f };
        hrMoveSpeed = new float[3] { 5f, 3f, 2f };
        hrDoubleJump = new bool[3] { true, false, false };
        hrMaxTimers = new float[3] { 30f, 30f, 30f };
        hrActiveTempo = new float[3] { 1f * atf, 1f * atf, 1f * atf };
        hrCoolDownTempo = new float[3] { 0.5f * cdf, 0.5f * cdf, 0.5f * cdf };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string PingMe()
    {
        return "Ghosts here!";
    }
}
