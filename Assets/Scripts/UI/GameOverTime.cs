using UnityEngine;
using TMPro;
using UnityEngine.Timeline;

public class GameOverTime : MonoBehaviour
{
    public int timePrice;
    public int additionalTime;
    public GameObject menu;
    public TextMeshProUGUI priceText;
    public Animation notEnough;
    public LevelLoader levelLoader;
    public AudioSource backgroundAudioSource;
    public AudioClip gameOverSound;
    [HideInInspector]
    public bool isGameOver;

    public static GameOverTime instance;

    private Animation anim;
    private NodeController nodeController;
    private TimerController timerController;


    void Start()
    {
        instance = this;
        anim = this.GetComponent<Animation>();
        timerController = FindObjectOfType<TimerController>();
        nodeController = FindObjectOfType<NodeController>();
        priceText.text = timePrice.ToString();
        isGameOver = false;
    }

    public void GameOverControl()
    {
        isGameOver = true;

        anim.Play("Window-In");
        menu.SetActive(false);
        backgroundAudioSource.Stop();
        backgroundAudioSource.PlayOneShot(gameOverSound);
    }

    // Method to be called when the player wants to purchase more time
    public void Continue()
    {
        if (Wallet.GetAmount() >= timePrice)
        {
            Wallet.SetAmount(Wallet.GetAmount() - timePrice);

            anim.Play("Window-Out");

            isGameOver = false;
            timerController.AddTime(additionalTime);
            backgroundAudioSource.Stop();
            backgroundAudioSource.Play();
            menu.SetActive(true);
            nodeController.ResumeGame();
        }
        else
        {
            notEnough.Play("Not-Enough-In");
        }
    }
}
