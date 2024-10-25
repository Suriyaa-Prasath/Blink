using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadUIScript : MonoBehaviour
{
    public Canvas InGameCanvas;
    public Canvas DeadCanvas;
    [Header("UI Pages")]
    public GameObject DeadUI;

    [Header("Main Menu Buttons")]
    public Button retryButton;
    public Button quitButton;

    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        DeadUIM();

        //Hook events
        retryButton.onClick.AddListener(RetryGame);
        quitButton.onClick.AddListener(QuitGame);

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(DeadUIM);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RetryGame()
    {
        InGameCanvas.enabled = true;
        DeadCanvas.enabled = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name); 
    }

    public void HideAll()
    {
        DeadUI.SetActive(false);
    }

    public void DeadUIM()
    {
        DeadUI.SetActive(true);
    }
}