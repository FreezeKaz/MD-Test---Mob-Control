using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBridgeManager : MonoBehaviour
{

    [SerializeField] public float leftBound = -5f;
    [SerializeField] public float rightBound = 5f;
    [SerializeField] public float speed = 2f;

    public bool moving = true;
    private bool right = true;
    private bool isStopping = false;

    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private float stopDuration = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(transform.position);  
        if (moving)
        {
            if (isStopping) return;

            if (right)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);

                if (transform.position.x >= rightBound)
                {
                    right = false;
                    StartCoroutine(StopAtEdge());
                }
            }
            else
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);

                if (transform.position.x <= leftBound)
                {
                    right = true;
                    StartCoroutine(StopAtEdge()); 
                }
            }

        }
    }

    IEnumerator StopAtEdge()
    {
        isStopping = true;
        float wait = Random.Range(0.4f, stopDuration);
        yield return new WaitForSeconds(wait);
        speed = Random.Range(minSpeed, maxSpeed);
        isStopping = false;
    }
}
