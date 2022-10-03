using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoWars.Movement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 0.2f;
        [SerializeField] float jumpForce = 50f;

        [Header("Shoot Related")]
        [SerializeField] Transform cannon = null;
        [SerializeField] GameObject bulletPrefab = null;
        [SerializeField] Transform muzzle = null;
        [SerializeField] float bulletSpeed = 5f;

        [Header("CameraRelated")]
        [SerializeField] Transform cameraPivot = null;
        [SerializeField] float rotateVSpeed = 1f;
        [SerializeField] float rotateHSpeed = 1f;

        float vRotation;
        float hRotation;
        Rigidbody rb;

        //TODO Bullet Pooler

        private void Awake() { rb = GetComponent<Rigidbody>(); }

        //private void Update()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Rigidbody bullet = Instantiate(bulletPrefab, muzzle.position, cannon.rotation).GetComponent<Rigidbody>();
        //        bullet.velocity = cannon.forward * bulletSpeed;
        //    }
        //}

        //Called in Controller
        public void Jump(bool haltJump)
        {
            if (haltJump && rb.velocity.y > 0) { return; }

            Vector3 jumpSpeed = rb.velocity;
            jumpSpeed.y = haltJump ? jumpSpeed.y * 0.5f : jumpSpeed.y + jumpForce;
            rb.velocity = jumpSpeed;
        }

        //Called in Controller
        public void Move(float xAxis, float yAxis)
        {
            Vector3 step = transform.forward * moveSpeed * xAxis;
            step += transform.right * moveSpeed * yAxis;

            rb.MovePosition(transform.position + step);
        }

        //Called in Controller
        public void Rotate(float vAxis, float hAxis)
        {
            vRotation += vAxis * rotateVSpeed;
            transform.eulerAngles = new Vector3(0, vRotation, 0);

            hRotation += hAxis * rotateHSpeed;
            cannon.eulerAngles = (new Vector3(hRotation, 0, 0) + transform.eulerAngles) - Vector3.right * cameraPivot.position.y;
            cameraPivot.eulerAngles = cannon.eulerAngles; //TODO in his own script
        }
    }
}