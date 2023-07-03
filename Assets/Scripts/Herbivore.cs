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
    public bool turnLeft;
    public bool turnRight;
    public bool walk;

    public float spawnTimerDuration; // Duración del temporizador para instanciar un nuevo Herbivore
    public float spawnTimer; // Temporizador actual

    public GameObject herbivorePrefab; // Prefab del Herbivore a instanciar

    public SpawnerManager spawnerManager; // Referencia al SpawnerManager

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

        // Actualizar el temporizador
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnHerbivore(); // Instanciar un nuevo Herbivore
        }
    }

    public void Move()
    {
        rb.velocity = currentDirection * maxSpeed;
    }

    public void CastRays()
    {
        float angleIncrement = 360f / raycastCount; // Calcular el incremento de ángulo entre los rayos
        float currentAngle = 0f; // Ángulo inicial

        // Obtener la rotación del personaje
        Quaternion rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        for (int i = 0; i < raycastCount; i++)
        {
            // Calcular la dirección del rayo en función del ángulo actual y la rotación del personaje
            Quaternion rayRotation = Quaternion.Euler(0f, currentAngle, 0f);
            Vector3 rayDirection = rotation * rayRotation * currentDirection;

            // Lanzar el rayo
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, detectionRange))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    Debug.Log("Raycast golpeó una pared. Girando...");
                    currentDirection = Quaternion.Euler(0f, maxDirectionChangeAngle + 1, 0f) * currentDirection;
                    break;
                }
                else if (i >= 0 && i < 7)
                {
                    // Girar a la izquierda
                    Debug.Log("Está a tu derecha el Carnívoro, deberías doblar a la izquierda");
                    currentDirection = Quaternion.Euler(0f, -maxDirectionChangeAngle + 1, 0f) * currentDirection;
                    break;
                }
                else if (i >= 14 && i <= 20)
                {
                    // Girar a la derecha
                    Debug.Log("Está a tu izquierda el Carnívoro, deberías doblar a la derecha");
                    currentDirection = Quaternion.Euler(0f, maxDirectionChangeAngle + 1, 0f) * currentDirection;
                    break;
                }
                // Si el rayo golpea algo, puedes hacer algo aquí, como cambiar la dirección del personaje
                // Por ejemplo, puedes imprimir el nombre del objeto golpeado
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            }

            // Dibujar el rayo en la escena
            Debug.DrawRay(transform.position, rayDirection * detectionRange, Color.red);

            // Incrementar el ángulo para el siguiente rayo
            currentAngle += angleIncrement;
        }
    }

    public void OnDeath()
    {
        
        spawnerManager.herbivoreList.Remove(this.gameObject);
        Destroy(gameObject);
    }

    public Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

    private void ResetSpawnTimer()
    {
        spawnTimer = spawnTimerDuration;
    }

    private void SpawnHerbivore()
    {
        ResetSpawnTimer(); // Reiniciar el temporizador
        GameObject newHerbivore = Instantiate(herbivorePrefab, transform.position, Quaternion.identity);
        spawnerManager.herbivoreList.Add(newHerbivore);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + currentDirection * detectionRange);
    }
}
