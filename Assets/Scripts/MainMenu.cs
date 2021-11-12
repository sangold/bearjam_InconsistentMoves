using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {

        Screen.SetResolution(Mathf.FloorToInt(Screen.height * (9f / 16f)), Screen.height, FullScreenMode.Windowed);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/Game");
    }
}
