using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeController : MonoBehaviour
{
    // game effects and UI variables - do not modify
    public AudioClip dragSound;
    public AudioClip incorrectSound;
    public AudioClip correctSound;
    public AudioClip gameOverSound;
    public AudioSource backgroundAudioSource;
    private AudioSource audioSource;
    public GameObject nodePrefab;
    public GameObject heartIcon;
    public int numNodes;
    public int numLives;
    public TextMeshProUGUI livesText;
    public GameObject gameOverDialog;

    private GameObject[] allNodes;
    private GameObject selectedNode;
    private Vector3[] snapPositions;
    private bool isSwapping = false;

    // sort specific variable
    private int sortedBoundary;


    void Start() {
        audioSource = GetComponentInChildren<AudioSource>();
        UpdateLives();
        
        sortedBoundary = numNodes;
        allNodes = new GameObject[numNodes];
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
    }
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


    // all sorting algorithm check goes here
    private bool IsValidSwap(int index1, int index2) {
        if (index1 == index2 - 1) {
            return int.Parse(allNodes[index1].GetComponentInChildren<TextMeshPro>().text) >
                int.Parse(allNodes[index2].GetComponentInChildren<TextMeshPro>().text);
        }

        return false;
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
        StartCoroutine(PulseEffect());
    }

    private void GameOver() {
        Time.timeScale = 0;
        backgroundAudioSource.Stop();
        backgroundAudioSource.PlayOneShot(gameOverSound);
        gameOverDialog.SetActive(true);
    }

    IEnumerator SwapPositions(GameObject node1, GameObject node2) {
        int node1Index = System.Array.IndexOf(allNodes, node1);
        int node2Index = System.Array.IndexOf(allNodes, node2);

        if (!IsValidSwap(node1Index, node2Index)) {
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

            if (selectedNode != null) {
                selectedNode.transform.localScale /= 1.2f;
                selectedNode = null;
            }
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

    IEnumerator PulseEffect() {
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
}
