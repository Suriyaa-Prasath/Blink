using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadUIScript : MonoBehaviour
{
    public Canvas InGameCanvas;
    public Canvas DeadCanvas;
    public void Quit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        InGameCanvas.enabled = true;
        DeadCanvas.enabled = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
