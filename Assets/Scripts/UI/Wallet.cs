using UnityEngine;
using TMPro;
using System.Collections;

public class Wallet : MonoBehaviour
{
    private static int amount;
    private static Wallet instance; 
    private TextMeshProUGUI walletText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        amount = PlayerPrefs.GetInt("WalletAmount", 0);
        walletText = GetComponent<TextMeshProUGUI>();
        UpdateDisplayAmount();
    }

    public static int GetAmount()
    {
        return amount;
    }

    public static void SetAmount(int amountToSet)
    {
        instance.ChangeAmount(amountToSet);
    }

    public void ChangeAmount(int amountToSet)
    {
        StartCoroutine(UpdateCoinText(amount, amountToSet));
        amount = amountToSet;
        PlayerPrefs.SetInt("WalletAmount", amount);
    }

    private IEnumerator UpdateCoinText(int initialCoins, int finalCoins)
    {
        while (initialCoins != finalCoins)
        {
            initialCoins += (initialCoins < finalCoins) ? 1 : -1;
            if(walletText != null)
            {
                walletText.text = initialCoins.ToString();
            }
            yield return new WaitForSecondsRealtime(0.02f);
        }
    }

    private void UpdateDisplayAmount()
    {
        if (walletText != null)
        {
            walletText.text = amount.ToString();
        }
    }
}
