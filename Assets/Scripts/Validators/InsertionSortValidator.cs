using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSortValidator : MonoBehaviour, ISwapValidator
{
    private int[] numbersToBeSorted;
    private int i = 1;
    private int j = 0;
    private int key;
    public bool IsValidSwap(GameObject[] nodes, int index1, int index2, int startIndex)
    {
        while (j < 0 || j >= 0 && numbersToBeSorted[j] < key)
        {
            i++;
            j = i - 1;
            if (i >= numbersToBeSorted.Length)
            {
                return false;
            }
            key = numbersToBeSorted[i];
        }
        if (index1 == j && index2 == j + 1 || index1 == j + 1 && index2 == j)
        {
            (numbersToBeSorted[j], numbersToBeSorted[j + 1]) = (numbersToBeSorted[j + 1], numbersToBeSorted[j]);
            j--;
            return true;
        }
        return false;
    }

    public void SetNumbersToBeSorted(int[] numbersToBeSorted)
    {
        this.numbersToBeSorted = numbersToBeSorted;
        key = numbersToBeSorted[i];
    }

    public (int, int) GetNextSwap(GameObject[] nodes) {
        return (-1, -1);
    }
}
