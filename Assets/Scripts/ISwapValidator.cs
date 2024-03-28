using UnityEngine;

public interface ISwapValidator {
    bool IsValidSwap(GameObject[] nodes, int index1, int index2);
}