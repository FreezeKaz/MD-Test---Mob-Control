using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DestroyablePortal : MonoBehaviour
{

    [SerializeField] public GameObject particles;
    [SerializeField] public GameObject portal;
    [SerializeField] public CinemachineCollisionImpulseSource impulse;
    [SerializeField] public AudioClip portalBreak;
    // Start is called before the first frame update
    public void DestroyPortal()
    {
        particles.SetActive(true);
        portal.SetActive(false);
        AudioManager.Instance.PlaySFX(portalBreak,0.2f);
        impulse.GenerateImpulse(1f);
    }
}
