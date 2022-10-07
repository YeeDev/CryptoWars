using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    [Tooltip("For debuggin purposes")]
    [SerializeField] int currency = 0;  //TODO refactor this to another script
    [SerializeField] Text currencyText = null;
    [SerializeField] RectTransform fuelBar = null;

    float fuelBarMaxXSize;

    private void Awake()
    {
        fuelBarMaxXSize = fuelBar.sizeDelta.x;
        UpdateCurrency(0);
    }

    public void UpdateCurrency(int currencyToAdd)
    {
        currency += currencyToAdd;
        currencyText.text = $"Currency: {currency}";
    }

    public void UpdateFuelBar(float fuelRemaining, float maxFuel)
    {
        Vector2 fuelBarSize = fuelBar.sizeDelta;
        fuelBarSize.x = (fuelRemaining * fuelBarMaxXSize) / maxFuel;
        fuelBar.sizeDelta = fuelBarSize;
    }
}
