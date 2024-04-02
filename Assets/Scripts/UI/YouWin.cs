using System.Collections;
using UnityEngine;

public class YouWin : MonoBehaviour
{
    public AudioSource backgroundAudioSource;
    public AudioSource audioSource;
    public AudioClip winningSound;
    public AudioClip starSound;
    public GameObject StarFilled1;
    public GameObject StarFilled2;
    public GameObject StarFilled3;

    private ItemManager itemManager;
    private NodeController nodeController;
    private Animation anim;

    void Start()
    {
        anim = this.GetComponent<Animation>();
        nodeController = FindObjectOfType<NodeController>();
        itemManager = FindObjectOfType<ItemManager>();
    }

    public void CompleteGame()
    {
        anim.Play("Window-In");
        int starsEarned = 0;
        if (itemManager.LifeCount >= 5) starsEarned = 3;
        else if (itemManager.LifeCount >= 3) starsEarned = 2;
        else if (itemManager.LifeCount >= 1) starsEarned = 1;

        int coinsToAdd = (starsEarned == 3) ? 100 : (starsEarned == 2) ? 60 : 30;
        Wallet.SetAmount(Wallet.GetAmount() + coinsToAdd);

        backgroundAudioSource.Stop();
        backgroundAudioSource.PlayOneShot(winningSound);

        StartCoroutine(DisplayStarsSequence(starsEarned));
    }

    private IEnumerator DisplayStarsSequence(int starsEarned)
    {
        int initialCoins = Wallet.GetAmount();
        int coinsToAdd = (starsEarned == 3) ? 100 : (starsEarned == 2) ? 60 : 30;
        int finalCoins = initialCoins + coinsToAdd;

        yield return new WaitForSecondsRealtime(0.5f);

        if (starsEarned >= 1) {
            StarFilled1.SetActive(true);
            StartCoroutine(nodeController.PulseEffect(StarFilled1, 1.5f, 1.0f));
            audioSource.PlayOneShot(starSound);
            yield return new WaitForSecondsRealtime(1.0f);
        }

        if (starsEarned >= 2) {
            StarFilled2.SetActive(true);
            StartCoroutine(nodeController.PulseEffect(StarFilled2, 1.5f, 1.0f));
            audioSource.PlayOneShot(starSound);
            yield return new WaitForSecondsRealtime(1.0f);
        }

        if (starsEarned >= 3) {
            StarFilled3.SetActive(true);
            StartCoroutine(nodeController.PulseEffect(StarFilled3, 1.5f, 1.0f));
            audioSource.PlayOneShot(starSound);
            yield return new WaitForSecondsRealtime(1.0f);
        }
    }

}
