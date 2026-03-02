using UnityEngine;
using UnityEngine.UI;

public class CarrotHealthUI : MonoBehaviour
{
    public Image carrotDisplay; // Arraste a sua UI Image aqui
    public Sprite[] carrotStages; // Arraste seus sprites aqui no Inspector
    
    // Supondo que você tenha 4 estados (ex: 3, 2, 1, 0 vidas)
    public void UpdateHealthUI(int currentHealth)
    {
        // Garante que o índice não saia do limite da lista
        int index = Mathf.Clamp(currentHealth, 0, carrotStages.Length - 1);
        
        // Troca o sprite da UI
        carrotDisplay.sprite = carrotStages[index];
    }
}
