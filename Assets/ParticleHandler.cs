using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(WaitTillDestroy());
    }
    public IEnumerator WaitTillDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
