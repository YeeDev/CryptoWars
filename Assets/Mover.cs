using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.2f;
    [SerializeField] float rotationSpeed = 1f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 step = transform.forward * moveSpeed * Input.GetAxis("Vertical");
        rb.MovePosition(transform.position + step);

        Quaternion rotateTo = Quaternion.Euler(transform.rotation.eulerAngles + (transform.up * Input.GetAxis("Horizontal")));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, rotationSpeed); 
    }
}
