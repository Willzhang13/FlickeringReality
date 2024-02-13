using Cyan;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class EnemyLogic : MonoBehaviour
{
    public Player player;

    public Animator animator;

    public GameObject[] patrolPoints;
    public float waitTime;
    public int currentPointIndex;
    private bool playerVisible;
    private bool inLight = false;
    private Vector3 lightPos = new Vector3();
    public bool isAttacking;

    public bool Arrived;

    public AIDestinationSetter destinationSetter;
    [SerializeField] private GameObject target;

    public float attackRadius = 4;

    bool once;

    private LayerMask layers;
    private LayerMask lightCheck;


    void Start()
    {
        layers = LayerMask.GetMask("Player", "Walls");
        lightCheck = LayerMask.GetMask("Enemy", "Walls");
        currentPointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        stateSwitch();
        checkVisibility();

        if (inLight || playerVisible) {
            transform.GetChild(1).gameObject.SetActive(true);
        } else {
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    void stateSwitch()
    {
        float playerDistance = Vector3.Distance(player.GetComponent<Transform>().position, transform.position);
        if ((playerVisible == true & playerDistance <= attackRadius))
        {
            isAttacking = true;
        }

        if (isAttacking)
        {
            //transform.position = Vector2.MoveTowards(transform.position, player.GetComponent<Transform>().position, runSpeed * Time.deltaTime);
            
            //The Joy of hacking deep copies
            if (playerVisible) {
                target.transform.position = player.transform.position;
            }

            destinationSetter.target = target.transform;
            

            if (Arrived) {
                isAttacking = false;
                if (once == false)
                {
                    once = true;
                    animator.SetFloat("speed", 0);
                    StartCoroutine(Wait());
                }
            }

            // Determines movement direction for animator
            Vector2 direction = (transform.position - player.GetComponent<Transform>().position).normalized;
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", direction.y);
            animator.SetFloat("speed", 1);

            //exclamation mark
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (i == currentPointIndex)
                {
                    patrolPoints[i].GetComponent<Collider2D>().enabled = true;
                }
                else
                {
                    patrolPoints[i].GetComponent<Collider2D>().enabled = false;
                }
            }

            if (!Arrived & !isAttacking)
            {
                //transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].GetComponent<Transform>().position, runSpeed * Time.deltaTime);
                destinationSetter.target = patrolPoints[currentPointIndex].transform;
                // Determines movement direction for animator
                Vector2 direction = (transform.position - patrolPoints[currentPointIndex].GetComponent<Transform>().position).normalized;
                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);
                animator.SetFloat("speed", 1);
            } else {
                if (once == false)
                {
                    once = true;
                    animator.SetFloat("speed", 0);
                    StartCoroutine(Wait());
                }
            }
        }
    }

    void checkVisibility()
    {
        Vector3 playerDistanceVector = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDistanceVector.normalized, 20f, layers);
        if (hit.collider != null && hit.collider.gameObject.tag == "Player" && player.amVisible)
        {
            playerVisible = true;
        }
        else
        {
            playerVisible = false;
        }
        
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(waitTime);
        if (currentPointIndex + 1 < patrolPoints.Length) {
            currentPointIndex++;
        } else {
            currentPointIndex = 0;
        }
        once = false;
        Arrived = false;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (isAttacking && c.gameObject.tag == "PlayerTracker") {
            Arrived = true;
        } else if (c.gameObject.tag == "Patrol")
        {
            Arrived = true;
        }
    }
    void OnTriggerStay2D(Collider2D c) {
        if (c.gameObject.tag == "LightSource" && c.gameObject.layer != 7)
        {
            Vector3 lightDistanceVector = transform.position - c.gameObject.transform.position;
            //Debug.Log(transform.position + " " + collision.gameObject.transform.position + " " + lightDistanceVector);
            RaycastHit2D hit = Physics2D.Raycast(c.gameObject.transform.position, lightDistanceVector.normalized, 20f, lightCheck);
            if (hit.collider != null && hit.collider.gameObject.tag != "Wall" && hit.collider.gameObject.tag == "Enemy")
            {
                inLight = true;
                lightPos = transform.position;
            }
        }
    }

    void OnTriggerExit2D(Collider2D c) {
        if (c.gameObject.tag == "LightSource" && c.gameObject.layer != 7)
        {
            inLight = false;
        }
    }

}
