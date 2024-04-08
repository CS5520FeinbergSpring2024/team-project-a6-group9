using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public bool rateGameOnExit;

    public RateGame rateGame;

    private bool windowOpened;

    void Update()
    {
        #if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if(!windowOpened)
            {
                Exit();
            }
        }    
        #endif    
    }

    public void Exit()
    {
        if(rateGameOnExit)
        {
            rateGame.OpenWindow();
        }
        else
        {
            Application.Quit();
        }
    }

    public void WindowOpened(bool opened)
    {
        windowOpened = opened;
    }
}
