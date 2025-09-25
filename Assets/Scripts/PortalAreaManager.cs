using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalAreaManager : MonoBehaviour
{

    [SerializeField] public DestroyablePortal destro;
    // Start is called before the first frame update
    public void DestroyPortal()
    {
        destro.DestroyPortal();
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");


        }
    }
}
