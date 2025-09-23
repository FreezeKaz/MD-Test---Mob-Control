using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro; 
public class ObstacleManager : MonoBehaviour
{
    [SerializeField] public int value;
    private Vector3 baseScale = Vector3.one;
    [SerializeField] public Transform parentTransform;
    [SerializeField] public TextMeshPro textMesh;
    // Start is called before the first frame update
    void Start()
    {
        baseScale = parentTransform.localScale;
        textMesh.text = value.ToString();
    }

    public void OnHit()
    {
        value--;
        textMesh.text = value.ToString();
        parentTransform.DOKill();
        parentTransform.localScale = baseScale; // reset to normal

        parentTransform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f, 1, 0.2f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
