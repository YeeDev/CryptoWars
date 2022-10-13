using Mirror;
using UnityEngine;
using CryptoWars.AttackTypes;

namespace CryptoWars.Attacks
{
    public class Shooter : NetworkBehaviour
    {
        [SerializeField] GameObject bulletPrefab = null;
        [SerializeField] Transform muzzle = null;

        //TODO BulletPooler (Perhaps later)
        [Command]
        public void CmdShoot()
        {
            GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            NetworkServer.Spawn(bullet.gameObject);
        }
    }
}