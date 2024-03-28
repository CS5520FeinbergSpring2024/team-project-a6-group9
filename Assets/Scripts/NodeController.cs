using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeController : MonoBehaviour
{
    public AudioClip dragSound;
    public AudioClip dropSound;
    public AudioSource backgroundAudioSource;
    public AudioClip gameOverSound;
    public GameObject nodePrefab;
    public int numNodes;
    public int numLives;
    public TextMeshProUGUI livesText;
    public GameObject wrongIndicator;
    public GameObject gameOverDialog;

    private GameObject[] allNodes;
    private GameObject selectedNode;
    private Vector3[] snapPositions;
    private bool isSwapping = false;


    void Start() {
        UpdateLives();
        
        wrongIndicator.gameObject.SetActive(false);
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

    private void HandleNodeSelection(GameObject node) {
        if (selectedNode == null) {
            selectedNode = node;
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

private bool IsValidSwap(int index1, int index2) {
    if (index1 == index2 - 1) {
        return int.Parse(allNodes[index1].GetComponentInChildren<TextMeshPro>().text) >
               int.Parse(allNodes[index2].GetComponentInChildren<TextMeshPro>().text);
    }

    return false;
}


    private void UpdateLives() {
        livesText.text = $"{numLives}";
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
            StartCoroutine(ShowIncorrectFeedback());

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

    IEnumerator ShowIncorrectFeedback() {
        wrongIndicator.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        wrongIndicator.gameObject.SetActive(false);
}
}
