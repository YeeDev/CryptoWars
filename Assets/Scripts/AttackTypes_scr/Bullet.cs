using UnityEngine;
using CryptoWars.Currency;

namespace CryptoWars.AttackTypes
{
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
}