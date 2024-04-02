using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class NavigationHandler : MonoBehaviour
{
    public GameObject shopMenu;
    public GameObject confirmationDialogHome;
    public GameObject confirmationDialogBack;

    public GameObject pauseButton;
    public GameObject playButton;
    public GameObject backButton;
    public GameObject homeButton;
    public GameObject settingButton;
    public GameObject shopButton;
    public AudioSource backgroundMusic;
    public AudioSource uiAudioSource;
    public AudioClip buttonSound;
    public AudioClip errorSound;
    public AudioClip shopMusicClip;
    private AudioClip mainMusicClip;
    private bool playButtonSound = true;
    

    public void SetButtonsInteractableExcept(GameObject exceptionButton, bool interactable) {
        SetButtonInteractable(playButton, playButton == exceptionButton || interactable);
        SetButtonInteractable(settingButton, settingButton == exceptionButton || interactable);
        SetButtonInteractable(shopButton, shopButton == exceptionButton || interactable);
        SetButtonInteractable(backButton, backButton == exceptionButton || interactable);
        SetButtonInteractable(homeButton, homeButton == exceptionButton || interactable);
    }

    public void SetButtonInteractable(GameObject button, bool interactable)
    {
        if (button.GetComponent<Button>() != null)
            button.GetComponent<Button>().interactable = interactable;
    }

    public void PlayButtonSound()
    {
        if (playButtonSound && uiAudioSource != null && buttonSound != null)
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
    
    public void ToggleShop()
    {
        bool isActive = !shopMenu.activeSelf;
        shopMenu.SetActive(isActive);
        SetButtonsInteractableExcept(shopButton, !isActive);
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
            SetButtonsInteractableExcept(homeButton, false);
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
        SetButtonsInteractableExcept(null, true);
    }

    public void RequestGoToLevelSelect()
    {
        if (!confirmationDialogBack.activeSelf)
        {
            confirmationDialogBack.SetActive(true);
            PauseGame();
            SetButtonsInteractableExcept(backButton, false);
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
        SetButtonsInteractableExcept(null, true);
    }


    IEnumerator ShakeGameObject(GameObject obj, float duration, float magnitude)
    {
        playButtonSound = false;
        if (uiAudioSource != null && errorSound != null)
        {
            uiAudioSource.PlayOneShot(errorSound);
        }
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
        playButtonSound = true;
    }

}
