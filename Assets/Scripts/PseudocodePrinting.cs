using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PseudocodePrinting : MonoBehaviour
{
    public TextMeshProUGUI pseudocodeText;
    public string[] pseudocodeLines;
    public float characterDisplayDelay = 0.02f;

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
                if (line[i] == '<')
                {
                    int tagClose = line.IndexOf('>', i);
                    if (tagClose >= 0)
                    {
                        pseudocodeText.text += line.Substring(i, tagClose - i + 1);
                        i = tagClose;
                    }
                }
                else
                {
                    pseudocodeText.text += line[i];
                    yield return new WaitForSeconds(characterDisplayDelay);
                }
                i++;
            }
            pseudocodeText.text += "\n";
            yield return new WaitForSeconds(characterDisplayDelay); 
        }

    }
}
