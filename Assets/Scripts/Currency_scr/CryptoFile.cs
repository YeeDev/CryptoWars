using UnityEngine;
using CryptoWars.UI;

namespace CryptoWars.Currency
{
    public class CryptoFile : MonoBehaviour
    {
        [SerializeField] int currencyValue = 5;
        [SerializeField] int health = 5;
        [SerializeField] float heightChange = 0.5f;

        float moveDirectionThreshold;
        UIUpdater uIUpdater;

        private void Awake()
        {
            uIUpdater = FindObjectOfType<UIUpdater>();
            moveDirectionThreshold = GameObject.FindGameObjectWithTag("Terrain").transform.position.y;
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
}