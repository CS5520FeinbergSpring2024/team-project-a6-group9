using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class NavigationHandler : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject shopMenu;
    public GameObject confirmationDialogHome;
    public GameObject confirmationDialogBack;
    public GameObject confirmationDialogRestart;

    public GameObject pauseButton;
    public GameObject playButton;
    public GameObject soundOnButton;
    public GameObject soundOffButton;
    public GameObject backButton;
    public GameObject homeButton;
    public GameObject settingButton;
    public GameObject shopButton;
    public GameObject restartButton;
    public AudioSource backgroundMusic;
    public AudioSource uiAudioSource;
    public AudioClip buttonSound;
    public AudioClip shopMusicClip;
    private AudioClip mainMusicClip;
    
    

    void Start()
    {
        if (backgroundMusic.isPlaying)
        {
            mainMusicClip = backgroundMusic.clip;
            soundOffButton.SetActive(true);
            soundOnButton.SetActive(false);
        }

        pauseButton.SetActive(true);
        playButton.SetActive(false);
    }

    public void SetButtonInteractable(GameObject button, bool interactable)
    {
        if (button.GetComponent<Button>() != null)
            button.GetComponent<Button>().interactable = interactable;
    }

    public void PlayButtonSound()
    {
        if (uiAudioSource != null && buttonSound != null)
        {
            uiAudioSource.PlayOneShot(buttonSound);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (backgroundMusic != null && backgroundMusic.isPlaying)
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

    public void PauseGameButton()
    {
        PauseGame();
        pauseButton.SetActive(false);
        playButton.SetActive(true);
    }

    public void UnpauseGameButton()
    {
        UnpauseGame();
        pauseButton.SetActive(true);
        playButton.SetActive(false);
    }


    public void PauseSound()
    {
        if (backgroundMusic != null && backgroundMusic.isPlaying)
        {
            backgroundMusic.Pause();
        }
        soundOnButton.SetActive(true);
        soundOffButton.SetActive(false);
    }

    public void UnpauseSound()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
        soundOnButton.SetActive(false);
        soundOffButton.SetActive(true);
    }

    public void ToggleSettings()
    {
        bool isActive = !settingsMenu.activeSelf;
        settingsMenu.SetActive(isActive);
        SetButtonInteractable(playButton, !isActive);
        SetButtonInteractable(pauseButton, !isActive);
        SetButtonInteractable(soundOnButton, !isActive);
        SetButtonInteractable(soundOffButton, !isActive);
        SetButtonInteractable(shopButton, !isActive);
        SetButtonInteractable(backButton, !isActive);
        SetButtonInteractable(homeButton, !isActive);
        SetButtonInteractable(restartButton, !isActive);
        if (isActive) PauseGame();
        else UnpauseGame();
    }

    public void ToggleShop()
    {
        bool isActive = !shopMenu.activeSelf;
        shopMenu.SetActive(isActive);
        SetButtonInteractable(playButton, !isActive);
        SetButtonInteractable(pauseButton, !isActive);
        SetButtonInteractable(soundOnButton, !isActive);
        SetButtonInteractable(soundOffButton, !isActive);
        SetButtonInteractable(settingButton, !isActive);
        SetButtonInteractable(backButton, !isActive);
        SetButtonInteractable(homeButton, !isActive);
        SetButtonInteractable(restartButton, !isActive);
        if (isActive)
        {
            Time.timeScale = 0f;
            if (backgroundMusic != null && shopMusicClip != null)
            {
                backgroundMusic.Stop();
                backgroundMusic.clip = shopMusicClip;
                backgroundMusic.Play();
            }
        }
        else
        {
            Time.timeScale = 1f;
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
        if (!confirmationDialogHome.activeSelf)
        {
            confirmationDialogHome.SetActive(true);
            PauseGame();
            SetButtonInteractable(playButton, false);
            SetButtonInteractable(pauseButton, false);
            SetButtonInteractable(soundOnButton, false);
            SetButtonInteractable(soundOffButton, false);
            SetButtonInteractable(settingButton, false);
            SetButtonInteractable(backButton, false);
            SetButtonInteractable(shopButton, false);
            SetButtonInteractable(restartButton, false);
        }
        else
        {
            StartCoroutine(ShakeGameObject(confirmationDialogHome, 0.5f, 10f));
        }
    }

    public void ConfirmGoToHome()
    {
        UnpauseGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelGoToHome()
    {
        confirmationDialogHome.SetActive(false);
        UnpauseGame();
        SetButtonInteractable(playButton, true);
        SetButtonInteractable(pauseButton, true);
        SetButtonInteractable(soundOnButton, true);
        SetButtonInteractable(soundOffButton, true);
        SetButtonInteractable(settingButton, true);
        SetButtonInteractable(backButton, true);
        SetButtonInteractable(shopButton, true);
        SetButtonInteractable(restartButton, true);
    }

    public void RequestGoToLevelSelect()
    {
        if (!confirmationDialogBack.activeSelf)
        {
            confirmationDialogBack.SetActive(true);
            PauseGame();
            SetButtonInteractable(playButton, false);
            SetButtonInteractable(pauseButton, false);
            SetButtonInteractable(soundOnButton, false);
            SetButtonInteractable(soundOffButton, false);
            SetButtonInteractable(settingButton, false);
            SetButtonInteractable(homeButton, false);
            SetButtonInteractable(shopButton, false);
            SetButtonInteractable(restartButton, false);
        }
        else
        {
            StartCoroutine(ShakeGameObject(confirmationDialogBack, 0.5f, 10f));
        }
    }

    public void ConfirmGoToLevelSelect()
    {
        UnpauseGame();
        SceneManager.LoadScene("LevelSelectMenu");
    }

    public void CancelGoToLevelSelect()
    {
        confirmationDialogBack.SetActive(false);
        UnpauseGame();
        SetButtonInteractable(playButton, true);
        SetButtonInteractable(pauseButton, true);
        SetButtonInteractable(soundOnButton, true);
        SetButtonInteractable(soundOffButton, true);
        SetButtonInteractable(settingButton, true);
        SetButtonInteractable(homeButton, true);
        SetButtonInteractable(shopButton, true);
        SetButtonInteractable(restartButton, true);
    }

    public void RequestRestart()
    {
        if (!confirmationDialogRestart.activeSelf)
        {
            confirmationDialogRestart.SetActive(true);
            PauseGame();
            SetButtonInteractable(playButton, false);
            SetButtonInteractable(pauseButton, false);
            SetButtonInteractable(soundOnButton, false);
            SetButtonInteractable(soundOffButton, false);
            SetButtonInteractable(settingButton, false);
            SetButtonInteractable(homeButton, false);
            SetButtonInteractable(shopButton, false);
            SetButtonInteractable(backButton, false);
        }
        else
        {
            StartCoroutine(ShakeGameObject(confirmationDialogRestart, 0.5f, 10f));
        }
    }

    public void ConfirmRestart()
    {
        UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CancelRestart()
    {
        confirmationDialogRestart.SetActive(false);
        UnpauseGame();
        SetButtonInteractable(playButton, true);
        SetButtonInteractable(pauseButton, true);
        SetButtonInteractable(soundOnButton, true);
        SetButtonInteractable(soundOffButton, true);
        SetButtonInteractable(settingButton, true);
        SetButtonInteractable(homeButton, true);
        SetButtonInteractable(shopButton, true);
        SetButtonInteractable(backButton, true);
    }

    IEnumerator ShakeGameObject(GameObject obj, float duration, float magnitude)
    {
        Vector3 originalPosition = obj.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = originalPosition.x + Random.Range(-1f, 1f) * magnitude;
            float y = originalPosition.y + Random.Range(-1f, 1f) * magnitude;

            obj.transform.localPosition = new Vector3(x, y, originalPosition.z);
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        obj.transform.localPosition = originalPosition;
    }

}
