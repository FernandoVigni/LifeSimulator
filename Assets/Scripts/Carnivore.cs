using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivore : MonoBehaviour
{
    public float maxSpeed;
    private Rigidbody rb;
    public Herbivore herbivore;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Chase();
    }

    public void Chase()
    {
        Vector3 direction = (herbivore.transform.position - transform.position).normalized;
        rb.velocity = direction * maxSpeed;

        // Apunta hacia el herbívoro
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}