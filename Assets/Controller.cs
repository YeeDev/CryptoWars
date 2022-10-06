using UnityEngine;
using CryptoWars.Movement;
using CryptoWars.Attacks;


namespace CryptoWars.Controls
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Shooter))]
    public class Controller : MonoBehaviour
    {
        [SerializeField] Transform groundChecker = null;
        [SerializeField] float checkerRadius = 0.1f;
        [SerializeField] LayerMask groundMask = 0;

        bool grounded;
        Mover mover;
        Shooter shooter;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            shooter = GetComponent<Shooter>();

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            ReadMoveInput();
            ReadJumpInput();
            ReadShootInput();
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

        private void ReadShootInput() { if (Input.GetMouseButtonDown(0)) { shooter.Shoot(); } }

        private void FixedUpdate()
        {
            grounded = Physics.CheckSphere(groundChecker.position, checkerRadius, groundMask);

            if (grounded) { mover.ResetHoveringTimer(); }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(groundChecker.position, checkerRadius);
        }
    }
}