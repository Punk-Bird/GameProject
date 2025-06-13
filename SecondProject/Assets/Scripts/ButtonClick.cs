using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
