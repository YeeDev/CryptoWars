using Mirror;
using UnityEngine;
using CryptoWars.Movement;
using CryptoWars.Attacks;
using CryptoWars.CustomPhysics;
using CryptoWars.Core;

namespace CryptoWars.Controls
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Shooter))]
    public class Controller : NetworkBehaviour
    {
        [SerializeField] Transform groundChecker = null;
        [SerializeField] float checkerRadius = 0.1f;
        [SerializeField] LayerMask groundMask = 0;
        [SerializeField] Transform cannon;

        bool grounded;
        Mover mover;
        Shooter shooter;
        PhysicsApplier physicsApplier;

        public override void OnStartLocalPlayer()
        {
            mover = GetComponent<Mover>();
            shooter = GetComponent<Shooter>();
            physicsApplier = GetComponent<PhysicsApplier>();

            mover.SetPhysicsApplier = physicsApplier;
            Camera.main.GetComponentInParent<Follower>().SetCamera(transform, transform.GetChild(0), cannon);

            Cursor.lockState = CursorLockMode.Locked;
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