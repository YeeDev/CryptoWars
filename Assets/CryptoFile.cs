using UnityEngine;

public class CryptoFile : MonoBehaviour
{
    [SerializeField] int currencyValue = 5;
    [SerializeField] int health = 5;
    [SerializeField] float loweredHeightAmount = 0.5f;

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
        loweredPosition.y -= loweredHeightAmount;
        transform.position = loweredPosition;

        if (health <= 0)
        {
            uIUpdater.UpdateCurrency(currencyValue);
            Destroy(gameObject);
        }
    }
}
