using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    private GameObject[] allNodes;
    private GameObject selectedNode;
    private Vector2 originalPosition;
    private float swapThreshold = 1.0f;
    private bool isDragging = false;
    private Vector3[] snapPositions; 

    void Start()
    {
        allNodes = GameObject.FindGameObjectsWithTag("Node");
        
        snapPositions = new Vector3[allNodes.Length];
        float screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        float spacing = screenWidth / (snapPositions.Length + 1);

        for (int i = 0; i < snapPositions.Length; i++)
        {
            snapPositions[i] = new Vector3((i + 1) * spacing - (screenWidth / 2), 0, 0);
            
            if (i < allNodes.Length)
            {
                allNodes[i].transform.position = new Vector3(snapPositions[i].x, 0, 0) - new Vector3(screenWidth, 0, 0);
                StartCoroutine(MoveToPosition(allNodes[i].transform, snapPositions[i], 1.0f));
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Node"))
            {
                selectedNode = hit.collider.gameObject;
                originalPosition = selectedNode.transform.position;
                isDragging = true;
            }
        }

        if (selectedNode != null && isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedNode.transform.position = new Vector3(mousePosition.x, selectedNode.transform.position.y, selectedNode.transform.position.z);
        }

        if (Input.GetMouseButtonUp(0) && selectedNode != null)
        {
            isDragging = false;
            HandleDrop();
            selectedNode = null;
        }
    }


    void OnMouseDown()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

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

            if (distance < closestDistance && Mathf.Abs(node.transform.position.y - selectedNode.transform.position.y) < 0.1f) // Ensure they are roughly at the same y-level
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
