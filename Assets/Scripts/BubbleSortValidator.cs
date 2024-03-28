using UnityEngine;
using TMPro;

public class BubbleSortValidator : MonoBehaviour, ISwapValidator {
    public bool IsValidSwap(GameObject[] nodes, int index1, int index2) {
        if (index1 != index2 - 1) return false; 
        return int.Parse(nodes[index1].GetComponentInChildren<TextMeshPro>().text) >
               int.Parse(nodes[index2].GetComponentInChildren<TextMeshPro>().text);
    }
}