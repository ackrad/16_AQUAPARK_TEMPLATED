using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextController:MonoBehaviour
{
    protected Text textUI;
    protected TextMeshPro textMesh;
    protected TextMeshProUGUI textMeshUI;

    public string prefix = "";
    public string suffix = "";
    

    protected virtual void Awake()
    {
        textUI = GetComponent<Text>();
        textMesh = GetComponent<TextMeshPro>();
        textMeshUI = GetComponent<TextMeshProUGUI>();
    }

    public virtual void updateText(string text)
    {
        if (textUI != null)
        {
            textUI.text = prefix + text + suffix;
        }else if (textMesh != null)
        {
            textMesh.text = prefix + text + suffix;
        }
        else if (textMeshUI != null)
        {
            textMeshUI.text = prefix + text + suffix;
        }
        else
        {
            Debug.LogError("There are no any text component on this object!");
        }
    }
    

}
