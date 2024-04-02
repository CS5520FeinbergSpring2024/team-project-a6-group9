using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public int continuePrice;
    public GameObject menu;
    public TextMeshProUGUI priceText;
    public Animation notEnough;
    public LevelLoader levelLoader;
    public AudioSource backgroundAudioSource;
    public AudioClip gameOverSound;
    [HideInInspector]
    public bool isGameOver;

    public static GameOver instance;

    private Animation anim;
    private Pause pauseScript;

    void Start()
    {
        instance = this;
        anim = this.GetComponent<Animation>();
        pauseScript = FindObjectOfType<Pause>();
        priceText.text = continuePrice.ToString();
        isGameOver = false;
    }

    public void GameOverControl()
    {
        isGameOver = true;

        anim.Play("Game-Over-In");
        menu.SetActive(false);
        backgroundAudioSource.Stop();
        backgroundAudioSource.PlayOneShot(gameOverSound);
    }

    // If player selects continue button.
    public void Continue()
    {
        // If player has enough money to continue.
        if(Wallet.GetAmount() >= continuePrice)
        {
            Wallet.SetAmount(Wallet.GetAmount() - continuePrice);

            pauseScript.ResumeGame(); 

            anim.Play("Window-Out");
            menu.SetActive(true);
            isGameOver = false;
        }
        else
        {
            //Play not enough money animation.
            notEnough.Play("Not-Enough-In");
        }
    }
}
