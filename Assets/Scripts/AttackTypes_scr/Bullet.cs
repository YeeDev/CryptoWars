using Mirror;
using UnityEngine;
using CryptoWars.Currency;

namespace CryptoWars.AttackTypes
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] int damage = 1;
        [SerializeField] float bulletspeed = 40f;
        [SerializeField] Rigidbody rb = null;

        private void Start()
        {
            rb.velocity = transform.forward * bulletspeed;
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Currency"))
            {
                other.GetComponent<CryptoFile>().ReduceHealth(damage);
                DestroySelf();
            }
        }

        [Server]
        void DestroySelf() => NetworkServer.Destroy(gameObject);
    }
}