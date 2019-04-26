using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Cameron Garchow's Pause Menu
/// Currently this pause menu pauses the game when escape is hit. It is bound inside of its own canvas and object to prevent issues
/// There is currently a bug that has to deal with the GROW functionality in unity editor. Which prevents us from building the scene.
/// The only way to fix this is to delete the editor functionality from all objects that retain this functionality
/// Once it is deleted we can then load the scenes and build them.
/// This menu system is currently in working order, right now if exit to main menu is clicked it won't do anything but send a debug command.
/// Once we have deleted the old functionality it will work.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Game Paused
    /// Checks to see if the game is paused or unpaused.
    /// </summary>
    public static bool gamepaused = false;
    /// <summary>
    /// pauseMENU UI
    /// A reference to canvas
    /// </summary>
    public GameObject pausemenuUI;
    /// <summary>
    /// OBJ is a string reference to a scene
    /// </summary>
    public string OBJ;

    ReplacementShader replace = new ReplacementShader();

    void Start()
    {
        replace = GetComponent<ReplacementShader>();
    }

    // Update is called once per frame
    /// <summary>
    ///  Update
    ///  Asks if you hit escape, if game is not paused is resumed
    ///  If game is paused speed of game set to 0
    /// </summary>
    void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            if (gamepaused)
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
        gamepaused = false;
        AudioListener.volume = 1.0f;
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
        gamepaused = true;
        AudioListener.volume = .25f; //set to lower volume

        replace.Populate();
    }
    /// <summary>
    /// Load Menu
    /// Loads a menu and resets the time to normal
    /// </summary>
    public void LoadMenu()
    {
        Debug.Log("Scene Loaded");
       //SceneManager.LoadScene("OBJ");
        Resume();
    }


    /// <summary>
    /// Load Menu
    /// Loads a menu and resets the time to normal
    /// </summary>
    public void Options()
    {
        Debug.Log("Options Loaded");
        //SceneManager.LoadScene("OBJ");
    }
    /// <summary>
    /// Quit Game
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {

        Debug.Log("Quit Game"); // CALLS FOR CONFIRMATION OF QUIT
        Application.Quit(); // DOES NOT WORK IN EDITTOR ONLY ON BUILD
    }
}
