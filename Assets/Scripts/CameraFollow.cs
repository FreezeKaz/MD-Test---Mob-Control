using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    public Vector3 offset2;
    public Vector3 desiredFinalPos;
    public Vector3 desiredRotation;

    public Vector3 saveBasePlace;
    public Vector3 saveBaseRot;

    public float smoothSpeed = 0.125f;
    public float transitionSpeed = 1f;

    private Vector3 basePos;
    private Vector3 desiredPos;
    public bool ChangeView = false;
    public bool follow = true;

    void Start()
    {
        offset = transform.position - target.position;
        offset2 = new Vector3(-0, 10f, -10f);
    }

    private void Update()
    {
      follow = target.gameObject.GetComponent<CanonManager>().dead ? false : true;
    }

    void LateUpdate()
    {
        if (follow)
        {
            if (ChangeView)
            {
                desiredPos = Vector3.Lerp(target.position + offset, target.position + offset2, 10f + Time.deltaTime);
            }
            else
            {
                desiredPos = target.position + offset;
            }
            transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        }
      
    }

}
