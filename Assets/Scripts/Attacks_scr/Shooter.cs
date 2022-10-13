using Mirror;
using UnityEngine;

namespace CryptoWars.Attacks
{
    public class Shooter : NetworkBehaviour
    {
        [SerializeField] GameObject bulletPrefab = null;
        [SerializeField] Transform muzzle = null;
        [SerializeField] AudioSource audioSource = null;

        //TODO BulletPooler (Perhaps later)
        [Command]
        public void CmdShoot()
        {
            GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            audioSource.Play();
            NetworkServer.Spawn(bullet.gameObject);
        }
    }
}