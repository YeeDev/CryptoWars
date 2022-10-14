using UnityEngine;
using CryptoWars.UI;
using Mirror;
using CryptoWars.Resources;

namespace CryptoWars.Currency
{
    public class CryptoFile : NetworkBehaviour
    {
        [SerializeField] int maxHealth = 5;
        [SerializeField] int value = 5;
        [SerializeField] float heightChange = 0.5f;
        [SerializeField] Color initialColor = Color.blue;
        [SerializeField] Color finalColor = Color.red;
        [SerializeField] Collider col = null;

        float moveDirectionThreshold;
        UIUpdater uIUpdater;
        Material material;

        [SyncVar] int health;
        [SyncVar(hook = nameof(OnChangeMaterialColor))] Color damageColor = Color.black;

        private void OnChangeMaterialColor(Color oldColor, Color newColor) => material.color = newColor;

        public int Health { get => health; }

        private void Awake()
        {
            moveDirectionThreshold = GameObject.FindGameObjectWithTag("Terrain").transform.position.y;
            material = GetComponent<MeshRenderer>().material;
            uIUpdater = FindObjectOfType<UIUpdater>();

            health = maxHealth;
        }

        [Server]
        public void ReduceHealth(int damage, GameObject playerID)
        {
            health -= damage;
            Vector3 loweredPosition = transform.position;
            loweredPosition.y += transform.position.y < moveDirectionThreshold ? -heightChange : heightChange;
            transform.position = loweredPosition;
            damageColor = Color.Lerp(finalColor, initialColor, health / maxHealth);

            if (health <= 0)
            {
                RpcCallUpdateUI(playerID);
                Color transparent = Color.white;
                transparent.a = 0;
                damageColor = transparent;
                col.enabled = false;
            }
        }

        [ClientRpc]
        private void RpcCallUpdateUI(GameObject playerID) => playerID.GetComponent<Stats>().AddCurrency(value, playerID);
    }
}