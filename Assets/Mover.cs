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
        [SerializeField] Transform cannon = null;

        [Header("CameraRelated")]
        [SerializeField] Transform cameraPivot = null;

        float vRotation;
        float hRotation;
        Rigidbody rb;

        private void Awake() { rb = GetComponent<Rigidbody>(); }

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