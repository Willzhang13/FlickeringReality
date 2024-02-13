using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float runSpeed = 3f;


    private Vector2 moveDirection;

    private Rigidbody2D rb;
    private Scene currentScene;

    public Animator animator;

    public bool amVisible = false;
    //Tracks whether we have updated visibility this tick.
    private Vector3 lightPos = new Vector3();
    private LayerMask layers;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentScene = SceneManager.GetActiveScene();
        layers = LayerMask.GetMask("Player", "Walls");
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY);

        if (moveDirection.SqrMagnitude() > 0.01)
        {
            animator.SetFloat("horizontal", moveX);
            animator.SetFloat("vertical", moveY);
        }
        animator.SetFloat("speed", moveDirection.SqrMagnitude());
        
    }
    void FixedUpdate() {
        if (lightPos != transform.position) {
            amVisible = false;
        }

        rb.MovePosition(rb.position + moveDirection * runSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene(currentScene.name);
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "LightSource")
        {
            Vector3 lightDistanceVector = transform.position - collision.gameObject.transform.position;
            //Debug.Log(transform.position + " " + collision.gameObject.transform.position + " " + lightDistanceVector);
            RaycastHit2D hit = Physics2D.Raycast(collision.gameObject.transform.position, lightDistanceVector.normalized, 5f, layers);
            if (hit.collider != null && hit.collider.gameObject.tag != "Wall" && hit.collider.gameObject.tag == "Player")
            {
                amVisible = true;
                lightPos = transform.position;
            }
        }
    }
}
