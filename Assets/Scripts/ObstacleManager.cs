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
    [SerializeField] public GameObject Particle;
    [SerializeField] public Animator animator;
    [SerializeField] public CannonController cannon;
   private bool IsLerping = false;


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
            if (SpecialObstacle && value <= 0)
            {
              
                Vector3 cachedPos = parentTransform.position;
                IsLerping = true;
                animator.applyRootMotion = false;
                animator.SetBool("Unlock", true);
                cannon.canAct = false;
                cannon.sound.enabled = false;
               
                cannon.animator.SetBool("Shooting", false);
                parentTransform.position = cachedPos;
                //ANIM CODE
            }
            else if (value <= 0)
            {
                Particle.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }

    }
    // Update is called once per frame
    void LateUpdate()
    {

        if (SpecialObstacle && value <= 0 && IsLerping)
        {
            if (cannon.gameObject.transform.position == new Vector3(parentTransform.position.x, cannon.gameObject.transform.position.y, cannon.gameObject.transform.position.z)) { IsLerping = false; }
            cannon.gameObject.transform.position = Vector3.Lerp(cannon.gameObject.transform.position, new Vector3(parentTransform.position.x, cannon.gameObject.transform.position.y, cannon.gameObject.transform.position.z), 5f * Time.deltaTime); ;
        }
    }
}
