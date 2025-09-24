using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObjectManager : MonoBehaviour
{
    [SerializeField] public float leftBound = -5f;
    [SerializeField] public float rightBound = 5f;
    [SerializeField] public float speed = 2f;
    [SerializeField] public ObstacleManager obstacleManager;

    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private float stopDuration = 1f;

    [SerializeField] private MeshRenderer bridgeRenderer;
    [SerializeField] private Color startColor = Color.yellow;
    [SerializeField] private Color endColor = Color.white;
    [SerializeField] private float duration = 2f;


    private float t = 0f;
    private bool isLerping = false;
    private Material bridgeMaterial;


    // Start is called before the first frame update
    private bool moving = true;
    private bool right = true;
    private bool isStopping = false;

    private void Start()
    {
      
        bridgeMaterial = bridgeRenderer.material;
        startColor = bridgeMaterial.color;

    }

    // Update is called once per frame
    void Update()
    {
        if (isLerping)
        {
            t += Time.deltaTime / duration;
            bridgeMaterial.color = Color.Lerp(startColor, endColor, t);

            if (t >= 1f)
                isLerping = false; // stop once finished
        }
        moving = obstacleManager.value <= 0 ? false : true;
        if (moving)
        {
            if (isStopping) return; // pause movement if stopping

            if (right)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);

                if (transform.position.x >= rightBound)
                {
                    right = false;
                    StartCoroutine(StopAtEdge()); // stop a bit at the edge
                }
            }
            else
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);

                if (transform.position.x <= leftBound)
                {
                    right = true;
                    StartCoroutine(StopAtEdge()); // stop a bit at the edge
                }
            }

        }


    }
    public void StartColorChange()
    {
        t = 0f;
        isLerping = true;
    }

    public void ChangeLayer()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Floor");


        }
        gameObject.layer = LayerMask.NameToLayer("Floor");
    }
    public void HasDoneExpanding()
    {
       
    }
    IEnumerator StopAtEdge()
    {
        isStopping = true;

        // Random pause time instead of always stopDuration
        float wait = Random.Range(0.4f, stopDuration);
        yield return new WaitForSeconds(wait);

        // New random speed for next run
        speed = Random.Range(minSpeed, maxSpeed);

        isStopping = false;
    }
}
