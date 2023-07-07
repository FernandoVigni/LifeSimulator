using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivore : LifeForm
{
    public float detectionAngle;
    public float detectionRange;
    public float killDistance; 

    private void Update()
    {
        DetectHerbivore();
        Move();
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
                    Herbivore herbivore = hitForward.collider.GetComponent<Herbivore>();
                    if (herbivore != null)
                    {
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
                Debug.Log("Raycast golpeó una pared...");
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
                    Herbivore herbivore = hitLeft.collider.GetComponent<Herbivore>();
                    if (Vector3.Distance(transform.position, hitLeft.transform.position) <= killDistance)
                    {
                        herbivore.OnDeath();
                        Debug.Log("Herbivore ha sido eliminado");
                    }
                    currentDirection = direction;
                    return; // Cambiar la dirección hacia el Herbivore detectado
                }

                if (hitLeft.collider.CompareTag("Wall"))
                {
                    Debug.Log("Raycast golpeó una pared...");
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
                    Herbivore herbivore = hitRight.collider.GetComponent<Herbivore>();
                    if (Vector3.Distance(transform.position, hitRight.transform.position) <= killDistance)
                    {
                        herbivore.OnDeath();
                        Debug.Log("Herbivore ha sido eliminado");
                    }

                    if (hitRight.collider.CompareTag("Wall"))
                    {
                        Debug.Log("Raycast golpeó una pared...");
                    }

                    currentDirection = direction;
                    return; // Cambiar la dirección hacia el Herbivore detectado
                }
            }
            Debug.DrawRay(transform.position, direction * detectionRange, Color.red);
        }
    }
}