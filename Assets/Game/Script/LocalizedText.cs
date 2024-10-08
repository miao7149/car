using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class LocalizedTextEntry
{
    public Language language;
    public string text;
}

public class LocalizedText : MonoBehaviour
{

    public List<LocalizedTextEntry> localizedTexts;

    private Text textComponent;

    void Start()
    {
        textComponent = GetComponent<Text>();
        UpdateText();
    }

    public void UpdateText()
    {
        foreach (var entry in localizedTexts)
        {
            if (entry.language == GlobalManager.Instance.CurrentLanguage)
            {
                textComponent.text = entry.text;
                break;
            }
        }
    }
}