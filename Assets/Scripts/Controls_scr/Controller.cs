using Mirror;
using UnityEngine;
using CryptoWars.Movement;
using CryptoWars.Attacks;
using CryptoWars.CustomPhysics;
using CryptoWars.Core;
using CryptoWars.Animations;
using CryptoWars.Resources;

namespace CryptoWars.Controls
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Shooter))]
    [RequireComponent(typeof(Animater))]
    [RequireComponent(typeof(PhysicsApplier))]
    [RequireComponent(typeof(Collisioner))]
    public class Controller : NetworkBehaviour
    {
        [SerializeField] Transform groundChecker = null;
        [SerializeField] float checkerRadius = 0.1f;
        [SerializeField] LayerMask groundMask = 0;
        [SerializeField] Transform cannon = null;

        bool grounded;
        Stats stats;
        Mover mover;
        Shooter shooter;
        Animater animater;
        PhysicsApplier physicsApplier;
        Collisioner collisioner;
        CurrencyDataHolder dataHolder;

        public override void OnStartLocalPlayer()
        {
            transform.LookAt(GameObject.FindGameObjectWithTag("Terrain").transform);

            mover = GetComponent<Mover>();
            shooter = GetComponent<Shooter>();
            physicsApplier = GetComponent<PhysicsApplier>();
            animater = GetComponent<Animater>();
            collisioner = GetComponent<Collisioner>();
            stats = GetComponent<Stats>();

            transform.GetChild(0).name = transform.name;

            mover.SetPhysicsApplier = physicsApplier;
            Camera.main.GetComponentInParent<Follower>().SetCamera(transform, transform.GetChild(0), cannon);

            Cursor.lockState = CursorLockMode.Locked;

            dataHolder = GameObject.FindGameObjectWithTag("Currency Manager").GetComponent<CurrencyDataHolder>();
            dataHolder.RegisterPlayer();
        }

        private void Update()
        {
            if (!isLocalPlayer) { return; }

            ReadMoveInput();
            ReadJumpInput();
            ReadShootInput();
            physicsApplier.CheckIfInvertGravity(collisioner.IsInsideBG);
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer) { return; }

            grounded = Physics.CheckSphere(groundChecker.position, checkerRadius, groundMask);
            animater.SetGroundedParam(grounded);

            if (grounded) { stats.ModifyFuelStat(Time.deltaTime); }
            if (grounded && mover.IsHovering) { mover.StopHovering(); }

            physicsApplier.ApplyGravity();
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

        private void ReadShootInput() { if (Input.GetMouseButtonDown(0)) { shooter.CmdShoot(gameObject); } }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(groundChecker.position, checkerRadius);
        }
    }
}