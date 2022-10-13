using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoWars.Animations;
using CryptoWars.UI;

namespace CryptoWars.Resources
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] int maxHealth;
        [SerializeField] float maxFuel;

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

            return currentHealth;
        }

        public void RestoreStats()
        {
            currentHealth = maxHealth;
            uIUpdater.UpdateHealthBar(currentHealth, maxHealth);
            ModifyFuelStat(maxFuel);
        }
    }
}