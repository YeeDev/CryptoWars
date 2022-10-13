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
        public string PlayerThatShoot { get => playerThatShoot; set => playerThatShoot = value; }

        private void Start() { rb.velocity = transform.forward * bulletspeed; }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.transform.name);
            Debug.Log(playerThatShoot);
            if (other.transform.name.ToString() == playerThatShoot) { return; }

            if (other.CompareTag("Currency")) { other.GetComponent<CryptoFile>().ReduceHealth(damage); }

            GameObject particles = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            NetworkServer.Spawn(particles);
            DestroySelf();
        }

        [Server]
        void DestroySelf() => NetworkServer.Destroy(gameObject);
    }
}