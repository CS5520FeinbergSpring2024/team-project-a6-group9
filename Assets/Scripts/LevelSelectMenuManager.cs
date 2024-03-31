using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    // Call this method when the "Home" button is clicked
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Call this method when the "Back" button is clicked
    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
