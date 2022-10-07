using System.Collections;
using UnityEngine;
using CryptoWars.CustomPhysics;
using CryptoWars.UI;

namespace CryptoWars.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 0.2f;
        [SerializeField] float jumpForce = 50f;
        [SerializeField] float rotateVSpeed = 1f;
        [SerializeField] float rotateHSpeed = 1f;
        [Tooltip("1 fuel unit = 1 second.")]
        [SerializeField] float maxFuel = 5f; //TODO Grab it from hardware piece Fuel
        [SerializeField] Transform cannon = null;
        
        bool isHovering;
        float vRotation;
        float hRotation;
        float currentFuel;
        Transform cameraPivot;
        PhysicsApplier physics;
        UIUpdater uIUpdater;

        //Used in Controller
        public bool IsHovering { get => isHovering; }

        public PhysicsApplier SetPhysicsApplier { set => physics = value; }

        private void Awake()
        {
            cameraPivot = Camera.main.transform.parent.transform;
            uIUpdater = FindObjectOfType<UIUpdater>();

            currentFuel = maxFuel;
        }

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
            step += transform.right * moveSpeed * yAxis;

            physics.RB.MovePosition(transform.position + step);
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

        //Called in Controller
        public IEnumerator Hover()
        {
            if (currentFuel <= Mathf.Epsilon) { yield break; }

            isHovering = true;
            physics.RB.constraints = RigidbodyConstraints.FreezePositionY ^ RigidbodyConstraints.FreezeRotation;

            while (currentFuel > Mathf.Epsilon && isHovering)
            {
                currentFuel -= Time.deltaTime;
                uIUpdater.UpdateFuelBar(currentFuel, maxFuel);

                yield return new WaitForEndOfFrame();
            }

            physics.RB.constraints = RigidbodyConstraints.FreezeRotation;
            isHovering = false;
        }

        //Called in Controller
        public void StopHovering() { isHovering = false; }
        //Called in Controller
        public void ResetHoveringTimer()
        {
            if (isHovering) { return; }

            currentFuel = Mathf.Clamp(currentFuel + Time.deltaTime, 0, maxFuel);
            uIUpdater.UpdateFuelBar(currentFuel, maxFuel);
        }
    }
}