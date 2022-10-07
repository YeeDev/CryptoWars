using Mirror;
using UnityEngine;

namespace CryptoWars.Attacks
{
    public class Shooter : NetworkBehaviour
    {
        [SerializeField] GameObject bulletPrefab = null;
        [SerializeField] Transform muzzle = null;

        //TODO BulletPooler
        //Needs to create a list of weapons bullets based on type
        [Command]
        public void CmdShoot()
        {
            GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            NetworkServer.Spawn(bullet.gameObject);
        }
    }
}