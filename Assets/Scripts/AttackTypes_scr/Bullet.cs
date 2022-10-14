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
        [SerializeField] GameObject explosionParticles = null;

        Collider col;
        GameObject playerId;

        public GameObject SetPlayer { set => playerId = value; }

        private void Start()
        {
            rb.velocity = transform.forward * bulletspeed;
            col = GetComponent<Collider>();
            Invoke("StartCollider", 0.1f);
        }

        private void StartCollider() => col.enabled = true;

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Currency"))
            {
                CryptoFile file = other.GetComponent<CryptoFile>();
                file.ReduceHealth(damage, playerId);
            }

            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            DestroySelf();
        }

        [Server]
        private void DestroySelf() => NetworkServer.Destroy(gameObject);
    }
}