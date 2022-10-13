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

        string playerThatShoot;
        Collider col;
        public string PlayerThatShoot { get => playerThatShoot; set => playerThatShoot = value; }

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
            if (other.CompareTag("Currency")) { other.GetComponent<CryptoFile>().ReduceHealth(damage); }

            GameObject particles = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            NetworkServer.Spawn(particles);
            DestroySelf();
        }

        [Server]
        void DestroySelf() => NetworkServer.Destroy(gameObject);
    }
}