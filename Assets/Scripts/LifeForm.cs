using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeForm : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 currentDirection;
    public float maxDirectionChangeAngle;
    public float maxSpeed;
    public bool isMoving;
    public SpawnerManager spawnerManager; // Referencia al SpawnerManager

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        spawnerManager = FindObjectOfType<SpawnerManager>();
        currentDirection = GetRandomDirection();
    }

    public void TurnLeft()
    {
         currentDirection = Quaternion.Euler(0f, -maxDirectionChangeAngle, 0f) * currentDirection;
    }

    public void TurnRight()
    {
         currentDirection = Quaternion.Euler(0f, maxDirectionChangeAngle, 0f) * currentDirection;
    }

    public void Move()
    {
        if (isMoving)
        {
            rb.velocity = currentDirection * maxSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero; // Hacer la velocidad igual a cero cuando no se está moviendo
        }
    }

    public Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

}
