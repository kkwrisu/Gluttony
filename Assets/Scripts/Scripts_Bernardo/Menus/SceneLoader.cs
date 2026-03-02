using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Chame esta função para ir para o Menu (ex: ao clicar num botão ou morrer)
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menuinicial"); // Substitua pelo NOME EXATO da sua cena de menu
    }

    // Chame esta função para reiniciar o jogo (Gameplay)
    public void LoadGameplay()
    {
        SceneManager.LoadScene("main"); // Substitua pelo NOME EXATO da sua cena de jogo
    }
}