using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PipeManager : MonoBehaviour
{
    private Vector3 baseScale;
    private Vector3 exitBaseScale;
    [SerializeField] public Transform exitFirePoint;
    [SerializeField] public Transform exitTransform;
    [SerializeField] public float characSpeed = 7f; // How fast the character fly
    [SerializeField] public GameObject mob;

    private void Start()
    {
        exitBaseScale = exitTransform.localScale;
        baseScale = transform.localScale;
    }
    public void OnHit()
    {
        transform.DOKill();
        transform.localScale = baseScale; // reset to normal
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f, 1, 0.2f);
        StartCoroutine(DelayTransport());
    }

    public IEnumerator DelayTransport()
    {
        yield return new WaitForSeconds(0.8f);
        GameObject temp = Instantiate(mob, exitFirePoint.position, exitFirePoint.rotation);
        CharacterBrain tempCB = temp.GetComponent<CharacterBrain>();
        tempCB.CameFromPipe = true;
        tempCB.HasTOuchedFloor = true;
        tempCB.Init(0f, true, true);
        Rigidbody rb = temp.GetComponent<Rigidbody>();
        if (rb != null)
        {
          
            rb.velocity = exitFirePoint.forward * characSpeed;
            rb.velocity = new Vector3(2, 0, rb.velocity.z);

        }
        exitTransform.DOKill();
        exitTransform.localScale = exitBaseScale; // reset to normal
        exitTransform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f, 1, 0.2f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
