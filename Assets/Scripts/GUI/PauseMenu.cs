using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Game Paused
    /// Checks to see if the game is paused or unpaused.
    /// </summary>
    public static bool GamePaused = false;
    /// <summary>
    /// pauseMENU UI
    /// A reference to canvas
    /// </summary>
    public GameObject pausemenuUI;
    /// <summary>
    /// OBJ is a string reference to a scene
    /// </summary>
    public string OBJ;

    // Update is called once per frame
    /// <summary>
    ///  Update
    ///  Asks if you hit escape, if game is not paused is resumed
    ///  If game is paused speed of game set to 0
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }
    /// <summary>
    /// Resume
    /// Resume the Game does not effect the loading of the universe
    /// Game Paused is set to false
    /// Time set normal
    /// </summary>

    public void Resume()
    {
        pausemenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }
    /// <summary>
    /// Pause
    /// Pauses the Game state but not the loading of the universe
    /// Time set to 0 and is stopped
    /// Game Paused is set to true
    /// </summary>
    void Pause()
    {
        pausemenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }
    /// <summary>
    /// Load Menu
    /// Loads a menu and resets the time to normal
    /// </summary>
    public void LoadMenu()
    {
        SceneManager.LoadScene("OBJ");
        Time.timeScale = 1f;
    }
    /// <summary>
    /// Quit Game
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {

        Debug.Log("Quit Game"); // CALLS FOR CONFIRMATION OF QUIT
        Application.Quit(); // DOES NOT WORK IN EDITTOR
    }
}
