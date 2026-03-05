using UnityEngine;
using UnityEngine.UI;

public class CarrotHealthUI : MonoBehaviour
{
    public Image carrotDisplay;
    public Sprite[] carrotStages;
    
    public void UpdateHealthUI(int currentHealth)
    {
        int index = Mathf.Clamp(currentHealth, 0, carrotStages.Length - 1);
        
        carrotDisplay.sprite = carrotStages[index];
    }
}
