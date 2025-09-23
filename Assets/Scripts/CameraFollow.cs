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
        if (transitionDone)
            return;

        if (!isTransitioning)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
        else
        {
           
            transform.position = Vector3.Lerp(transform.position, desiredFinalPos, transitionSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.Euler(desiredRotation);
            transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,transitionSpeed * Time.deltaTime);

  
            if (Vector3.Distance(transform.position, desiredFinalPos) < 0.01f && Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
            {
                transform.position = desiredFinalPos;
                transform.rotation = targetRotation;
                transitionDone = true;
            }
        }
    }
}
