using Mirror;
using UnityEngine;
using CryptoWars.AttackTypes;
using CryptoWars.Resources;

namespace CryptoWars.Attacks
{
    public class Shooter : NetworkBehaviour
    {
        [SerializeField] GameObject bulletPrefab = null;
        [SerializeField] GameObject bulletDummyPrefab = null;
        [SerializeField] Transform muzzle = null;
        [SerializeField] AudioSource audioSource = null;

        [Command]
        public void CmdShoot(bool comesFromServer)
        {
            GameObject typeOfBullet = comesFromServer ? bulletPrefab : bulletDummyPrefab;
            CreateBullet(typeOfBullet);
            RpcCreateBullet();
            audioSource.Play();
        }

        private void CreateBullet(GameObject typeOfPrefab)
        {
            GameObject bullet = Instantiate(typeOfPrefab, muzzle.position, muzzle.rotation);
            if (typeOfPrefab != bulletDummyPrefab) { bullet.GetComponent<Bullet>().SetTransform = GetComponent<Stats>(); }
        }

        [ClientRpc]
        private void RpcCreateBullet()
        {
            if (!isServer) { CreateBullet(bulletPrefab); }
        }
    }
}