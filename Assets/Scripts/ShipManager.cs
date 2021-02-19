using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public GameObject projectile;
    float projectileSpeed = 400;
    bool fired = false;

    float moveTimerMax = 6;
    float moveTimer;
    float moveSpeed = 200;

    List<GameObject> players;
    int currentTargetIndex = 0;
    GameObject currentTarget;

    void Start()
    {
        players = new List<GameObject>();
        moveTimer = moveTimerMax;
    }

    void Update()
    {
        // TODO: Instead of this (inefficient), have Players add themselves to this list on spawn with AddToList() to avoid duplication.
        players = new List<GameObject>(); 
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) 
        {
            players.Add(g);
        }
        //

        // Picks a random player to target. Might need some sort of timer so each ship sticks on one player for longer?
        currentTargetIndex = Random.Range(0, players.Count);
        currentTarget = players[currentTargetIndex];

        // Always faces the targeted player.
        if (currentTarget != null)
        {
            transform.LookAt(currentTarget.transform);
        }

        // Move towards the player to stay within a certain range.
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0)
        {
            fired = false; // Reset fire timer.
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * moveSpeed);

            moveTimer = moveTimerMax;
        }
        // Stops movement if too close to player.
        if (Vector3.Distance(currentTarget.transform.position, transform.position) < 4)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        // Stops movement (to prevent drift) and fires before next movement.
        if (moveTimer < 1 && moveTimer > 0)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            if (!fired)
            {
                GameObject currentProjectile = Instantiate(projectile, transform.position + Vector3.Normalize(transform.forward), Quaternion.identity);
                currentProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
                Destroy(currentProjectile, 10); // Destroy projectile after its 10 second lifespan.
                fired = true;
            }
        }
    }

    public void AddToList(GameObject g)
    {
        players.Add(g);
    }
}
