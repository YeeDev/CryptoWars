using Mirror;
using UnityEngine;
using CryptoWars.Animations;
using CryptoWars.UI;

namespace CryptoWars.Resources
{
    public class Stats : NetworkBehaviour
    {
        [SerializeField] int maxHealth = 0;
        [SerializeField] float maxFuel = 0;

        int currency;
        int currentHealth;
        float currentFuel;
        Animater anm;
        UIUpdater uIUpdater;

        public float CurrentFuel { get => currentFuel; }
        public float MaxFuel { get => maxFuel; }

        private void Awake()
        {
            anm = GetComponent<Animater>();
            uIUpdater = FindObjectOfType<UIUpdater>();

            currentHealth = maxHealth;
            ModifyFuelStat(maxFuel);
        }

        // Called in Controller, Collisioner, and Mover
        public void ModifyFuelStat(float amount)
        {
            currentFuel = Mathf.Clamp(currentFuel + amount, 0, maxFuel);
            uIUpdater.UpdateFuelBar(currentFuel, maxFuel);
        }

        // Called in Collisioner
        public int TakeDamage()
        {
            currentHealth--;
            uIUpdater.UpdateHealthBar(currentHealth, maxHealth);
            anm.PlayTakeDamageAnimation();

            return currentHealth;
        }

        // Called in Collisioner
        public void RestoreStats()
        {
            currentHealth = maxHealth;
            uIUpdater.UpdateHealthBar(currentHealth, maxHealth);
            ModifyFuelStat(maxFuel);
        }

        // Called in Bullet
        public void AddCurrency(int amountToAdd, GameObject playerID)
        {
            if (playerID.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                currency += amountToAdd;
                uIUpdater.UpdateCurrency(currency);
            }
        }
    }
}