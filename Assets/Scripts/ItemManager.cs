using UnityEngine;

public class ItemManager : MonoBehaviour {
    private int hintCount = 15;
    private int autoCompleteCount = 2;
    private int lifeCount = 5;
    public int HintCount => hintCount;
    public int AutoCompleteCount => autoCompleteCount;
    public int LifeCount => lifeCount;

    private NodeController nodeController;

    void Start() {
        nodeController = FindObjectOfType<NodeController>();
        if (nodeController == null) {
            Debug.LogError("NodeController not found in the scene!");
        }
    }

    public bool ConsumeHint() {
        if (hintCount > 0) {
            hintCount--;
            return true;
        }
        Debug.Log("No hints left!");
        return false;
    }

    public bool ConsumeAutoComplete() {
        if (autoCompleteCount > 0) {
            autoCompleteCount--;
            return true;
        }
        Debug.Log("No auto-complete left!");
        return false;
    }

        public void AddItem(string itemName, int count) {
        switch (itemName) {
            case "Hint":
                hintCount += count;
                break;
            case "AutoComplete":
                autoCompleteCount += count;
                break;
            case "Life":
                lifeCount += count;
                break;
            default:
                Debug.LogError("Unknown item name");
                break;
        }
    }
}
