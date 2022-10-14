using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoWars.Movement;
using CryptoWars.Resources;
using Mirror;

namespace CryptoWars.CustomPhysics
{
    public class Collisioner : NetworkBehaviour
    {
        [SerializeField] GameObject deathParticles = null;
        [SerializeField] float respawnTime = 5f;

        bool isInsideBG;
        bool respawning;
        Mover mover;
        Stats stats;
        PhysicsApplier physicsApplier;

        List<MeshRenderer> meshes = new List<MeshRenderer>();

        public bool IsInsideBG { get => isInsideBG; }

        public override void OnStartLocalPlayer()
        {
            mover = GetComponent<Mover>();
            stats = GetComponent<Stats>();
            physicsApplier = GetComponent<PhysicsApplier>();

            foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>()) { meshes.Add(mesh); }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isLocalPlayer) { return; }

            if (other.CompareTag("Limit"))
            {
                if (isInsideBG) { StartCoroutine(Respawn()); return; }
                isInsideBG = true;
            }

            if (other.CompareTag("Bullet")) { if (stats.TakeDamage() <= 0) StartCoroutine(Respawn()); }
        }

        private IEnumerator Respawn()//TODO change to Coroutine
        {
            if (respawning) { yield break; }

            respawning = true;

            CMDSpawnDeathParticles();
            mover.FreezeRigibody();

            foreach (MeshRenderer mesh in meshes) { mesh.enabled = false; }

            yield return new WaitForSeconds(respawnTime);

            mover.MoveToSpawnPoint();
            mover.UnfreezeRigidbody();
            physicsApplier.FlipZAxis(false);
            isInsideBG = false;
            stats.RestoreStats();
            foreach (MeshRenderer mesh in meshes) { mesh.enabled = true; }

            respawning = false;
        }

        [Command]
        private void CMDSpawnDeathParticles()
        {
            GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
            NetworkServer.Spawn(particles);
        }
    }
}