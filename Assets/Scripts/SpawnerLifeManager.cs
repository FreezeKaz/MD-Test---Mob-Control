using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnerLifeManager : MonoBehaviour
{

    [SerializeField] public Transform parentTransform;
    [SerializeField] public TextMeshPro text;
    [SerializeField] public float amount;

    private Vector3 baseScale;
    private Quaternion baseRotation;


    private void Start()
    {
        baseScale = parentTransform.localScale;
        baseRotation = parentTransform.rotation;
        text.text = amount.ToString();
    }
    // Start is called before the first frame update
    public void OnSpawnerHit()
    {
        amount--;
        text.text = amount.ToString();
        parentTransform.DOKill();
        parentTransform.localScale = baseScale; // reset to normal
        parentTransform.rotation = baseRotation;
        parentTransform.DOPunchRotation(new Vector3(0, 0, 2f), 0.5f,1, 1).OnComplete(() =>
        {
            parentTransform.rotation = baseRotation; // back to 0,0,0
        }); ;
       
    }
}
