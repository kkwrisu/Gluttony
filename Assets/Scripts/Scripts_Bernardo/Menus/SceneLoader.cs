using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menuinicial");
    }

    public void LoadGameplay()
    {
        SceneManager.LoadScene("Fase_1");
    }
}