using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore : LifeForm
{
    public GameObject herbivorePrefab; // Prefab del Herbivore a instanciar
    public SpawnerManager spawnerManager; // Referencia al SpawnerManager

    public int raycastCount;
    public float spawnTimerDuration; // Duración del temporizador para instanciar un nuevo Herbivore
    public float spawnerTimer; // Temporizador actual
    public float detectionRange;
    public float killDistanceLimits;

    private void Update()
    {
        CastRays();
        Move();
        CheckHerbivoreInstanciateTimer();
    }

    public void CheckHerbivoreInstanciateTimer() 
    {
        if (!isMoving)
        {
            SpawnHervivoreTimeDiscounter();
        }
        else
        {
            ResetSpawnTimer();
        }
    }

    public void SpawnHervivoreTimeDiscounter() 
    {
        // Actualizar el temporizador
        spawnerTimer -= Time.deltaTime;
        if (spawnerTimer <= 0)
        {
            SpawnHerbivore(); // Instanciar un nuevo Herbivore
        }
    }

    public void OnDeath()
    {
        spawnerManager.herbivoreList.Remove(this.gameObject);
        Destroy(gameObject);
    }

    private void SpawnHerbivore()
    {
        ResetSpawnTimer(); // Reiniciar el temporizador
        GameObject newHerbivore = Instantiate(herbivorePrefab, transform.position, Quaternion.identity);
        spawnerManager.herbivoreList.Add(newHerbivore);
    }

    private void ResetSpawnTimer()
    {
        spawnerTimer = spawnTimerDuration;
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
                    //TODO Genera codigo para que valide si la distancia entre el punto donde choca el raycast y el emisor del mismo 
                    // es menos que el KillDistanceLimits

                    float distanceToWall = Vector3.Distance(transform.position, hit.point);
                    if (distanceToWall < killDistanceLimits)
                    {
                        Debug.Log("Distance to wall: " + distanceToWall);
                        Debug.Log("OnDeath");
                        OnDeath();
                    }

                    Debug.Log("Raycast golpeó una pared.");
                }
                else if (i >= 0 && i < 7 && hit.collider.CompareTag("Carnivore"))
                {
                    // Girar a la izquierda
                    Debug.Log("Está a tu derecha el Carnívoro");
                }
                else if (i >= 14 && i <= 20 && hit.collider.CompareTag("Carnivore"))
                {
                    // Girar a la derecha
                    Debug.Log("Está a tu izquierda el Carnívoro");
                }
            }
            // Dibujar el rayo en la escena
            Debug.DrawRay(transform.position, rayDirection * detectionRange, Color.red);

            // Incrementar el ángulo para el siguiente rayo
            currentAngle += angleIncrement;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + currentDirection * detectionRange);
    }
}
