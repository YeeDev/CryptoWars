using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Currency"))
        {
            other.GetComponent<CryptoFile>().ReduceHealth(damage);
            Destroy(gameObject);
        }
    }
}
