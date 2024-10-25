using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEnd: MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
   
    
    [Header("Main Menu Buttons")]
    public Button startButton;
   
    public Button quitButton;

    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        

        //Hook events
        startButton.onClick.AddListener(RetryGame);
        
        quitButton.onClick.AddListener(QuitGame);

       
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RetryGame()
    {
        
        SceneManager.LoadScene("SampleScene");
    }

    
}
