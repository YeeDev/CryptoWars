using System.Collections;
using UnityEngine;

namespace CryptoWars.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 0.2f;
        [SerializeField] float jumpForce = 50f;
        [SerializeField] float rotateVSpeed = 1f;
        [SerializeField] float rotateHSpeed = 1f;
        [Tooltip("1 fuel unit = 1 second.")][SerializeField] float maxFuel = 5f; //TODO Grab it from hardware piece Fuel
        [SerializeField] Transform cannon = null;
        
        bool isHovering;
        float vRotation;
        float hRotation;
        float currentFuel;
        Rigidbody rb;
        Transform cameraPivot;
        UIUpdater uIUpdater;

        //Used in Controller
        public Rigidbody GetRigidbody { get => rb; }
        public bool IsHovering { get => isHovering; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cameraPivot = Camera.main.transform.parent.transform;
            uIUpdater = FindObjectOfType<UIUpdater>();

            currentFuel = maxFuel;
        }

        //Called in Controller
        public void Jump(bool haltJump = false)
        {
            if (haltJump && rb.velocity.y < 0) { return; }

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

        //Called in Controller
        public IEnumerator Hover()
        {
            if (currentFuel <= Mathf.Epsilon) { yield break; }

            isHovering = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY ^ RigidbodyConstraints.FreezeRotation;

            while (currentFuel > Mathf.Epsilon && isHovering)
            {
                currentFuel -= Time.deltaTime;
                uIUpdater.UpdateFuelBar(currentFuel, maxFuel);

                yield return new WaitForEndOfFrame();
            }

            rb.constraints = RigidbodyConstraints.FreezeRotation;
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