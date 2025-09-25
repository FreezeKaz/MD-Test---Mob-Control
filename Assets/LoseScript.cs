using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScript : MonoBehaviour
{

    [SerializeField] private AudioClip LooseCLip;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlaySFX(LooseCLip, 0.3f);
    }

}
