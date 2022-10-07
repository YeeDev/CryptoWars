using UnityEngine;

namespace CryptoWars.Core
{
    public class Follower : MonoBehaviour
    {
        Transform player;
        Transform mesh;
        Transform cannon;

        public void SetCamera(Transform player, Transform mesh, Transform cannon)
        {
            this.player = player;
            this.mesh = mesh;
            this.cannon = cannon;
        }

        void LateUpdate()
        {
            if (player == null) { return; }

            transform.position = player.position;

            Vector3 followRotation = player.rotation.eulerAngles;
            followRotation += Vector3.forward * mesh.rotation.eulerAngles.z;
            followRotation += Vector3.right * cannon.rotation.eulerAngles.x;
            transform.rotation = Quaternion.Euler(followRotation);
        }
    }
}