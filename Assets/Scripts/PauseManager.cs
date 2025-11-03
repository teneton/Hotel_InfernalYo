using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    public GameObject menuCanvas;

    void Start()
    {
        pauseMenuUI.SetActive(false); 
        menuCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Asegura que el tiempo esté activo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarga la escena actual
    }

    public void OpenOptions()
    {
        pauseMenuUI.SetActive(false);     
        menuCanvas.SetActive(true);     
    }

    public void CloseOptions()
    {
        menuCanvas.SetActive(false);     // Oculta el menú de opciones
        pauseMenuUI.SetActive(true);     // Muestra el menú de pausa
    }

}

