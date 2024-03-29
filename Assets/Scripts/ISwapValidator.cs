using UnityEngine;

public interface ISwapValidator {
    bool IsValidSwap(GameObject[] nodes, int index1, int index2);

    bool IsValidSwapInSelectionSort(GameObject[] nodes, int index1, int index2, int startIndex);
}