using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ButtonBehavior : MonoBehaviour
{
    public AudioSource uiAudioSource;
    public AudioClip buttonSound;
    private bool playButtonSound = true;

    public void PlayButtonSound()
    {
        if (playButtonSound && uiAudioSource != null && buttonSound != null)
        {
            uiAudioSource.PlayOneShot(buttonSound);
        }
    }
}
