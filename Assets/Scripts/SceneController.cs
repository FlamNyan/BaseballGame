using UnityEngine;
using UnityEngine.SceneManagement;
using BaseballGame.Scripts.Managers; // Gives access to your SoundManager

public class SceneController : MonoBehaviour
{
    // Call this from your "Start Game" or "Play" button
    public void LoadGameScene()
    {
        // LoadSceneMode.Single automatically destroys the previous scene's objects 
        // (except our persistent Managers), so we don't need an unLoadScene method!
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        
        // Tell the SoundManager to play the crowd cheer
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameStartCheer();
        }
    }

    // Call this from your "Back" button or when the game ends
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        
        // Ensure the looping menu music is playing
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayMenuMusic();
        }
    }

    // Call this from your "Credits" button
    public void LoadCreditScene()
    {
        SceneManager.LoadScene("CreditScene", LoadSceneMode.Single);
        
        // Credits usually share the main menu vibe, so keep the menu music rolling
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayMenuMusic();
        }
    }

    // Always good to have a Quit button on your Main Menu!
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}