using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoFile : MonoBehaviour
{
    [SerializeField] int health = 5;
    [SerializeField] float loweredHeightAmount = 0.5f;

    //TODO Refactor this, POSSNewName TakeDamage
    public void ReduceHealth(int damage)
    {
        health -= damage;

        Vector3 loweredPosition = transform.position;
        loweredPosition.y -= loweredHeightAmount;
        transform.position = loweredPosition;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
