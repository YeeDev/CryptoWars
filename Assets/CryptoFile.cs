using UnityEngine;

public class CryptoFile : MonoBehaviour
{
    [SerializeField] int currencyValue = 5;
    [SerializeField] int health = 5;
    [SerializeField] float heightChange = 0.5f;
    [SerializeField] float moveDirectionThreshold = 10;

    UIUpdater uIUpdater;

    private void Awake()
    {
        uIUpdater = FindObjectOfType<UIUpdater>();
    }

    //TODO Refactor this, POSSNewName TakeDamage
    public void ReduceHealth(int damage)
    {
        health -= damage;

        Vector3 loweredPosition = transform.position;
        loweredPosition.y += transform.position.y < moveDirectionThreshold ? -heightChange : heightChange;
        transform.position = loweredPosition;

        if (health <= 0)
        {
            uIUpdater.UpdateCurrency(currencyValue);
            Destroy(gameObject);
        }
    }
}
