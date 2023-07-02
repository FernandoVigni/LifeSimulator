using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore : MonoBehaviour
{
    public float maxSpeed;
    public float detectionRange;
    public int raycastCount;
    public float changeDirectionInterval;
    public float maxDirectionChangeAngle;

    private Rigidbody rb;
    private Vector3 currentDirection;
    private float changeDirectionTimer;
    private float currentAngle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        changeDirectionTimer = changeDirectionInterval;
        currentDirection = GetRandomDirection();
    }

    private void Update()
    {
        Move();
        CastRays();
        UpdateDirection();
    }

    public void Move()
    {
        rb.velocity = currentDirection * maxSpeed;
    }

    public void CastRays()
    {
        // Disparar los raycasts en direcciones equidistantes
        float angleStep = 360f / raycastCount;
        for (int i = 0; i < raycastCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, detectionRange))
            {
                if (hit.collider.CompareTag("Carnivore"))
                {
                    // Realizar acciones en respuesta al encuentro con un personaje carnívoro
                    // Puedes implementar el comportamiento específico aquí, como huir o interactuar con el carnívoro
                    // Recuerda que esto es solo un ejemplo y deberás adaptarlo a tu lógica específica.
                    Debug.Log("Encuentro con un personaje carnívoro");
                }
            }
            // Dibujar el rayo en la escena
            Debug.DrawRay(transform.position, direction * detectionRange, Color.red);

        }
    }

    public void UpdateDirection()
    {
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0f)
        {
            float angle = Random.Range(-maxDirectionChangeAngle, maxDirectionChangeAngle);

            // Obtener el ángulo resultante dentro del rango de ±30 grados respecto al ángulo actual
            float newAngle = currentAngle + angle;
            newAngle = Mathf.Clamp(newAngle, currentAngle - 30f, currentAngle + 30f);

            // Convertir el ángulo resultante en una nueva dirección
            Quaternion rotation = Quaternion.Euler(0f, newAngle, 0f);
            currentDirection = rotation * transform.forward;

            changeDirectionTimer = changeDirectionInterval;
        }
    }

    public Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + currentDirection * detectionRange);
    }
}
