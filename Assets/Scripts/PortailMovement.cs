using TMPro;
using UnityEngine;

public class PortailMovement : MonoBehaviour
{



    [SerializeField] public float leftBound = -5f;  
    [SerializeField] public float rightBound = 5f;   
    [SerializeField] public float speed = 2f;     
    [SerializeField] public bool moving = false;       

    [SerializeField] public TextMeshPro multiplier;       
    [SerializeField] public int amount;         

    private bool right = true;
    void Awake()
    {
       
        string display = "X" + amount;
        multiplier.text = display;
    }
    void Update()
    {
        if (moving)
        {
            if (right)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (transform.position.x >= rightBound)
                    right = false;
            }
            else
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                if (transform.position.x <= leftBound)
                    right = true;
            }
        }
     
    }

}
