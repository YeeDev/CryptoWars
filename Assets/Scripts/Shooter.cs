using UnityEngine;

namespace CryptoWars.Attacks
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] GameObject bulletPrefab = null;
        [SerializeField] Transform muzzle = null;
        [SerializeField] float bulletSpeed = 5f; //TODO refactor to weapon

        //TODO BulletPooler
        //Needs to create a list of weapons bullets based on type
        public void Shoot()
        {
            Rigidbody bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation).GetComponent<Rigidbody>();
            bullet.velocity = bullet.transform.forward * bulletSpeed;
        }
    }
}