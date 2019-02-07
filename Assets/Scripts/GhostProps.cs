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


    // Start is called before the first frame update
    void Start()
    {
        // Init scalars
        hrNames = new string[3] { "Ninja", "Pirate", "Zombie" };
        hrJumpFactor = new float[3] { 12f, 8f, 5f };
        hrMoveSpeed = new float[3] { 5f, 2.5f, 1.5f };
        hrDoubleJump = new bool[3] { true, false, false };
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
