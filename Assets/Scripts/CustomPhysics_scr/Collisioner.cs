using System.Collections;
using UnityEngine;
using CryptoWars.Movement;
using CryptoWars.Resources;
using CryptoWars.AttackTypes;
using Mirror;

namespace CryptoWars.CustomPhysics
{
    public class Collisioner : NetworkBehaviour
    {
        [SerializeField] GameObject deathParticles = null;

        bool isInsideBG;
        Mover mover;
        Stats stats;
        PhysicsApplier physicsApplier;

        public bool IsInsideBG { get => isInsideBG; }

        public override void OnStartLocalPlayer()
        {
            mover = GetComponent<Mover>();
            stats = GetComponent<Stats>();
            physicsApplier = GetComponent<PhysicsApplier>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isLocalPlayer) { return; }

            if (other.CompareTag("Limit"))
            {
                if (isInsideBG) { StartCoroutine(Respawn()); return; }
                isInsideBG = true;
            }

            if (other.CompareTag("Bullet"))
            {
                if (other.GetComponent<Bullet>().PlayerThatShoot == transform.name) { return; }
                if (stats.TakeDamage() <= 0) { StartCoroutine(Respawn()); }
            }
        }

        private IEnumerator Respawn()//TODO change to Coroutine
        {
            CMDSpawnDeathParticles();

            yield return new WaitForSeconds(1.5f);

            mover.MoveToSpawnPoint();
            physicsApplier.FlipZAxis(false);
            isInsideBG = false;
            stats.ModifyFuelStat(stats.MaxFuel);
        }

        [Command]
        private void CMDSpawnDeathParticles()
        {
            GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
            NetworkServer.Spawn(particles);
        }
    }
}