using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivore : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float detectionAngle = 25f;
    public float detectionRange = 10f;
    public float killDistance = 3f; // Nueva variable para la distancia de matar
    private Rigidbody rb;
    private Vector3 currentDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentDirection = transform.forward;
    }

    private void Update()
    {
        Move();
        DetectHerbivore();
    }

    public void Move()
    {
        rb.velocity = currentDirection * maxSpeed;
    }

    public void DetectHerbivore()
    {
        float halfDetectionAngle = detectionAngle / 2f;

        // Raycast hacia adelante
        RaycastHit hitForward;
        if (Physics.Raycast(transform.position, currentDirection, out hitForward, detectionRange))
        {
            if (hitForward.collider.CompareTag("Herbivore"))
            {
                Debug.Log("Hay un Herbivore al frente");
                if (Vector3.Distance(transform.position, hitForward.transform.position) <= killDistance)
                {
                    Destroy(hitForward.collider.gameObject);
                    Debug.Log("Herbivore ha sido eliminado");
                }
                return; // Continuar caminando hacia adelante si hay un Herbivore
            }
        }
        Debug.DrawRay(transform.position, currentDirection * detectionRange, Color.red);

        // Raycasts hacia la izquierda
        for (int i = 1; i <= 2; i++)
        {
            float angle = -halfDetectionAngle * i;
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 direction = rotation * currentDirection;

            RaycastHit hitLeft;
            if (Physics.Raycast(transform.position, direction, out hitLeft, detectionRange))
            {
                if (hitLeft.collider.CompareTag("Herbivore"))
                {
                    Debug.Log("Hay un Herbivore a la izquierda");
                    if (Vector3.Distance(transform.position, hitLeft.transform.position) <= killDistance)
                    {
                        Destroy(hitLeft.collider.gameObject);
                        Debug.Log("Herbivore ha sido eliminado");
                    }
                    currentDirection = direction;
                    return; // Cambiar la dirección hacia el Herbivore detectado
                }
            }
            Debug.DrawRay(transform.position, direction * detectionRange, Color.red);
        }

        // Raycasts hacia la derecha
        for (int i = 1; i <= 2; i++)
        {
            float angle = halfDetectionAngle * i;
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 direction = rotation * currentDirection;

            RaycastHit hitRight;
            if (Physics.Raycast(transform.position, direction, out hitRight, detectionRange))
            {
                if (hitRight.collider.CompareTag("Herbivore"))
                {
                    Debug.Log("Hay un Herbivore a la derecha");
                    if (Vector3.Distance(transform.position, hitRight.transform.position) <= killDistance)
                    {
                        Destroy(hitRight.collider.gameObject);
                        Debug.Log("Herbivore ha sido eliminado");
                    }
                    currentDirection = direction;
                    return; // Cambiar la dirección hacia el Herbivore detectado
                }
            }
            Debug.DrawRay(transform.position, direction * detectionRange, Color.red);
        }
    }
}