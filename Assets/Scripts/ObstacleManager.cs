using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cinemachine;
public class ObstacleManager : MonoBehaviour
{
    [SerializeField] public int value;
    private Vector3 baseScale = Vector3.one;
    [SerializeField] public Transform parentTransform;
    [SerializeField] public TextMeshPro textMesh;
    [SerializeField] public GameObject Particle;
    [SerializeField] public Animator animator;
    [SerializeField] public CannonController cannon;
    [SerializeField] public CinemachineImpulseSource cinemachine;
    [SerializeField] public SpecialObjectManager specialObject;
    [SerializeField] public MeshCollider meshCollider;
    [SerializeField] public Renderer myrenderer;
    [SerializeField] public AudioClip blockBreak;
    public bool IsLerping = false;


    public bool SpecialObstacle = false;
    // Start is called before the first frame update
    void Start()
    {
        baseScale = parentTransform.localScale;
        textMesh.text = value.ToString();
    }

    public void OnHit()
    {

       
        if (value >= 1)
        {
            value--;

            textMesh.text = value.ToString();
            parentTransform.DOKill();
            parentTransform.localScale = baseScale; // reset to normal
            parentTransform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f, 1, 0.2f);

            if (value <= 0)
            {
                if (SpecialObstacle)
                {
                    IsLerping = true;
                    cannon.canAct = false;
                    cannon.sound.enabled = false;
                    cannon.animator.SetBool("Shooting", false);
                    specialObject.StopBridge();
                    cinemachine.GenerateImpulse(0.3f);
                    meshCollider.enabled = false;
                    myrenderer.enabled = false;
                    textMesh.enabled = false;
                    Particle.gameObject.SetActive(true);
                    AudioManager.Instance.PlaySFX(blockBreak, 0.06f);
                }
                else
                {
                    cinemachine.GenerateImpulse(0.3f);
                    Particle.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                    AudioManager.Instance.PlaySFX(blockBreak, 0.06f);
                }
             
            }
        }

    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (SpecialObstacle && value <= 0 && IsLerping)
        {
         
            if (Mathf.Abs(cannon.gameObject.transform.position.x - parentTransform.position.x) <= 0.2f) 
            {
                Debug.Log("HEYYY");
                cannon.animator.SetBool("Move", true);
                IsLerping = false; 

            }
            cannon.gameObject.transform.position = Vector3.Lerp(cannon.gameObject.transform.position, new Vector3(parentTransform.position.x, cannon.gameObject.transform.position.y, cannon.gameObject.transform.position.z), 3f * Time.deltaTime); ;
        }
    }
}
