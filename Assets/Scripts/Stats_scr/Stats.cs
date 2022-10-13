using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoWars.UI;

namespace CryptoWars.Resources
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] float maxHealth;
        [SerializeField] float maxFuel;

        float currentFuel;
        UIUpdater uIUpdater;

        public float CurrentFuel { get => currentFuel; }
        public float MaxFuel { get => maxFuel; }

        private void Awake()
        {
            uIUpdater = FindObjectOfType<UIUpdater>();

            ModifyFuelStat(maxFuel);
        }

        // Called in Controller, Collisioner, and Mover
        public void ModifyFuelStat(float amount)
        {
            currentFuel = Mathf.Clamp(currentFuel + amount, 0, maxFuel);
            uIUpdater.UpdateFuelBar(currentFuel, maxFuel);
        }
    }
}