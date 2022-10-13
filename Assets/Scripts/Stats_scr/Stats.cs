using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoWars.Animations;
using CryptoWars.UI;

namespace CryptoWars.Resources
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] int maxHealth = 0;
        [SerializeField] float maxFuel = 0;
        [SerializeField] int currency = 0;

        float currentFuel;
        int currentHealth;
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

        // Called in CryptoFile
        public void AddCurrency(int amountToAdd)
        {
            currency += amountToAdd;
            uIUpdater.UpdateCurrency(currency);
        }
    }
}