using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    public GameObject painelControles; // Arraste o seu PainelControles aqui

    // Chamado pelo botão
    public void OpenControls()
    {
        painelControles.SetActive(true);
    }

    // Chamado para fechar
    public void CloseControls()
    {
        painelControles.SetActive(false);
    }

    void Update()
    {
        // Se o painel estiver aberto e clicar em qualquer lugar
        if (painelControles.activeSelf && Input.GetMouseButtonDown(0))
        {
            CloseControls();
        }
    }
}