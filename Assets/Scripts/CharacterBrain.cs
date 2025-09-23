using System.Collections;
using UnityEngine;
using static ToonyColorsPro.ShaderGenerator.Enums;

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
    [SerializeField] private Animator animator;
    [SerializeField] private Material MobMaterial;
    [SerializeField] private SkinnedMeshRenderer render;
    [SerializeField] private Collider myCollider;

    [SerializeField] private float minDistance = 0.5f; // security distance
    public LayerMask unitLayer;
    public bool HasTOuchedFloor = false;
    public Vector3 reducedVelocity = Vector3.zero;

    public void Init(float time, bool pipe, bool floor)
    {
        TimeBeforeActivation = time;
        CameFromPipe = pipe;
        HasTOuchedFloor = floor; ;

        StartCoroutine(Activate());

    }
    void Update()
    {

        if (HasTOuchedFloor && !Enemy && !Died)
        {
            rb.velocity = new Vector3(0, 0, 7f);
        }
        if(Enemy && !Died)
            rb.velocity = new Vector3(0, 0, -7f);
        if (Died)
            rb.velocity = Vector3.zero;
    }

    public IEnumerator Activate()
    {
        yield return new WaitForSeconds(TimeBeforeActivation);
        CanMultiply = true;
        yield return null;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (!HasAlreadyMultiplied && CanMultiply && !Enemy)
        {
            HasAlreadyMultiplied = true;
            for (int i = 0; i < other.gameObject.GetComponentInParent<PortailMovement>().amount - 1; i++)
            {
                Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
                GameObject tempGO = Instantiate(gameObject, spawnPos, transform.rotation);
                Rigidbody tempRb = tempGO.GetComponent<Rigidbody>();
                CharacterBrain tempCB = tempGO.GetComponent<CharacterBrain>();
                tempCB.Init(0.6f, false, true);
                tempRb.velocity = rb.velocity;
                Debug.Log(tempRb.velocity);
                tempCB.reducedVelocity = tempRb.velocity;
            }
        }


    }
    void OnCollisionEnter(Collision collision)
    {
        if(!Enemy)
        {
            if (((1 << collision.gameObject.layer) & floorLayer) != 0)
            {
                HasTOuchedFloor = true;

                rb.velocity = new Vector3(0, 0, rb.velocity.z);
                reducedVelocity = rb.velocity;
                reducedVelocity.z = 3.75f;
                rb.velocity = reducedVelocity;
            }
            if (((1 << collision.gameObject.layer) & objectLayer) != 0)
            {
                if (gameObject.layer != LayerMask.NameToLayer("Dead"))
                {
                    gameObject.layer = LayerMask.NameToLayer("Dead");
                    Debug.Log("COlliding");
                    collision.gameObject.GetComponent<ObstacleManager>().OnHit();
                    StartCoroutine(WaitBeforeDie());
                }

            }
            if (((1 << collision.gameObject.layer) & PipeLayer) != 0)
            {
                Destroy(gameObject);
                collision.gameObject.GetComponent<PipeManager>().OnHit();
            }
            if (((1 << collision.gameObject.layer) & EnemyLayer) != 0)
            {

                if (gameObject.layer != LayerMask.NameToLayer("Enemy") && gameObject.layer != LayerMask.NameToLayer("Dead"))
                {
                    myCollider.enabled = false;
                    gameObject.layer = LayerMask.NameToLayer("Dead");
                    collision.gameObject.GetComponent<CharacterBrain>().Die();
                    collision.gameObject.GetComponent<CharacterBrain>().myCollider.enabled = false;
                    Debug.Log(collision.gameObject.GetComponent<CharacterBrain>().Enemy);
                    Die();
                }
                
            }
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
