using System.Collections;
using UnityEngine;
using CryptoWars.Movement;
using CryptoWars.Resources;
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
        }

        private IEnumerator Respawn()//TODO change to Coroutine
        {
            CMDSpawnDeathParticles();

            yield return new WaitForSeconds(1.5f);

            mover.ResetSpeed();
            mover.MoveToSpawnPoint();
            physicsApplier.FlipZAxis(false);
            isInsideBG = false;
            mover.InitializeRotation();
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