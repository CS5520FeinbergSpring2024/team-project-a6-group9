using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public int continuePrice;

    public GameObject menu;
    public TextMeshProUGUI priceText;
    public Animation notEnough;
    public LevelLoader levelLoader;
    [HideInInspector]
    public bool crashed;

    public static GameOver instance;

    private Animation anim;

    void Start()
    {
        instance = this;
        anim = this.GetComponent<Animation>();
        priceText.text = continuePrice.ToString();
        crashed = false;
    }

    // When player crashes to obstacle.
    public void Crashed()
    {
        crashed = true;

        // Play game over window open animation.
        anim.Play("Game-Over-In");
        // Disable game menu gameobject with all buttons.
        menu.SetActive(false);
    }

    // If player selects continue button.
    public void Continue()
    {
        // If player has enough money to continue.
        if(Wallet.GetAmount() >= continuePrice)
        {
            // Subract continue price from player wallet.
            Wallet.SetAmount(Wallet.GetAmount() - continuePrice);
            // Load game scene.
            levelLoader.LoadLevel(1);
        }
        else
        {
            //Play not enough money animation.
            notEnough.Play("Not-Enough-In");
        }
    }
}
