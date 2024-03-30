using UnityEngine;

public interface ISwapValidator {
    void SetNumbersToBeSorted(int[] numbersToBeSorted);
    bool IsValidSwap(GameObject[] nodes, int index1, int index2, int startIndex);
}