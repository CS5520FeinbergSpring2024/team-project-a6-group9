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
    private NodeController nodeController;
    private ItemManager itemManager;

    void Start()
    {
        instance = this;
        anim = this.GetComponent<Animation>();
        nodeController = FindObjectOfType<NodeController>();
        itemManager = FindObjectOfType<ItemManager>();
        priceText.text = continuePrice.ToString();
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

    // If player selects continue button.
    public void Continue()
    {
        // If player has enough money to continue.
        if(Wallet.GetAmount() >= continuePrice)
        {
            Wallet.SetAmount(Wallet.GetAmount() - continuePrice);

            anim.Play("Window-Out");
            
            isGameOver = false;
            itemManager.AddItem("Life", 1);
            nodeController.UpdateLifeCountUI();
            backgroundAudioSource.Stop();
            backgroundAudioSource.Play();
            menu.SetActive(true);
            nodeController.ResumeGame();
        }
        else
        {
            //Play not enough money animation.
            notEnough.Play("Not-Enough-In");
        }
    }
}
