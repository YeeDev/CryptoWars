using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoWars.Movement;

namespace CryptoWars.CustomPhysics
{
    public class PhysicsApplier : MonoBehaviour
    {
        [SerializeField] float gravityForce = -20;

        Mover mover;

        public Mover SetMover { set => mover = value; }

        public void ApplyGravity()
        {
            mover.GetRigidbody.AddForce(Vector3.up * gravityForce);
        }
    }
}