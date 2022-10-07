using UnityEngine;

namespace CryptoWars.Core
{
    public class Follower : MonoBehaviour
    {
        Transform player;
        Transform model;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            model = player.GetChild(0);
        }

        void LateUpdate()
        {
            transform.position = player.position;

            Vector3 followRotation = player.rotation.eulerAngles;
            followRotation += Vector3.forward * model.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(followRotation);
        }
    }
}