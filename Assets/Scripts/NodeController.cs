using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class NodeController : MonoBehaviour
{
    public AudioClip dragSound;
    public AudioClip incorrectSound;
    public AudioClip correctSound;
    public AudioClip gameOverSound;
    public AudioClip winningSound;
    public AudioClip starSound;
    public AudioSource backgroundAudioSource;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI coinsEarnedText;
    public GameObject nodePrefab;
    public GameObject heartIcon;
    public GameObject gameOverDialog;
    public GameObject winningDialog;
    public GameObject StarFilled1;
    public GameObject StarFilled2;
    public GameObject StarFilled3;
    public ISwapValidator swapValidator;
    public int numNodes;
    public int numLives;
    public string nextLevelSceneName;

    private AudioSource audioSource;
    private GameObject[] allNodes;
    private int[] numbersToBeSorted;
    private GameObject selectedNode;
    private Vector3[] snapPositions;
    private bool isSwapping = false;
    private int playerCoin;

    private int startIndex;  //the index of node to be swapped


    void Start() {
        audioSource = GetComponentInChildren<AudioSource>();
        livesText.text = $"{numLives}";

        swapValidator = GetComponent<ISwapValidator>();

        if (swapValidator == null) {
            swapValidator = FindObjectOfType(typeof(ISwapValidator)) as ISwapValidator;
        }

        if (swapValidator == null) {
            Debug.LogError("No swap validator found in the scene!");
            return;
        }

        allNodes = new GameObject[numNodes];
        numbersToBeSorted = new int[numNodes];
        snapPositions = new Vector3[allNodes.Length];

        float screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        float spacing = screenWidth / (allNodes.Length + 1);

        Vector3 center = new Vector3(-893, 500, 0);
        Vector3 scale = new Vector3(15,16,1);

        for (int i = 0; i < allNodes.Length; i++) {
            Vector3 snapPosition = new Vector3((i + 1) * spacing - (screenWidth / 2)+center.x, center.y, center.z);
            snapPositions[i] = snapPosition;

            GameObject node = Instantiate(nodePrefab, snapPosition, Quaternion.identity, this.transform);
            node.tag = "Node";
            node.transform.localScale = Vector3.zero;
            allNodes[i] = node;

            StartCoroutine(ScaleNodeToSize(node.transform, scale, 0.5f));

            int randomNumber = Random.Range(1, 100);
            TextMeshPro textComponent = node.GetComponentInChildren<TextMeshPro>();
            if (textComponent != null) {
                textComponent.text = randomNumber.ToString();
            }
            numbersToBeSorted[i] = randomNumber;
        }
        startIndex = 0;
        swapValidator.SetNumbersToBeSorted(numbersToBeSorted);

        // set to 100 for now
        PlayerPrefs.SetInt("PlayerCoin", 100);
        playerCoin = PlayerPrefs.GetInt("PlayerCoin");

        // if (!PlayerPrefs.HasKey("PlayerCoin")) {
        //     playerCoin = 100;
        //     PlayerPrefs.SetInt("PlayerCoin", playerCoin);
        // } else {
        //     playerCoin = PlayerPrefs.GetInt("PlayerCoin");
        // }
        coinsEarnedText.text = $"{playerCoin}";
    }

    void Update()
    {
        if (isSwapping) return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Node"))
            {
                HandleNodeSelection(hit.collider.gameObject);
            }
        }
    }

    private bool IsArraySorted() {
        for (int i = 0; i < allNodes.Length - 1; i++) {
            int currentValue = int.Parse(allNodes[i].GetComponentInChildren<TextMeshPro>().text);
            int nextValue = int.Parse(allNodes[i + 1].GetComponentInChildren<TextMeshPro>().text);
            if (currentValue > nextValue) {
                return false;
            }
        }
        return true;
    }

    private void HandleNodeSelection(GameObject node) {
        if (selectedNode == null) {
            selectedNode = node;
            audioSource.PlayOneShot(dragSound);
            selectedNode.transform.localScale *= 1.2f;

        } else {
            if (node != selectedNode) {
                isSwapping = true;
                StartCoroutine(SwapPositions(selectedNode, node));
            } else {
                selectedNode.transform.localScale /= 1.2f;
                selectedNode = null;
            }
        }
    }

    private void UpdateLives() {
        livesText.text = $"{numLives}";
        StartCoroutine(PulseHeartEffect());
    }

    public void GameOver() {
        Time.timeScale = 0;
        backgroundAudioSource.Stop();
        backgroundAudioSource.PlayOneShot(gameOverSound);
        gameOverDialog.SetActive(true);
    }

    private void CompleteGame() {
        Time.timeScale = 0;
        int starsEarned = 0;
        if (numLives >= 5) {
            starsEarned = 3;
            playerCoin += 100;
        }
        else if (numLives >= 3) {
            starsEarned = 2;
            playerCoin += 60;
        }
        else if (numLives >= 1) {
            starsEarned = 1;
            playerCoin += 30;
        }
        PlayerPrefs.SetInt("PlayerCoin", playerCoin);
        PlayerPrefs.Save();

        backgroundAudioSource.Stop();
        backgroundAudioSource.PlayOneShot(winningSound);
        winningDialog.SetActive(true);

        StartCoroutine(DisplayStarsSequence(starsEarned));
    }

    public void LoadNextLevel() {
        if (!string.IsNullOrEmpty(nextLevelSceneName)) {
            SceneManager.LoadScene(nextLevelSceneName);
        } else {
            Debug.LogError("Next level scene name is not set!");
        }
    }

    IEnumerator SwapPositions(GameObject node1, GameObject node2) {
        int node1Index = System.Array.IndexOf(allNodes, node1);
        int node2Index = System.Array.IndexOf(allNodes, node2);

        if (!swapValidator.IsValidSwap(allNodes, node1Index, node2Index, startIndex)) {
            numLives--;
            UpdateLives();
            audioSource.PlayOneShot(incorrectSound);
            StartCoroutine(FlashNodeColor(node1, Color.red, 0.25f));
            StartCoroutine(FlashNodeColor(node2, Color.red, 0.25f));

            if (numLives <= 0) {
                GameOver();
                yield break;
            }

            if (selectedNode != null) {
                selectedNode.transform.localScale /= 1.2f;
                selectedNode = null;
            }
        } else {
            StartCoroutine(MoveToPosition(node1.transform, snapPositions[node2Index], 0.25f));
            StartCoroutine(MoveToPosition(node2.transform, snapPositions[node1Index], 0.25f));
            StartCoroutine(FlashNodeColor(node1, Color.green, 0.25f));
            StartCoroutine(FlashNodeColor(node2, Color.green, 0.25f));
            audioSource.PlayOneShot(correctSound);
            yield return new WaitForSeconds(0.25f);

            allNodes[node1Index] = node2;
            allNodes[node2Index] = node1;

            startIndex++;

            if (selectedNode != null) {
                selectedNode.transform.localScale /= 1.2f;
                selectedNode = null;
            }
        }

        if (IsArraySorted()) {
            CompleteGame();
            yield break;
        }

        isSwapping = false;
    }


    IEnumerator MoveToPosition(Transform objectTransform, Vector3 position, float duration) {
        Vector3 startPosition = objectTransform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration) {
            objectTransform.position = Vector3.Lerp(startPosition, position, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectTransform.position = position;
    }

    IEnumerator ScaleNodeToSize(Transform nodeTransform, Vector3 targetScale, float duration) {
        float elapsedTime = 0;
        Vector3 startingScale = nodeTransform.localScale;

        while (elapsedTime < duration) {
            nodeTransform.localScale = Vector3.Lerp(startingScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        nodeTransform.localScale = targetScale;
    }

    IEnumerator FlashNodeColor(GameObject node, Color flashColor, float duration) {
        var originalColor = node.GetComponentInChildren<SpriteRenderer>().color;
        node.GetComponentInChildren<SpriteRenderer>().color = flashColor;

        yield return new WaitForSeconds(duration);

        node.GetComponentInChildren<SpriteRenderer>().color = originalColor;
    }

    IEnumerator PulseHeartEffect() {
        Transform heartTransform = heartIcon.transform;

        float timeToScale = 0.1f;
        float maxScale = 1.5f;
        float timer = 0;

        while (timer <= timeToScale) {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(1.0f, maxScale, timer / timeToScale);
            heartTransform.localScale = new Vector3(scale, scale, 1);
            livesText.transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }

        timer = 0;
        while (timer <= timeToScale) {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(maxScale, 1.0f, timer / timeToScale);
            heartTransform.localScale = new Vector3(scale, scale, 1);
            livesText.transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }

        heartTransform.localScale = Vector3.one;
        livesText.transform.localScale = Vector3.one;
    }

    IEnumerator DisplayStarsSequence(int starsEarned) {
        int initialCoins = playerCoin - (starsEarned == 3 ? 100 : starsEarned == 2 ? 60 : 30);
        int finalCoins = playerCoin;

        yield return new WaitForSecondsRealtime(0.5f);

        if (starsEarned >= 1) {
            StarFilled1.SetActive(true);
            StartCoroutine(PulseStar(StarFilled1));
            audioSource.PlayOneShot(starSound);
            yield return new WaitForSecondsRealtime(1.0f);
        }

        if (starsEarned >= 2) {
            StarFilled2.SetActive(true);
            StartCoroutine(PulseStar(StarFilled2));
            audioSource.PlayOneShot(starSound);
            yield return new WaitForSecondsRealtime(1.0f);
        }

        if (starsEarned >= 3) {
            StarFilled3.SetActive(true);
            StartCoroutine(PulseStar(StarFilled3));
            audioSource.PlayOneShot(starSound);
            yield return new WaitForSecondsRealtime(1.0f);
        }

        StartCoroutine(UpdateCoinText(initialCoins, finalCoins));
    }

    IEnumerator PulseStar(GameObject star) {
        float pulseDuration = 1.0f;
        Vector3 originalScale = star.transform.localScale;
        Vector3 targetScale = originalScale * 1.5f;

        float timer = 0;
        while (timer <= pulseDuration / 2) {
            timer += Time.unscaledDeltaTime;
            star.transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / (pulseDuration / 2));
            yield return null;
        }
        timer = 0;
        while (timer <= pulseDuration / 2) {
            timer += Time.unscaledDeltaTime;
            star.transform.localScale = Vector3.Lerp(targetScale, originalScale, timer / (pulseDuration / 2));
            yield return null;
        }

        star.transform.localScale = originalScale;
    }

    IEnumerator UpdateCoinText(int initialCoins, int finalCoins) {
        while (initialCoins < finalCoins) {
            initialCoins++;
            coinsEarnedText.text = $"{initialCoins}";
            yield return new WaitForSecondsRealtime(0.02f);
        }
    }
}
