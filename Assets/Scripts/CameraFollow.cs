using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        
    private Vector3 offset;           
    public Vector3 desiredFinalPos;    
    public Vector3 desiredRotation;

    public Vector3 saveBasePlace;    
    public Vector3 saveBaseRot;     
                                      
    public float smoothSpeed = 0.125f;   
    public float transitionSpeed = 1f;   

    private Vector3 basePos;
    private bool isTransitioning = false;
    private bool transitionDone = false;

    void Start()
    {
        offset = transform.position - target.position;
        StartCoroutine(StartTransition());
    }
   

    IEnumerator StartTransition()
    {
        yield return new WaitForSeconds(3f);
        isTransitioning = true;
    }

    void LateUpdate()
    {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
