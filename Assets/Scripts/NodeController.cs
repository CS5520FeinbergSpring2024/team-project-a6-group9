using System.Collections;
using UnityEngine;
using TMPro;

public class NodeController : MonoBehaviour
{
    public AudioClip dragSound;
    public AudioClip incorrectSound;
    public AudioClip correctSound;
    public AudioClip winningSound;
    public AudioClip starSound;
    public AudioSource backgroundAudioSource;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI hintsText;
    public TextMeshProUGUI autoCompleteText;
    public TextMeshProUGUI coinsText;
    public GameObject nodePrefab;
    public GameObject heartIcon;
    public GameObject hintIcon;
    public GameObject autoCompleteIcon;
    public ISwapValidator swapValidator;
    public int numNodes;
    private ItemManager itemManager;
    private GameOver gameOver;
    private YouWin youWin;

    private AudioSource audioSource;
    private GameObject[] allNodes;
    private int[] numbersToBeSorted;
    private GameObject selectedNode;
    private Vector3[] snapPositions;
    private bool isSwapping = false;

    private int startIndex;  //the index of node to be swapped


    void Start() {
        audioSource = GetComponentInChildren<AudioSource>();
        itemManager = FindObjectOfType<ItemManager>();
        swapValidator = GetComponent<ISwapValidator>();
        gameOver = FindObjectOfType<GameOver>();
        youWin = FindObjectOfType<YouWin>();

        InitializeGame();

        if (IsArraySorted()) {
            Debug.Log("Array was sorted, regenerating...");
            InitializeGame();
        }

        livesText.text = $"{itemManager.LifeCount}";

        hintsText.text = $"{itemManager?.HintCount ?? 0}";
        autoCompleteText.text=$"{itemManager?.AutoCompleteCount ?? 0}";
        coinsText.text=$"{Wallet.GetAmount()}";
    }

    void InitializeGame() {
        swapValidator ??= FindObjectOfType(typeof(ISwapValidator)) as ISwapValidator;


        Wallet.SetAmount(999); 

        allNodes = new GameObject[numNodes];
        numbersToBeSorted = new int[numNodes];
        snapPositions = new Vector3[allNodes.Length];

        float screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        float spacing = screenWidth / (allNodes.Length + 1);

        Vector3 center = new Vector3(-893, 500, 0);
        Vector3 scale = new Vector3(15,16,1);

        for (int i = 0; i < allNodes.Length; i++) {
            Vector3 snapPosition = new Vector3((i + 1) * spacing - (screenWidth / 2) + center.x, center.y, center.z);
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

    public void ResetSelectedNode() {
        if (selectedNode != null) {
            selectedNode.transform.localScale /= 1.2f;
            selectedNode = null;
        }
    }

    public void ResumeGame() {
        isSwapping = false;
        ResetSelectedNode();
    }

    public void ConsumeLife() {
        if (!itemManager.ConsumeLife()) {
            gameOver.GameOverControl();
        }
        UpdateLifeCountUI();
    }

    public void UpdateLifeCountUI() {
        livesText.text = $"{itemManager.LifeCount}";
        StartCoroutine(PulseEffect(heartIcon, 1.2f, 0.1f));
    }

    public void UpdateHintCountUI() {
        hintsText.text = $"{itemManager?.HintCount ?? 0}";
        StartCoroutine(PulseEffect(hintIcon, 1.2f, 0.1f));
    }
    
    public void UpdateCompleteCountUI() {
        autoCompleteText.text = $"{itemManager?.AutoCompleteCount ?? 0}";
        StartCoroutine(PulseEffect(autoCompleteIcon, 1.2f, 0.1f));
    }

    public void UseHint() {
        if (itemManager.ConsumeHint()) {
            var swapPair = swapValidator.GetNextSwap(allNodes);
            if (swapPair.Item1 != -1 && swapPair.Item2 != -1) {
                StartCoroutine(SwapPositions(allNodes[swapPair.Item1], allNodes[swapPair.Item2]));
            } else {
                Debug.Log("No swap needed or hint not applicable.");
            }
            UpdateHintCountUI();
        }
    }

    public void UseAutoComplete() {
        if (itemManager.ConsumeAutoComplete()) {
            StartCoroutine(AutoCompleteSort());
            UpdateCompleteCountUI();
        }
    }






    // effects
    private IEnumerator SwapPositions(GameObject node1, GameObject node2) {
        int node1Index = System.Array.IndexOf(allNodes, node1);
        int node2Index = System.Array.IndexOf(allNodes, node2);

        if (!swapValidator.IsValidSwap(allNodes, node1Index, node2Index, startIndex)) {
            audioSource.PlayOneShot(incorrectSound);
            StartCoroutine(FlashNodeColor(node1, Color.red, 0.25f));
            StartCoroutine(FlashNodeColor(node2, Color.red, 0.25f));

            if (selectedNode != null) {
                selectedNode.transform.localScale /= 1.2f;
                selectedNode = null;
            }
            ConsumeLife();
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
            youWin.CompleteGame();
            yield break;
        }

        isSwapping = false;
    }

    private IEnumerator MoveToPosition(Transform objectTransform, Vector3 position, float duration) {
        Vector3 startPosition = objectTransform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration) {
            objectTransform.position = Vector3.Lerp(startPosition, position, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectTransform.position = position;
    }

    private IEnumerator ScaleNodeToSize(Transform nodeTransform, Vector3 targetScale, float duration) {
        float elapsedTime = 0;
        Vector3 startingScale = nodeTransform.localScale;

        while (elapsedTime < duration) {
            nodeTransform.localScale = Vector3.Lerp(startingScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        nodeTransform.localScale = targetScale;
    }

    private IEnumerator FlashNodeColor(GameObject node, Color flashColor, float duration) {
        var originalColor = node.GetComponentInChildren<SpriteRenderer>().color;
        node.GetComponentInChildren<SpriteRenderer>().color = flashColor;

        yield return new WaitForSeconds(duration);

        node.GetComponentInChildren<SpriteRenderer>().color = originalColor;
    }

    public IEnumerator PulseEffect(GameObject target, float maxScale, float pulseDuration) {
    Transform targetTransform = target.transform;
    Vector3 originalScale = targetTransform.localScale;
    Vector3 targetScale = originalScale * maxScale;

    float timer = 0;
    while (timer <= pulseDuration / 2) {
        timer += Time.deltaTime;
        targetTransform.localScale = Vector3.Lerp(originalScale, targetScale, timer / (pulseDuration / 2));
        yield return null;
    }

    timer = 0;
    while (timer <= pulseDuration / 2) {
        timer += Time.deltaTime;
        targetTransform.localScale = Vector3.Lerp(targetScale, originalScale, timer / (pulseDuration / 2));
        yield return null;
    }

    targetTransform.localScale = originalScale;
}

    private IEnumerator AutoCompleteSort() {
        while (!IsArraySorted()) {
            var swapPair = swapValidator.GetNextSwap(allNodes);
            if (swapPair.Item1 != -1 && swapPair.Item2 != -1) {
                yield return StartCoroutine(SwapPositions(allNodes[swapPair.Item1], allNodes[swapPair.Item2]));
                yield return new WaitForSeconds(0.5f);
            } else {
                break;
            }
        }
    }
}
