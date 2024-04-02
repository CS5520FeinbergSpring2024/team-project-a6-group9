using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public Sprite soundOn, soundOff, vibrationOn, vibrationOff;
    public Image soundImage, vibrationImage;

    void Start()
    {
        SetSounds();
        SetVibration();
    }

    public void ChangeSounds()
    {
        bool active = GetSetting("Sounds");
        
        if(active)
        {
            soundImage.sprite = soundOff;
            AudioListener.volume = 0.0f;
            ChangeSetting("Sounds", 0);
        }
        else
        {
            soundImage.sprite = soundOn;
            AudioListener.volume = 1.0f;
            ChangeSetting("Sounds", 1);
        }
    }

    public void ChangeVibration()
    {
        bool active = GetSetting("Vibration");
        
        if(active)
        {
            vibrationImage.sprite = vibrationOff;
            ChangeSetting("Vibration", 0);
        }
        else
        {
            vibrationImage.sprite = vibrationOn;
            ChangeSetting("Vibration", 1);
        }
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static bool GetSetting(string name)
    {
        return PlayerPrefs.GetInt(name, 1) == 1 ? true : false;
    }

    private void ChangeSetting(string name, int state)
    {
        PlayerPrefs.SetInt(name, state);
    }

    private void SetSounds()
    {
        bool active = GetSetting("Sounds");
        
        if(active)
        {
            soundImage.sprite = soundOn;
            AudioListener.volume = 1.0f;
        }
        else
        {
            soundImage.sprite = soundOff;
            AudioListener.volume = 0.0f;
        }
    }

    private void SetVibration()
    {
        bool active = GetSetting("Vibration");
        
        if(active)
        {
            vibrationImage.sprite = vibrationOn;
        }
        else
        {
            vibrationImage.sprite = vibrationOff;
        }
    }
}
