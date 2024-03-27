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
    private bool isDragging = false;
    private Vector3[] snapPositions;
    private Vector3 originalScale;


    void Start() {
        audioSource = GetComponentInChildren<AudioSource>();

        numNodes = 5;
        allNodes = new GameObject[numNodes];
        snapPositions = new Vector3[allNodes.Length];

        float screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        float spacing = screenWidth / (allNodes.Length + 1);

        for (int i = 0; i < allNodes.Length; i++) {
            GameObject node = Instantiate(nodePrefab, new Vector3(0, -50, 0), Quaternion.identity, this.transform);
            node.tag = "Node";
            allNodes[i] = node;

            int randomNumber = Random.Range(1, 100);
            TextMeshPro textComponent = node.GetComponentInChildren<TextMeshPro>();
            if (textComponent != null) {
                textComponent.text = randomNumber.ToString();
            }

            snapPositions[i] = new Vector3((i + 1) * spacing - (screenWidth / 2), -50, 0);
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
                selectedNode = hit.collider.gameObject;
                originalPosition = selectedNode.transform.position;
                originalScale = selectedNode.transform.localScale;
                isDragging = true;
                
                audioSource.PlayOneShot(dragSound);

                selectedNode.transform.localScale = originalScale * 1.1f;
                selectedNode.transform.position = new Vector3(selectedNode.transform.position.x, selectedNode.transform.position.y, selectedNode.transform.position.z - 0.5f); 
            }
        }

        if (selectedNode != null && isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedNode.transform.position = new Vector3(mousePosition.x, selectedNode.transform.position.y, selectedNode.transform.position.z);
        }

        if (Input.GetMouseButtonUp(0) && selectedNode != null)
        {
            isDragging = false;
            selectedNode.transform.localScale = originalScale;
            selectedNode.transform.position = new Vector3(selectedNode.transform.position.x, selectedNode.transform.position.y, originalPosition.z);
            HandleDrop();
            audioSource.PlayOneShot(dropSound);
            selectedNode = null;
        }
    }


    void OnMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.zero);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Node"))
        {
            selectedNode = hit.collider.gameObject;
            originalPosition = selectedNode.transform.position;
        }
    }

    void OnMouseUp()
    {
        if (selectedNode != null)
        {
            HandleDrop();
            selectedNode = null;
        }
    }

    private void HandleDrop()
    {
        int closestSnapIndex = FindClosestSnapPositionIndex(selectedNode.transform.position);
        int originalSnapIndex = FindClosestSnapPositionIndex(originalPosition);

        if (closestSnapIndex < originalSnapIndex)
        {
            PushNodesForward(closestSnapIndex);
        }
        else if (closestSnapIndex > originalSnapIndex)
        {
            PushNodesBackward(closestSnapIndex);
        }

        StartCoroutine(MoveToPosition(selectedNode.transform, snapPositions[closestSnapIndex], 0.25f));
    }

    private GameObject FindClosestNodeOnXAxis()
    {
        GameObject closestNode = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject node in allNodes)
        {
            if (node == selectedNode) continue;

            float distance = Mathf.Abs(node.transform.position.x - selectedNode.transform.position.x);

            if (distance < closestDistance && Mathf.Abs(node.transform.position.y - selectedNode.transform.position.y) < 0.1f) 
            {
                closestDistance = distance;
                closestNode = node;
            }
        }

        if (closestDistance <= swapThreshold)
        {
            return closestNode;
        }

        return null;
    }

    private void PushNodesForward(int snapIndex)
    {
        if (snapIndex >= snapPositions.Length - 1) return; 
        GameObject nodeAtPosition = FindNodeAtSnapPosition(snapIndex);
        if (nodeAtPosition != null)
        {
            PushNodesForward(snapIndex + 1);
            StartCoroutine(MoveToPosition(nodeAtPosition.transform, snapPositions[snapIndex + 1], 0.25f));
        }
    }

    
    private void PushNodesBackward(int snapIndex)
    {
        if (snapIndex <= 0) return; 
        GameObject nodeAtPosition = FindNodeAtSnapPosition(snapIndex);
        if (nodeAtPosition != null)
        {
            PushNodesBackward(snapIndex - 1);
            StartCoroutine(MoveToPosition(nodeAtPosition.transform, snapPositions[snapIndex - 1], 0.25f));
        }
    }

    private GameObject FindNodeAtSnapPosition(int snapIndex)
    {
        foreach (GameObject node in allNodes)
        {
            if (Vector3.Distance(node.transform.position, snapPositions[snapIndex]) < 0.1f)
            {
                return node;
            }
        }
        return null;
    }

    private int FindClosestSnapPositionIndex(Vector3 position)
    {
        int index = 0;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < snapPositions.Length; i++)
        {
            float distance = Vector3.Distance(position, snapPositions[i]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                index = i;
            }
        }

        return index;
    }


    IEnumerator SwapPositions(GameObject closestNode)
    {
        Vector3 targetPositionForCurrent = closestNode.transform.position;
        Vector3 targetPositionForClosest = selectedNode.transform.position;

        float elapsedTime = 0;
        float duration = 0.25f; 
        
        while (elapsedTime < duration)
        {
            selectedNode.transform.position = Vector3.Lerp(targetPositionForClosest, targetPositionForCurrent, (elapsedTime / duration));
            closestNode.transform.position = Vector3.Lerp(targetPositionForCurrent, targetPositionForClosest, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        selectedNode.transform.position = targetPositionForCurrent;
        closestNode.transform.position = targetPositionForClosest;
    }

    IEnumerator MoveToPosition(Transform objectTransform, Vector3 position, float duration)
    {
        Vector3 startPosition = objectTransform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            objectTransform.position = Vector3.Lerp(startPosition, position, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectTransform.position = position;
    }

}
