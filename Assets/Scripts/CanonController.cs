using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 10f;  
    [SerializeField] public float minX = -8f;       
    [SerializeField] public float maxX = 8f;       


    [SerializeField] public GameObject characPrefab; // c prefab
    [SerializeField] public Transform firePoint;     // charac spawn
    [SerializeField] public float characSpeed = 10f; // How fast the character fly
    [SerializeField] public float fireRate = 0.5f;   // Time between shots
    [SerializeField] public Animator animator;

    private float nextFireTime = 0f;

    private Vector3 touchPosition;

    void Update()
    {



        if (Input.GetKey(KeyCode.Space))
        {

            animator.SetBool("Shooting",true);
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
        else 
            animator.SetBool("Shooting",false);


        if (Input.GetMouseButton(0)) // left mouse held down
        {
            Vector3 mousePos = Input.mousePosition;

            // Convert mouse position to world coordinates
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(mousePos.x, mousePos.y, Camera.main.WorldToScreenPoint(transform.position).z)
            );

            // Keep Y and Z the same, only move X
            Vector3 targetPos = new Vector3(
                Mathf.Clamp(worldPos.x, minX, maxX),
                transform.position.y,
                transform.position.z
            );

            // Smoothly move the cannon to the target X position
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
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
}