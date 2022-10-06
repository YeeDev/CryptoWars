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
        [SerializeField] float hoveringMaxTime = 5f; //TODO Grab it from hardware piece Fuel
        [SerializeField] Transform cannon = null;
        [SerializeField] RectTransform fuelBar = null;
        
        bool isHovering;
        float vRotation;
        float hRotation;
        float hoveringTime;
        float fuelBarMaxXSize;
        Transform cameraPivot;
        Rigidbody rb;

        //Used in Controller
        public bool IsHovering { get => isHovering; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cameraPivot = Camera.main.transform.parent.transform;

            hoveringTime = hoveringMaxTime;
            fuelBarMaxXSize = fuelBar.sizeDelta.x;
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
            if (hoveringTime <= Mathf.Epsilon) { yield break; }

            isHovering = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY ^ RigidbodyConstraints.FreezeRotation;

            while (hoveringTime > Mathf.Epsilon && isHovering)
            {
                hoveringTime -= Time.deltaTime;
                Vector2 fuelBarSize = fuelBar.sizeDelta;
                fuelBarSize.x = (hoveringTime * fuelBarMaxXSize) / hoveringMaxTime;
                fuelBar.sizeDelta = fuelBarSize;

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
            hoveringTime = Mathf.Clamp(hoveringTime + Time.deltaTime, 0, hoveringMaxTime);
            Vector2 fuelBarSize = fuelBar.sizeDelta; //TODO Refacor this
            fuelBarSize.x = (hoveringTime * fuelBarMaxXSize) / hoveringMaxTime;
            fuelBar.sizeDelta = fuelBarSize;
        } 
    }
}