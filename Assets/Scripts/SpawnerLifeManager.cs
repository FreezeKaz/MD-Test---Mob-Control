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
    [SerializeField] public Vector3 ParticleSpawnPos;
    [SerializeField] public Quaternion ParticleRotation;
    [SerializeField] public GameObject Particle;

    private Vector3 baseScale;
    private Quaternion baseRotation;


    private void Start()
    {
        ParticleRotation = new Quaternion(-0.685923874f, -0.171780273f, 0.171780303f, 0.685923874f);
        baseScale = parentTransform.localScale;
        baseRotation = parentTransform.rotation;
        text.text = amount.ToString();
    }
    // Start is called before the first frame update
    public void OnSpawnerHit()
    {
        amount--;
        Instantiate(Particle, ParticleSpawnPos, ParticleRotation);
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
