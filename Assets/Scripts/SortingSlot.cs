using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SortingSlot : MonoBehaviour, IDropHandler
{
    public Transform itemSlot;
    [SerializeField] private int idx;   // TODO: should not be serialized

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped.GetComponent<SortingItem>())
        {
            SortingItem droppedItem = dropped.GetComponent<SortingItem>();
            SortingSlot originalSortingSlot = droppedItem.GetOriginalParent().gameObject.GetComponentInParent<SortingSlot>();
            Debug.Log("coming from slot " + originalSortingSlot.GetIdx());
            // check whether it's the correct step
            droppedItem.SetParent(itemSlot);
        }
    }

    public int GetIdx()
    {
        return idx;
    }
}
