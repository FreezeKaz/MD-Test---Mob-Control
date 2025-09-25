using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ToonyColorsPro.ShaderGenerator.Enums;
using static Unity.VisualScripting.Member;

public class CharacterBrain : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;


    private bool HasAlreadyMultiplied = false;
    private bool CanMultiply = false;
    public bool Enemy = false;
    public bool CameFromPipe = false;
    public bool Died = false;
    public float TimeBeforeActivation = 0;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private LayerMask portalLayer;
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private LayerMask DeadLayer;
    [SerializeField] private LayerMask PipeLayer;
    [SerializeField] private LayerMask EnemyLayer;
    [SerializeField] private LayerMask CastleLayer;
    [SerializeField] private LayerMask PortalLayer;
    [SerializeField] private LayerMask DirectionChanger;
    [SerializeField] private LayerMask CanonLayer;
    [SerializeField] private LayerMask WaterLayer;
    [SerializeField] private LayerMask CanonTarget;
    [SerializeField] private Animator animator;
    [SerializeField] private Material MobMaterial;
    [SerializeField] private SkinnedMeshRenderer render;
    [SerializeField] private Collider myCollider;
    [SerializeField] private GameObject smokePart;
    [SerializeField] private GameObject number;
    [SerializeField] private GameObject cannon;

    [SerializeField] public List<AudioClip> audioClips;

    [SerializeField] private float minDistance = 0.5f; // security distance
    public LayerMask unitLayer;
    public bool HasTOuchedFloor = false;
    public bool HasTouchedDeadZone = false;
    public bool TargetCannon = false;
    public Vector3 reducedVelocity = Vector3.zero;
    private float nextStepTime;

    public void Init(float time, bool pipe, bool floor)
    {
        TimeBeforeActivation = time;
        CameFromPipe = pipe;
        HasTOuchedFloor = floor; ;
        cannon = CanonManager.Instance.gameObject;
        StartCoroutine(Activate());

    }
    void Update()
    {

        if (CameFromPipe && !Died)
        {
            rb.velocity = new Vector3(-5, 0, 7f);
            Debug.Log(rb.velocity);
        }

        else
        {

            if (HasTOuchedFloor && !Enemy && !Died) rb.velocity = new Vector3(0, rb.velocity.y, 7f);




            if (Enemy && !Died)
            {

                rb.velocity = TargetCannon ? new Vector3(rb.velocity.x, 0, rb.velocity.z) : new Vector3(0, 0, -2f);
                if (TargetCannon)
                {
                    rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
                    Vector3 direction = (cannon.transform.position - rb.position).normalized;
                    rb.AddForce(direction * 2f, ForceMode.Acceleration);
                    transform.LookAt(cannon.transform);
                }

            }
           

            if (Died) rb.velocity = Vector3.zero;




        }
    }

    public IEnumerator Activate()
    {
        yield return new WaitForSeconds(TimeBeforeActivation);
        CanMultiply = true;
        yield return null;
    }

 
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & portalLayer) != 0)
        {
            if (Enemy)
            {
                other.gameObject.GetComponent<PortalAreaManager>().DestroyPortal();
            }

            if (!HasAlreadyMultiplied && CanMultiply && !Enemy)
            {
                AudioManager.Instance.PlaySFX(audioClips[4], 0.07f, 1.4f);
                number.SetActive(true);
                HasAlreadyMultiplied = true;
                for (int i = 0; i < other.gameObject.GetComponentInParent<PortailMovement>().amount - 1; i++)
                {

                    Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
                    GameObject tempGO = Instantiate(gameObject, spawnPos, transform.rotation);
                    Instantiate(smokePart, transform.position, Quaternion.identity);
                    Rigidbody tempRb = tempGO.GetComponent<Rigidbody>();
                    CharacterBrain tempCB = tempGO.GetComponent<CharacterBrain>();
                    tempCB.Init(0.2f, false, true);
                    Animator anim = tempCB.animator;
                    anim.Rebind();     
                    anim.Update(0f);
                    tempRb.velocity = rb.velocity;
                    Debug.Log(tempRb.velocity);
                    tempCB.reducedVelocity = tempRb.velocity;
                }
            }

        }
        if (((1 << other.gameObject.layer) & DirectionChanger) != 0)
        {
            Debug.Log("Direciton");
            rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
            transform.rotation = Quaternion.Euler(0, -45f, 0);
            CameFromPipe = true;
        }
        if (((1 << other.gameObject.layer) & WaterLayer) != 0)
        {

                gameObject.layer = LayerMask.NameToLayer("Dead");

                Die();
         

        }
        if (((1 << other.gameObject.layer) & CanonTarget) != 0)
        {
            if (Enemy)
            {
               TargetCannon = true;

            }
        }


    }
    void OnCollisionEnter(Collision collision)
    {
        if (!Enemy)
        {
            if (((1 << collision.gameObject.layer) & floorLayer) != 0)
            {
                HasTOuchedFloor = true;
                HasTouchedDeadZone = false;
                rb.velocity = new Vector3(0, 0, rb.velocity.z);
                reducedVelocity = rb.velocity;
                reducedVelocity.z = 3.75f;
                rb.velocity = reducedVelocity;
            }
            if (((1 << collision.gameObject.layer) & objectLayer) != 0)
            {
                if (gameObject.layer != LayerMask.NameToLayer("Dead"))
                {
                    AudioManager.Instance.PlaySFX(audioClips[1], 0.07f);

                    gameObject.layer = LayerMask.NameToLayer("Dead");
                    Debug.Log("COlliding");
                    collision.gameObject.GetComponent<ObstacleManager>().OnHit();
                    StartCoroutine(WaitBeforeDie());
                }

            }
            if (((1 << collision.gameObject.layer) & PipeLayer) != 0)
            {
                AudioManager.Instance.PlaySFX(audioClips[4], 0.07f, 1.4f);

                Destroy(gameObject);
                collision.gameObject.GetComponent<PipeManager>().OnHit();
            }
            if (((1 << collision.gameObject.layer) & EnemyLayer) != 0)
            {

                if (gameObject.layer != LayerMask.NameToLayer("Enemy") && gameObject.layer != LayerMask.NameToLayer("Dead"))
                {
                    AudioManager.Instance.PlaySFX(audioClips[1], 0.03f);
                    myCollider.enabled = false;
                    gameObject.layer = LayerMask.NameToLayer("Dead");
                    collision.gameObject.GetComponent<CharacterBrain>().Die();
                    collision.gameObject.GetComponent<CharacterBrain>().myCollider.enabled = false;
                    Debug.Log(collision.gameObject.GetComponent<CharacterBrain>().Enemy);
                    Die();
                }

            }
            if (((1 << collision.gameObject.layer) & CastleLayer) != 0)
            {

                if (gameObject.layer != LayerMask.NameToLayer("Dead"))
                {
                    //AudioManager.Instance.PlaySFX(audioClips[1], 0.07f);

                    gameObject.layer = LayerMask.NameToLayer("Dead");
                    collision.gameObject.GetComponent<SpawnerLifeManager>().OnSpawnerHit();
                    StartCoroutine(WaitBeforeDie());
                }
            }


        }

        if (((1 << collision.gameObject.layer) & CanonLayer) != 0)
        {

            gameObject.layer = LayerMask.NameToLayer("Dead");
            collision.gameObject.GetComponent<CanonManager>().OnHit();
            Die();

        }

    }

    public void Die()
    {

        Died = true;
        animator.SetBool("Die", true);
        StartCoroutine(WaitBeforeDestroy());
    }
    public IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    public IEnumerator WaitBeforeDie()
    {
        Died = true;
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("Die", true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
