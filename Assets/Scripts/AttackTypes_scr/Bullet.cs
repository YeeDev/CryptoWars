using Mirror;
using UnityEngine;
using CryptoWars.Currency;
using CryptoWars.Resources;

namespace CryptoWars.AttackTypes
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] int damage = 1;
        [SerializeField] float bulletspeed = 40f;
        [SerializeField] Rigidbody rb = null;
        [SerializeField] GameObject explosionParticles = null;

        Collider col;
        Stats playerStats;

        public Stats SetTransform { set => playerStats = value; }

        private void Start()
        {
            rb.velocity = transform.forward * bulletspeed;
            col = GetComponent<Collider>();
            Invoke("StartCollider", 0.1f);
        }

        private void StartCollider() => col.enabled = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Currency"))
            {
                CryptoFile file = other.GetComponent<CryptoFile>();
                file.CmdReduceHealth(damage, playerStats);
            }

            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}