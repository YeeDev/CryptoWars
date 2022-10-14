using UnityEngine;

public class BulletDummy : MonoBehaviour
{
    [SerializeField] float bulletspeed = 40f;
    [SerializeField] Rigidbody rb = null;
    [SerializeField] GameObject explosionParticles = null;

    Collider col;

    private void Start()
    {
        rb.velocity = transform.forward * bulletspeed;
        col = GetComponent<Collider>();
        Invoke("StartCollider", 0.1f);
    }

    private void StartCollider() => col.enabled = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) { return; }

        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Debug.Log("Hello there server!");
        Destroy(gameObject);
    }
}
