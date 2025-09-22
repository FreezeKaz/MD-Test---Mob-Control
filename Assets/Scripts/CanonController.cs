using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 10f;  
    [SerializeField] public float minX = -8f;       
    [SerializeField] public float maxX = 8f;       


    [SerializeField] public GameObject bulletPrefab; // c prefab
    [SerializeField] public Transform firePoint;     // charac spawn
    [SerializeField] public float characSpeed = 10f; // How fast the character fly
    [SerializeField] public float fireRate = 0.5f;   // Time between shots

    private float nextFireTime = 0f;

    private Vector3 touchPosition;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
          
        }
        // Only consider if there's a touch
        if (Input.touchCount > 0)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
            Touch touch = Input.GetTouch(0);

            // Convert touch position to world coordinates
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.WorldToScreenPoint(transform.position).z));

            // Keep the Y and Z the same, only move X
            touchPosition = new Vector3(Mathf.Clamp(touchPos.x, minX, maxX), transform.position.y, transform.position.z);

            // Smoothly move the cannon to the target X position
            transform.position = Vector3.Lerp(transform.position, touchPosition, moveSpeed * Time.deltaTime);
        }
    }


    public void Shoot()
    {
        GameObject temp = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = temp.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * characSpeed;
            Debug.Log(rb.velocity);
        }

    }
}