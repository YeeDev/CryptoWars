using Mirror;
using System.Collections;
using UnityEngine;
using CryptoWars.CustomPhysics;
using CryptoWars.UI;
using CryptoWars.Resources;

namespace CryptoWars.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : NetworkBehaviour
    {
        [SerializeField] float moveSpeed = 0.2f;
        [SerializeField] float jumpForce = 50f;
        [SerializeField] float rotateVSpeed = 1f;
        [SerializeField] float rotateHSpeed = 1f;
        [SerializeField] Transform cannon = null;
        
        bool isHovering;
        float vRotation;
        float hRotation;
        Vector3 spawnPosition;
        Stats stats;
        PhysicsApplier physics;

        //Used in Controller
        public bool IsHovering { get => isHovering; }

        public PhysicsApplier SetPhysicsApplier { set => physics = value; }

        public override void OnStartLocalPlayer()
        {
            stats = GetComponent<Stats>();
            spawnPosition = transform.position;
            InitializeRotation();
        }

        //Also Called in Controller
        public void InitializeRotation()
        {
            Vector3 lookPosition = GameObject.FindGameObjectWithTag("Terrain").transform.position - transform.position;
            vRotation = Quaternion.LookRotation(lookPosition, Vector3.up).eulerAngles.y;
        }

        //Called in Controller
        public void ResetSpeed() => physics.RB.velocity = Vector3.zero;

        //Called in Controller
        public void Jump(bool haltJump = false)
        {
            if (haltJump && (
                (physics.RB.velocity.y > 0 && physics.GravityDirection < 0) ||
                (physics.RB.velocity.y < 0 && physics.GravityDirection > 0)))
            { return; }
             
            Vector3 jumpSpeed = physics.RB.velocity;
            jumpSpeed.y = haltJump ? jumpSpeed.y * 0.5f : (jumpSpeed.y + jumpForce) * physics.GravityDirection;
            physics.RB.velocity = jumpSpeed;
        }

        //Called in Controller
        public void Move(float xAxis, float yAxis)
        {
            Vector3 step = transform.forward * moveSpeed * xAxis;
            step += transform.right * moveSpeed * yAxis * physics.GravityDirection;

            physics.RB.MovePosition(transform.position + step);
        }

        //Called in Controller
        public void Rotate(float vAxis, float hAxis)
        {
            vRotation += vAxis * rotateVSpeed * physics.GravityDirection;
            transform.eulerAngles = new Vector3(0, vRotation, 0);

            hRotation += hAxis * rotateHSpeed * physics.GravityDirection;
            cannon.eulerAngles = new Vector3(hRotation, 0, 0) + transform.eulerAngles;
        }

        //Called in Controller
        public IEnumerator Hover()
        {
            if (stats.CurrentFuel <= Mathf.Epsilon) { yield break; }

            isHovering = true;
            physics.RB.constraints = RigidbodyConstraints.FreezeRotation ^ RigidbodyConstraints.FreezePositionY;

            while (stats.CurrentFuel > Mathf.Epsilon && isHovering)
            {
                stats.ModifyFuelStat(-Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }

            physics.RB.constraints = RigidbodyConstraints.FreezeRotation;
            isHovering = false;
        }

        //Called in Controller
        public void StopHovering() { isHovering = false; }

        //Called in Collisioner
        public void MoveToSpawnPoint() => transform.position = spawnPosition;
    }
}