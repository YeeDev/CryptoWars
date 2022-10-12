using Mirror;
using UnityEngine;
using CryptoWars.Movement;
using CryptoWars.Attacks;
using CryptoWars.CustomPhysics;
using CryptoWars.Core;
using CryptoWars.Animations;

namespace CryptoWars.Controls
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Shooter))]
    public class Controller : NetworkBehaviour
    {
        [SerializeField] Transform groundChecker = null;
        [SerializeField] float checkerRadius = 0.1f;
        [SerializeField] LayerMask groundMask = 0;
        [SerializeField] Transform cannon = null;

        bool grounded;
        Vector3 spawnPosition;
        Mover mover;
        Shooter shooter;
        Animater animater;
        PhysicsApplier physicsApplier;

        public override void OnStartLocalPlayer()
        {
            mover = GetComponent<Mover>();
            shooter = GetComponent<Shooter>();
            physicsApplier = GetComponent<PhysicsApplier>();
            animater = GetComponent<Animater>();

            mover.SetPhysicsApplier = physicsApplier;
            Camera.main.GetComponentInParent<Follower>().SetCamera(transform, transform.GetChild(0), cannon);

            Cursor.lockState = CursorLockMode.Locked;

            spawnPosition = transform.position;
        }

        private void Update()
        {
            if (!isLocalPlayer) { return; }

            ReadMoveInput();
            ReadJumpInput();
            ReadShootInput();
            physicsApplier.CheckIfInvertGravity();
        }

        private void ReadMoveInput()
        {
            mover.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            mover.Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            bool isMoving = Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")) > Mathf.Epsilon;
            bool isRotating = Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y")) > Mathf.Epsilon;
            animater.PlayWalkAnimation((isMoving || isRotating) && grounded);
        }

        private void ReadJumpInput()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !mover.IsHovering && !grounded) { StartCoroutine(mover.Hover()); }
            else if (Input.GetKeyUp(KeyCode.Space) && mover.IsHovering) { mover.StopHovering(); }
            else if (Input.GetKeyDown(KeyCode.Space) && grounded) { mover.Jump(); }
            else if (Input.GetKeyUp(KeyCode.Space) && !mover.IsHovering) { mover.Jump(true); }
        }

        private void ReadShootInput() { if (Input.GetMouseButtonDown(0)) { shooter.CmdShoot(); } }

        private void FixedUpdate()
        {
            if (!isLocalPlayer) { return; }

            grounded = Physics.CheckSphere(groundChecker.position, checkerRadius, groundMask);
            animater.SetGroundedParam(grounded);

            if (grounded) { mover.ResetHoveringTimer(); }

            physicsApplier.ApplyGravity();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(groundChecker.position, checkerRadius);
        }
    }
}