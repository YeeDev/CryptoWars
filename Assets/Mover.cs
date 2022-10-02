using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.2f;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] Transform tankHead = null;
    [SerializeField] float mousePrecision = 0.1f;
    [SerializeField] Transform cannon = null;
    [SerializeField] GameObject bulletPrefab = null;
    [SerializeField] Transform muzzle = null;
    [SerializeField] float bulletSpeed = 5f;

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
    }

    void FixedUpdate()
    {
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
}
