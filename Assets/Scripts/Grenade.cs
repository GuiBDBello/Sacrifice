using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject ExplosionEffect;
    public float ExplosionDelay = 3.0f;
    public float ExplosionForce = 700.0f;
    public float ExplosionRadius = 5.0f;

    private float explosionCountdown;
    private bool hasExploded = false;

    void Start()
    {
        explosionCountdown = ExplosionDelay;
    }

    void Update()
    {
        explosionCountdown -= Time.deltaTime;
        if (explosionCountdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode() {
        Debug.Log("BOOM!");

        Instantiate(ExplosionEffect, transform.position, transform.rotation);

        Collider[] collidersToHit = Physics.OverlapSphere(transform.position, ExplosionRadius);

        foreach (Collider nearbyObject in collidersToHit)
        {
            if (nearbyObject.gameObject.tag == "Player")
            {
                PlayerController playerController = nearbyObject.gameObject.GetComponent<PlayerController>();
                playerController.Die();
            }
        }

        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, ExplosionRadius);

        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
            }
        }
        
        Destroy(gameObject);
    }
}
