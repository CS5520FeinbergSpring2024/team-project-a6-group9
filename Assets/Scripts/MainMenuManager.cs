using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject infoDialog; // Assign this in the inspector within Info Dialog Panel

    void Start()
    {
        // Ensure the dialog is initially hidden
        infoDialog.SetActive(false);
    }
    // Toggle function for the dialog
    public void ToggleInfoDialog()
    {
        if (infoDialog != null)
            infoDialog.SetActive(!infoDialog.activeSelf); // Toggle the active state
    }
    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelectMenu"); 
    }
}