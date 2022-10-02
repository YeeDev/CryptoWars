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

    bool grounded;
    Rigidbody rb;

    //TODO Bullet Pooler

    // Start is called before the first frame update
    void Start()
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

    void FixedUpdate()
    {
        grounded = Physics.CheckSphere(groundChecker.position, checkerRadius, groundMask);
        
        Vector3 step = transform.forward * moveSpeed * Input.GetAxis("Vertical");
        rb.MovePosition(transform.position + step);

        Quaternion rotateTo = Quaternion.Euler(transform.rotation.eulerAngles + (transform.up * Input.GetAxis("Horizontal")));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, rotationSpeed);

        Vector3 mouseCursorPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
    -transform.forward.z - -Camera.main.transform.forward.z + mousePrecision));
        cannon.LookAt(mouseCursorPoint, Vector3.up);
        mouseCursorPoint.y = tankHead.position.y;
        tankHead.LookAt(mouseCursorPoint, Vector3.up);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundChecker.position, checkerRadius);
    }
}
