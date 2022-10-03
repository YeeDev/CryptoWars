using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoWars.Movement;
using System;

namespace CryptoWars.Controls
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] Transform groundChecker = null;
        [SerializeField] float checkerRadius = 0.1f;
        [SerializeField] LayerMask groundMask = 0;

        bool grounded; //TODO use this for hovering
        Mover mover;

        private void Awake()
        {
            mover = GetComponent<Mover>();
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
            if (Input.GetKeyDown(KeyCode.Space)) { mover.Jump(false); }
            else if (Input.GetKeyUp(KeyCode.Space)) { mover.Jump(true); }
        }

        private void ReadShootInput()
        {
            Debug.Log("TODO create shooter script");
        }

        private void FixedUpdate()
        {
            grounded = Physics.CheckSphere(groundChecker.position, checkerRadius, groundMask);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(groundChecker.position, checkerRadius);
        }
    }
}