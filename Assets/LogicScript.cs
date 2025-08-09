using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class LogicScript : MonoBehaviour
{
    public int playerScore;
    public Text scoreText; // Reference to the UI Text component to display the score
    public GameObject gameOverPanel; // Reference to the Game Over panel

    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        playerScore = playerScore + scoreToAdd;
        scoreText.text = playerScore.ToString();
    }
    public void restartGame()
    {
        Time.timeScale = 1; // Resets the time scale to normal speed

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads the current scene to restart the game
    }   

    public void gameOver()
    {
        gameOverPanel.SetActive(true); // Activates the Game Over panel
        Time.timeScale = 0; // Pauses the game by setting the time scale to 0
    }
}