using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObjectManager : MonoBehaviour
{
    [SerializeField] public Transform bridge;
    [SerializeField] public bool Broke;
    [SerializeField] public MovingBridgeManager MBManager;


  
    private void Start()
    {
      
       

    }

    public void StopBridge()
    {
        Broke = true;
        MBManager.moving = false;
    }

    // Update is called once per frame
    void Update()
    {
      
        gameObject.transform.position = new Vector3(bridge.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

    }
   
}
