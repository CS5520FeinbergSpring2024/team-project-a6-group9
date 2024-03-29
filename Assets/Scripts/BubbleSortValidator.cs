using UnityEngine;
using TMPro;

public class BubbleSortValidator : MonoBehaviour, ISwapValidator {
    public bool IsValidSwap(GameObject[] nodes, int index1, int index2) {
        if (index1 != index2 - 1) return false; 
        return int.Parse(nodes[index1].GetComponentInChildren<TextMeshPro>().text) >
               int.Parse(nodes[index2].GetComponentInChildren<TextMeshPro>().text);
    }

    public bool IsValidSwapInSelectionSort(GameObject[] nodes, int index1, int index2, int startIndex) {
        int minIndex = 0;

        while (startIndex < nodes.Length - 1) {
            minIndex = startIndex;
            for (int i = startIndex; i < nodes.Length; i++) {
                if (int.Parse(nodes[i].GetComponentInChildren<TextMeshPro>().text) <
                    int.Parse(nodes[minIndex].GetComponentInChildren<TextMeshPro>().text))
                {
                    minIndex = i;
                }
            }

            if (minIndex != startIndex) {
                break;
            }
            startIndex++;
        }

        if ((index1 == startIndex && index2 == minIndex) ||
            (index1 == minIndex && index2 == startIndex)) {
            return true;
        }
        return false;
    }
}
