using UnityEngine;
using TMPro;
using static NodeController;

public class BubbleSortValidator : MonoBehaviour, ISwapValidator {
    private int lastSwapIndex = -1;
    private int sortedBoundary;

    void Start() {
        var nodeController = GetComponent<NodeController>();
        if (nodeController != null) {
            sortedBoundary = nodeController.numNodes - 1;
        } else {
            Debug.LogError("NodeController not found!");
        }
    }

    public bool IsValidSwap(GameObject[] nodes, int index1, int index2, int startIndex) {
        if (index1 != index2 - 1) return false;

        if (index1 >= sortedBoundary || index2 > sortedBoundary) return false;

        int checkStartIndex = lastSwapIndex >= 0 ? lastSwapIndex : 0;

        for (int i = checkStartIndex; i < sortedBoundary; i++) {
            int currentValue = int.Parse(nodes[i].GetComponentInChildren<TextMeshPro>().text);
            int nextValue = int.Parse(nodes[i + 1].GetComponentInChildren<TextMeshPro>().text);

            if (currentValue > nextValue) {
                if (i == index1 && (i + 1) == index2) {
                    lastSwapIndex = i + 1; 

                    if (lastSwapIndex == sortedBoundary) {
                        sortedBoundary--;
                        lastSwapIndex = -1;
                    }
                    return true;
                } else {
                    return false;
                }
            } else if (lastSwapIndex == sortedBoundary - 1) {
                sortedBoundary--;
                lastSwapIndex = -1;
                return true;
            }
        }
        sortedBoundary--;
        lastSwapIndex = -1;
        return false;
    }
}
