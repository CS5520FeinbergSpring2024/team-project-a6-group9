using UnityEngine;

public class Pause : MonoBehaviour
{
    #if UNITY_ANDROID
        private bool paused;
    #endif

    private AnimationController animationController;

    void Start()
    {
        animationController = this.GetComponent<AnimationController>();
    }

    public void PauseGame()
    {
        animationController.OpenWindow();
        #if UNITY_ANDROID
            paused = true;
        #endif
    }

    public void ResumeGame()
    {
        animationController.CloseWindow();
        #if UNITY_ANDROID
            paused = false;
        #endif
    }

    void Update()
    {
        #if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if(!GameOver.instance.isGameOver)
            {
                if(paused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }    
        #endif        
    }
}
