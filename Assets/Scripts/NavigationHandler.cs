using UnityEngine.SceneManagement;
using UnityEngine;

public class NavigationHandler : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject shopMenu;
    public GameObject confirmationDialog;
    public AudioSource backgroundMusic;
    public AudioClip shopMusicClip;
    private AudioClip mainMusicClip;

    void Start()
    {
        if (backgroundMusic != null && backgroundMusic.clip != null)
        {
            mainMusicClip = backgroundMusic.clip;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (backgroundMusic != null)
        {
            backgroundMusic.Pause();
        }
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

    public void ToggleSettings()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
        if (settingsMenu.activeSelf)
            PauseGame();
        else
            UnpauseGame();
    }

    public void ToggleShop()
    {
        shopMenu.SetActive(!shopMenu.activeSelf);
        if (shopMenu.activeSelf)
        {
            PauseGame();
            if (backgroundMusic != null && shopMusicClip != null)
            {
                backgroundMusic.Stop();
                backgroundMusic.clip = shopMusicClip;
                backgroundMusic.Play();
            }
        }
        else
        {
            UnpauseGame();
            if (backgroundMusic != null && mainMusicClip != null)
            {
                backgroundMusic.Stop();
                backgroundMusic.clip = mainMusicClip;
                backgroundMusic.Play();
            }
        }
    }

    public void RequestGoToHome()
    {
        confirmationDialog.SetActive(true);
        PauseGame();
    }

    public void ConfirmGoToHome()
    {
        UnpauseGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelGoToHome()
    {
        confirmationDialog.SetActive(false);
        UnpauseGame();
    }
}
