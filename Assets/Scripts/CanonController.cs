using System.Collections;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 10f;
    [SerializeField] public float minX = -8f;
    [SerializeField] public float maxX = 8f;
    [SerializeField] public float targetZ = -18f;

    public bool canTransport = false;

    [SerializeField] public GameObject characPrefab; // c prefab
    [SerializeField] public Transform firePoint;     // charac spawn
    [SerializeField] public float characSpeed = 10f; // How fast the character fly
    [SerializeField] public float fireRate = 0.5f;   // Time between shots
    [SerializeField] public Animator animator;
    [SerializeField] public Animator WheelAnimator;
    [SerializeField] public AudioSource sound;
    [SerializeField] public ObstacleManager firstObMan;
    [SerializeField] public CameraFollow camManager;
    [SerializeField] public GameObject MuzzleEffect;
    public bool canAct = true;

    private float nextFireTime = 0f;

    private Vector3 touchPosition;

    void Update()
    {


        if (canAct)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                MuzzleEffect.SetActive(true);
                animator.SetBool("Shooting", true);
                sound.enabled = true;
                if (Time.time >= nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + fireRate;
                }
            }
            else
            {
                MuzzleEffect.SetActive(false);
                animator.SetBool("Shooting", false);

                sound.enabled = false;
            }


            if (Input.GetMouseButton(0))
            {
                WheelAnimator.SetBool("Move", true);
                Vector3 mousePos = Input.mousePosition;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(
                    new Vector3(mousePos.x, mousePos.y, Camera.main.WorldToScreenPoint(transform.position).z)
                );

                Vector3 targetPos = new Vector3(
                    Mathf.Clamp(worldPos.x, minX, maxX),
                    transform.position.y,
                    transform.position.z
                );

                transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
            }
            else
            {
                WheelAnimator.SetBool("Move", false);
            }
        }
        else
        {
            MuzzleEffect.SetActive(false);
            WheelAnimator.SetBool("Move", false);
        }
         

    }


    public void Shoot()
    {
        GameObject temp = Instantiate(characPrefab, firePoint.position, firePoint.rotation);
        temp.GetComponent<CharacterBrain>().Init(0f, false, false);
        Rigidbody rb = temp.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * characSpeed;
        }

    }

    private void LateUpdate()
    {
        if (canTransport)
        {
            if (Mathf.Abs(transform.position.z - targetZ) <= 2f)
            {
                canTransport = false;
                animator.SetBool("Move", false);
                StartCoroutine(WaitBeforeAct());
            }
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, targetZ), 1.5f * Time.deltaTime);
        }

      



    }

    public void HasFinishedTransformation()
    {
        canTransport = true;
        camManager.ChangeView = true;
    }
    public IEnumerator WaitBeforeAct()
    {
        yield return new WaitForSeconds(1f);
        canAct = true;
        fireRate = 0.07f;
        characSpeed = 15f;

    }
}