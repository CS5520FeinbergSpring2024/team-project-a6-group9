using UnityEngine;
using TMPro;
using static NodeController;

public class BubbleSortValidator : MonoBehaviour, ISwapValidator {
    private int lastSwapIndex = -1;
    private int sortedBoundary;
    private int[] numbersToBeSorted;

    void Start() {
        sortedBoundary = numbersToBeSorted.Length-1;
    }

    public bool IsValidSwap(GameObject[] nodes, int index1, int index2, int startIndex) {
        if (index1 != index2 - 1) return false;

        if (index1 >= sortedBoundary || index2 > sortedBoundary) return false;

        int checkStartIndex = lastSwapIndex >= 0 ? lastSwapIndex : 0;
        
        Debug.Log($"before swap: last swap index: {lastSwapIndex}; boundary: {sortedBoundary}");
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
                    Debug.Log($"after swap: last swap index: {lastSwapIndex}; boundary: {sortedBoundary}");
                    return true;
                } else {
                    return false;
                }
            } else if (lastSwapIndex == sortedBoundary - 1) {
                sortedBoundary--;
                lastSwapIndex = -1;
                return true;
            } else if (lastSwapIndex == sortedBoundary - 2) {
                sortedBoundary--;
                lastSwapIndex = -1;
                return true;
            } else if (lastSwapIndex == sortedBoundary - 3) {
                sortedBoundary--;
                lastSwapIndex = -1;
                return true;
            }
        }
        sortedBoundary--;
        lastSwapIndex = -1;
        return false;
    }

    public void SetNumbersToBeSorted(int[] numbersToBeSorted)
    {
        this.numbersToBeSorted = numbersToBeSorted;
    }

    public (int, int) GetNextSwap(GameObject[] nodes) {
        int tempLastSwapIndex = lastSwapIndex;
        int tempSortedBoundary = sortedBoundary;

        while (true) {
            int start = tempLastSwapIndex >= 0 ? tempLastSwapIndex : 0;

            for (int i = start; i < tempSortedBoundary; i++) {
                int currentValue = int.Parse(nodes[i].GetComponentInChildren<TextMeshPro>().text);
                int nextValue = int.Parse(nodes[i + 1].GetComponentInChildren<TextMeshPro>().text);

                if (currentValue > nextValue) {
                    return (i, i + 1);
                }
            }

            if (start > 0) {
                tempSortedBoundary--;
                tempLastSwapIndex = -1;

                if (tempSortedBoundary <= 0) {
                    break;
                }
            } else {
                break;
            }
        }

        return (-1, -1);
    }



}
