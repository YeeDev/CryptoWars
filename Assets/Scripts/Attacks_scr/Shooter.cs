using Mirror;
using UnityEngine;
using CryptoWars.AttackTypes;
using CryptoWars.Resources;

namespace CryptoWars.Attacks
{
    public class Shooter : NetworkBehaviour
    {
        [SerializeField] GameObject bulletPrefab = null;
        [SerializeField] Transform muzzle = null;
        [SerializeField] AudioSource audioSource = null;

        [Command]
        public void CmdShoot(GameObject playerID)
        {
            GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            bullet.GetComponent<Bullet>().SetPlayer = playerID;
            NetworkServer.Spawn(bullet);
            audioSource.Play();
        }
    }
}