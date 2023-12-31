using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Carnivore : LifeForm
{
    float distanceToWall;


    public float detectionAngle;
    public float detectionRange;
    public float killDistance;
    public float runEnergy;
    public float runEnergyReset;
    public float currentDigestion;
    public float resetDigestionTimer;
    public float energyToSplit;

    private void Update()
    {
        DetectHerbivore();
        SpendEnergyToMove();
        Move();
        DecreseDigestionTimer();
    }

    public void SpendEnergyToMove() 
    {
        if(isMoving)
        {
            DecreseEnergy(); 
        }
    }

    public void DecreseEnergy() 
    {
        runEnergy -= Time.deltaTime;
        if (runEnergy <= 0) 
        {
            spawnerManager.carnivoreList.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void DecreseDigestionTimer() 
    {
        if (currentDigestion > 0) 
        {
            currentDigestion -= Time.deltaTime;
        }
    }

    public void ReseteDigestionTimer() 
    {
        currentDigestion = resetDigestionTimer;
    }

    public void IncreseSpawnerCarnivoreEnergy() 
    {
        energyToSplit += 50;
        if (energyToSplit >= 100) 
        {
            energyToSplit = 0;
        }
    }

    public void ResetRunEnergy() 
    {
        runEnergy = runEnergyReset;
    }

    public void DetectHerbivore()
    {
        float halfDetectionAngle = detectionAngle / 2f;

        // Raycast hacia adelante
        RaycastHit hitForward;
        if (Physics.Raycast(transform.position, currentDirection, out hitForward, detectionRange))
        {
            if (hitForward.collider.gameObject.CompareTag("Herbivore"))
            {
                Debug.Log("Hay un Herbivore al frente");

                if (Vector3.Distance(transform.position, hitForward.transform.position) <= killDistance && currentDigestion <= 0)
                {
                    Herbivore herbivore = hitForward.collider.gameObject.GetComponent<Herbivore>();
                    if (herbivore != null)
                    {
                        DigestionValidation();
                        herbivore.OnDeath();
                        Debug.Log("Herbivore ha sido eliminado");
                    }
                    else
                    {
                        Debug.Log("El objeto detectado no tiene el componente Herbivore");
                    }
                }
                return; // Continuar caminando hacia adelante si hay un Herbivore
            }
            else if (hitForward.collider.CompareTag("Wall"))
            {
                distanceToWall = hitForward.distance;
                if (distanceToWall < 3)
                {
                    OnDeath();
                }
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
                if (hitLeft.collider.gameObject.CompareTag("Herbivore"))
                {
                    Debug.Log("Hay un Herbivore a la izquierda");
                    Herbivore herbivore = hitLeft.collider.gameObject.GetComponent<Herbivore>();
                    if (Vector3.Distance(transform.position, hitLeft.transform.position) <= killDistance && currentDigestion <= 0)
                    {
                        DigestionValidation();
                        herbivore.OnDeath();
                        Debug.Log("Herbivore ha sido eliminado");
                    }
                    return; // Continuar caminando hacia adelante si hay un Herbivore a la izquierda
                }
                else if (hitLeft.collider.CompareTag("Wall"))
                {
                    distanceToWall = hitLeft.distance;
                    if (distanceToWall < 3)
                    {
                        OnDeath();
                    }
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
                if (hitRight.collider.gameObject.CompareTag("Herbivore"))
                {
                    Debug.Log("Hay un Herbivore a la derecha");
                    Herbivore herbivore = hitRight.collider.gameObject.GetComponent<Herbivore>();
                    if (Vector3.Distance(transform.position, hitRight.transform.position) <= killDistance)
                    {
                        DigestionValidation();
                        herbivore.OnDeath();
                        Debug.Log("Herbivore ha sido eliminado");
                    }
                    return; // Continuar caminando hacia adelante si hay un Herbivore a la derecha
                }
                else if (hitRight.collider.CompareTag("Wall"))
                {
                    distanceToWall = hitRight.distance;
                    if (distanceToWall < 3)
                    {
                        OnDeath();
                    }
                }
            }
            Debug.DrawRay(transform.position, direction * detectionRange, Color.red);
        }
    }

    public void DigestionValidation() 
    {
        if (currentDigestion <= 0)
        {
            ResetRunEnergy();
            IncreseSpawnerCarnivoreEnergy();
        }
        ReseteDigestionTimer();
    }

    public void OnDeath()
    {
        spawnerManager.carnivoreList.Remove(this.gameObject);
        Destroy(gameObject);
    }
}