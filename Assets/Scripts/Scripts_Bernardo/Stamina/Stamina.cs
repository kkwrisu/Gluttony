using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public Slider staminaSlider; // Arraste o seu Slider aqui

    public void UpdateStaminaUI(float percent)
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = percent;
        }
    }
}