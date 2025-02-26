using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign your Pause Menu Panel in the Inspector
    public Button resumeButton; // Assign Resume Button
    public Button menuButton; // Assign Menu Button

    private bool isPaused = false;

    private void Start()
    {
        // Ensure the pause menu is hidden initially
        pauseMenuUI.SetActive(false);

        // Attach button listeners
        resumeButton.onClick.AddListener(ResumeGame);
        menuButton.onClick.AddListener(TogglePause);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1; // 0 = Pause, 1 = Resume
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1; // Resume game time
    }
}
