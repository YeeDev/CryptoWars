using UnityEngine;
using CryptoWars.UI;
using Mirror;
using CryptoWars.Resources;

namespace CryptoWars.Currency
{
    public class CryptoFile : NetworkBehaviour
    {
        [SerializeField] int currencyValue = 5;
        [SerializeField] int maxHealth = 5;
        [SerializeField] float heightChange = 0.5f;
        [SerializeField] Color initialColor = Color.blue;
        [SerializeField] Color finalColor = Color.red;

        float health;
        float moveDirectionThreshold;
        Material material;

        [SyncVar(hook = nameof(SetColor))] Color damageColor = Color.black;

        private void SetColor(Color oldColor, Color newColor) => material.color = newColor;

        private void Awake()
        {
            moveDirectionThreshold = GameObject.FindGameObjectWithTag("Terrain").transform.position.y;
            material = GetComponent<MeshRenderer>().material;

            health = maxHealth;
        }

        public void ReduceHealth(int damage)
        {
            health -= damage;

            Vector3 loweredPosition = transform.position;
            loweredPosition.y += transform.position.y < moveDirectionThreshold ? -heightChange : heightChange;
            transform.position = loweredPosition;
            damageColor = Color.Lerp(finalColor, initialColor, health / maxHealth);

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}