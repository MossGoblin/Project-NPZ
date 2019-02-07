using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpriteScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject repo;
    public GameObject player;
    public Sprite[] chSpriteArray;
    public GameObject[] ghosts;

    void Start()
    {
        Debug.Log("Trying");
        if (repo == null)
        {
            repo = GameObject.FindWithTag("repo");
        }

        if (repo != null)
        {
            Debug.Log("Got it!");
        }

        // Trying to change the sprite
        player = GameObject.FindWithTag("Player");
        this.GetComponent<SpriteRenderer>().sprite = repo.GetComponent<SpriteRenderer>().sprite;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<SpriteRenderer>().sprite = chSpriteArray[1];
        }
        
    }
}
