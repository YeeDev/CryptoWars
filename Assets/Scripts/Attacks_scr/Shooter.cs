using Mirror;
using UnityEngine;

namespace CryptoWars.Attacks
{
    public class Shooter : NetworkBehaviour
    {
        [SerializeField] GameObject bulletPrefab = null;
        [SerializeField] Transform muzzle = null;
        [SerializeField] float bulletSpeed = 5f; //TODO refactor to weapon

        //TODO BulletPooler
        //Needs to create a list of weapons bullets based on type
        [Command]
        public void CmdShoot()
        {
            Rigidbody bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation).GetComponent<Rigidbody>();
            NetworkServer.Spawn(bullet.gameObject);
            bullet.velocity = bullet.transform.forward * bulletSpeed;
        }
    }
}