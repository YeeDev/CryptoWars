using UnityEngine;
using UnityEngine.UI;

namespace CryptoWars.UI
{
    public class UIUpdater : MonoBehaviour
    {
        [SerializeField] Text currencyText = null;
        [SerializeField] RectTransform healthBar = null;
        [SerializeField] RectTransform fuelBar = null;

        float healthBarMaxXSize;
        float fuelBarMaxXSize;

        private void Awake()
        {
            fuelBarMaxXSize = fuelBar.sizeDelta.x;
            healthBarMaxXSize = healthBar.sizeDelta.x;
            UpdateCurrency(0);
        }

        public void UpdateCurrency(int currencyToAdd) => currencyText.text = $"Currency: {currencyToAdd}";

        public void UpdateFuelBar(float fuelRemaining, float maxFuel)
        {
            Vector2 fuelBarSize = fuelBar.sizeDelta;
            fuelBarSize.x = (fuelRemaining * fuelBarMaxXSize) / maxFuel;
            fuelBar.sizeDelta = fuelBarSize;
        }

        public void UpdateHealthBar(float healthRemaining, float maxHealth)
        {
            Vector2 healthBarSize = healthBar.sizeDelta;
            healthBarSize.x = (healthRemaining * healthBarMaxXSize) / maxHealth;
            healthBar.sizeDelta = healthBarSize;
        }
    }
}