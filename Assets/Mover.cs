using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Movement Related")] //TODO Refactor this mess
    [SerializeField] float moveSpeed = 0.2f;
    [SerializeField] float rotationSpeed = 1f;
    [Header("Shoot Related")]
    [SerializeField] Transform tankHead = null;
    [SerializeField] float mousePrecision = 0.1f;
    [SerializeField] Transform cannon = null;
    [SerializeField] GameObject bulletPrefab = null;
    [SerializeField] Transform muzzle = null;
    [SerializeField] float bulletSpeed = 5f;
    [Header("Jump Related")]
    [SerializeField] float jumpForce = 50f;
    [SerializeField] Transform groundChecker = null;
    [SerializeField] float checkerRadius = 0.1f;
    [SerializeField] LayerMask groundMask = 0;
    [Header("CameraRelated")]
    [SerializeField] Transform cameraPivot = null;
    [SerializeField] float ySpeed = 1f;
    [SerializeField] float xSpeed = 1f;
    [SerializeField] float clampPosition = 35f;


    bool grounded;
    Rigidbody rb;

    //TODO Bullet Pooler

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Rigidbody bullet = Instantiate(bulletPrefab, muzzle.position, cannon.rotation).GetComponent<Rigidbody>();
            bullet.velocity = cannon.forward * bulletSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Vector3 jumpVector = rb.velocity;
            jumpVector.y += jumpForce;
            rb.velocity = jumpVector;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            Vector3 haltJumpVector = rb.velocity;
            haltJumpVector.y *= 0.5f;
            rb.velocity = haltJumpVector;
        }
    }
    float yRotation;
    float xRotation;
    //TODO Rotate Camera instead of everything else
    void FixedUpdate()
    {
        grounded = Physics.CheckSphere(groundChecker.position, checkerRadius, groundMask);

        Quaternion rotateTo = Quaternion.Euler(transform.rotation.eulerAngles + (transform.up * Input.GetAxis("Horizontal")));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, rotationSpeed);

        Vector3 step = transform.forward * moveSpeed * Input.GetAxis("Vertical");
        step += transform.right * moveSpeed * Input.GetAxis("Horizontal");

        rb.MovePosition(transform.position + step);

        yRotation += Input.GetAxis("Mouse X") * ySpeed;
        transform.eulerAngles = new Vector3(0, yRotation, 0);

        Vector3 mouseRealWorldPosition = Input.mousePosition;
        mouseRealWorldPosition.z = 10;

        Vector2 worldScreen = Camera.main.transform.position - Camera.main.ScreenToWorldPoint(mouseRealWorldPosition);
        xRotation += worldScreen.y * xSpeed;
        Debug.Log(xRotation);
        cannon.eulerAngles = (new Vector3(-xRotation, 0, 0) + transform.eulerAngles) - Vector3.right * cameraPivot.position.y;
        cameraPivot.eulerAngles = cannon.eulerAngles;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundChecker.position, checkerRadius);
    }
}
