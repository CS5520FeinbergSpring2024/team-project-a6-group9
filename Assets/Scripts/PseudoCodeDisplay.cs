using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PseudocodeDisplay : MonoBehaviour
{
    public TextMeshProUGUI pseudocodeText;
    public float characterDisplayDelay = 0.05f;
    private string[] pseudocodeLines = {
        "For each <color=#ffaabb>element</color>",
        "    <color=#11ffee>next</color> = next element",
        "    If  (<color=#ffaabb>element</color> > <color=#11ffee>next</color>)",
        "        swap(<color=#ffaabb>element</color>, <color=#11ffee>next</color>)"
    };

    void Start()
    {
        StartCoroutine(DisplayPseudocode());
    }

IEnumerator DisplayPseudocode()
{
    pseudocodeText.text = "";
    foreach (var line in pseudocodeLines)
    {
        int i = 0;
        while (i < line.Length)
        {
            // Check if the current character is the start of a tag
            if (line[i] == '<')
            {
                // Find the end of the tag
                int tagClose = line.IndexOf('>', i);
                if (tagClose >= 0)
                {
                    // Add the entire tag to the text without delay
                    pseudocodeText.text += line.Substring(i, tagClose - i + 1);
                    i = tagClose;
                }
            }
            else
            {
                // Add the character normally and apply delay
                pseudocodeText.text += line[i];
                yield return new WaitForSeconds(characterDisplayDelay);
            }
            i++;
        }
        pseudocodeText.text += "\n"; // Move to the next line after finishing a line
        yield return new WaitForSeconds(characterDisplayDelay); // Optional delay after finishing a line
    }
}

}

