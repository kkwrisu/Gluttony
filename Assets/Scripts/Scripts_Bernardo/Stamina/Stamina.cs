using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public Slider staminaSlider;

    public void UpdateStaminaUI(float percent)
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = percent;
        }
    }
}