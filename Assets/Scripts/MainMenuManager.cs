using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void GoToStage()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void GoToShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void GoToTower()
    {
        SceneManager.LoadScene("Tower");
    }

    public void Return()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}