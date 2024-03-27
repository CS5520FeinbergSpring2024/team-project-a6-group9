using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeController : MonoBehaviour
{
    public AudioClip dragSound;
    public AudioClip dropSound;
    public GameObject nodePrefab;
    public int numNodes;
    private AudioSource audioSource;
    private GameObject[] allNodes;
    private GameObject selectedNode;
    private Vector3 originalPosition;
    private float swapThreshold = 1.0f;
    private Vector3[] snapPositions;
    private Vector3 originalScale;


    void Start() {
        audioSource = GetComponentInChildren<AudioSource>();

        allNodes = new GameObject[numNodes];
        snapPositions = new Vector3[allNodes.Length];

        float screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        float spacing = screenWidth / (allNodes.Length + 1);

        Vector3 center = new Vector3(-893, 500, 0);

        for (int i = 0; i < allNodes.Length; i++) {
            GameObject node = Instantiate(nodePrefab, center, Quaternion.identity, this.transform);
            node.tag = "Node";
            allNodes[i] = node;

            int randomNumber = Random.Range(1, 100);
            TextMeshPro textComponent = node.GetComponentInChildren<TextMeshPro>();
            if (textComponent != null) {
                textComponent.text = randomNumber.ToString();
            }

            snapPositions[i] = new Vector3((i + 1) * spacing - (screenWidth / 2) + center.x, center.y, center.z);
            StartCoroutine(MoveToPosition(node.transform, snapPositions[i], 1.0f));
        }
    }

    void Update()
    {
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
                StartCoroutine(SwapPositions(selectedNode, node));
            } else {
                selectedNode.transform.localScale /= 1.2f;
                selectedNode = null;
            }
        }
    }

    IEnumerator SwapPositions(GameObject node1, GameObject node2) {
        Vector3 position1 = node1.transform.position;
        Vector3 position2 = node2.transform.position;

        StartCoroutine(MoveToPosition(node1.transform, position2, 0.25f));
        StartCoroutine(MoveToPosition(node2.transform, position1, 0.25f));

        yield return new WaitForSeconds(0.25f);

        if (selectedNode != null) {
            selectedNode.transform.localScale /= 1.2f;
            selectedNode = null;
        }
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

}
