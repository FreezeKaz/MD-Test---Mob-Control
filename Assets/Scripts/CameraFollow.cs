using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // Your cannon
    public Vector3 offset;           // Initial offset while following
    public Vector3 secondOffset;     // Where the camera should end up
    public float smoothSpeed = 0.125f;   // Smooth follow speed while following
    public float transitionSpeed = 1f;   // How fast it moves to secondOffset

    private Vector3 basePos;
    private bool isTransitioning = false;
    private bool transitionDone = false;

    void Start()
    {
        basePos = target.position;
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
            return; // Do nothing after transition, camera stays in place

        if (!isTransitioning)
        {
            // Follow cannon normally before 3 seconds
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
        else
        {
            // Smoothly move to secondOffset
            Vector3 targetPos = basePos + secondOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, transitionSpeed * Time.deltaTime);

            // Stop transitioning once close enough
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = targetPos;
                transitionDone = true; // Camera now stays here
            }
        }
    }
}
