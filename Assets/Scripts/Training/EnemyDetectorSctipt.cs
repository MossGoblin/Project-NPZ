using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectorSctipt : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponentInParent<EnemyAI>().heroInRange = true;
            GetComponentInParent<EnemyAI>().attackTarget = other.transform.position;
            GetComponentInParent<EnemyAI>().calmState = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(CalmDownTimer(3));
            GetComponentInParent<EnemyAI>().heroInRange = false;
        }
    }
    private IEnumerator CalmDownTimer(int calmDownTime)
    {
        int counter = calmDownTime;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
        GetComponentInParent<EnemyAI>().calmState = true;
        Debug.Log("Calmed Down");
    }
}
