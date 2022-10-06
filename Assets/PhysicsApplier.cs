using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoWars.CustomPhysics
{
    public class PhysicsApplier : MonoBehaviour
    {
        [SerializeField] float gravityForce = -20;

        float gravityThreshold;
        Rigidbody rb;

        public float GravityDirection { get => Mathf.Sign(gravityForce) * -1; }
        public Rigidbody RB { get => rb; }

        private void Awake()
        {
            gravityThreshold = GameObject.FindGameObjectWithTag("Terrain").transform.position.y;
            rb = GetComponent<Rigidbody>();
        }

        public void ApplyGravity() { rb.AddForce(Vector3.up * gravityForce); }

        public void CheckIfInvertGravity()
        {
            if (transform.position.y > gravityThreshold && gravityForce < 0) { gravityForce = -gravityForce; }
            else if (transform.position.y < gravityThreshold && gravityForce > 0) { gravityForce *= -1; }
        }
    }
}